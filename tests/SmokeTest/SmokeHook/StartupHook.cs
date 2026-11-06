using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;

/// <summary>
/// .NET startup hook (see DOTNET_STARTUP_HOOKS). Must live in the global namespace and be named StartupHook.
/// Controlled by environment variables set by the smoke runner:
///   ILN_SMOKE_OUT    output directory for result files (hook does nothing when unset)
///   ILN_SMOKE_NAME   base file name for this example's results
///   ILN_SMOKE_DELAY  milliseconds to let the example run before capturing (default 8000)
///   ILN_SMOKE_RENDERER  optional ILNumerics renderer to force for all panels (e.g. GDI, OpenGL)
/// </summary>
internal class StartupHook {

    static string? s_outDir;
    static string s_name = "example";
    static int s_failed;

    public static void Initialize() {
        s_outDir = Environment.GetEnvironmentVariable("ILN_SMOKE_OUT");
        if (string.IsNullOrEmpty(s_outDir)) return;
        Directory.CreateDirectory(s_outDir);
        s_name = Environment.GetEnvironmentVariable("ILN_SMOKE_NAME") ?? s_name;
        int delay = int.TryParse(Environment.GetEnvironmentVariable("ILN_SMOKE_DELAY"), out var d) ? d : 8000;

        AppDomain.CurrentDomain.UnhandledException += (s, e) => Fail("Unhandled exception", e.ExceptionObject);
        // Record the first exceptions thrown inside ILNumerics / OpenTK, even if they are caught: they explain
        // renderer fallbacks and blank panels.
        AppDomain.CurrentDomain.FirstChanceException += (s, e) => {
            try {
                var src = e.Exception.TargetSite?.DeclaringType?.FullName ?? "";
                if (!(src.StartsWith("ILNumerics.Drawing") || src.StartsWith("OpenTK"))) return;
                lock (s_firstChance) if (s_firstChance.Count < 8) s_firstChance.Add($"{e.Exception.GetType().Name} in {src}.{e.Exception.TargetSite?.Name}: {e.Exception.Message}");
            } catch { }
        };
        try {
            // WinForms would otherwise show its "continue / quit" dialog and keep the process alive.
            System.Windows.Forms.Application.SetUnhandledExceptionMode(System.Windows.Forms.UnhandledExceptionMode.ThrowException);
        } catch (Exception exc) {
            Log("could not set WinForms exception mode: " + exc.Message);
        }

        try {
            ConfigureIlNumerics(Environment.GetEnvironmentVariable("ILN_SMOKE_RENDERER"));
        } catch (Exception exc) {
            Log("could not configure ILNumerics: " + exc.Message);
        }

        var t = new Thread(() => CaptureLoop(delay)) { IsBackground = true, Name = "ILN smoke capture" };
        t.Start();
    }

    // Separate method, so a missing ILNumerics.Drawing assembly only fails this call.
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
    static void ConfigureIlNumerics(string? renderer) {
        // A modal "falling back to GDI" message box would block the unattended run.
        ILNumerics.Settings.ShowMessageBoxOnGDIFallback = false;
        if (!string.IsNullOrEmpty(renderer)) {
            ILNumerics.Settings.DefaultRenderer = Enum.Parse<ILNumerics.Drawing.RendererTypes>(renderer, ignoreCase: true);
        }
    }

    static readonly List<string> s_dialogs = new();
    static readonly List<string> s_firstChance = new();

    static void CaptureLoop(int delay) {
        var sw = System.Diagnostics.Stopwatch.StartNew();
        while (sw.ElapsedMilliseconds < delay) { DismissDialogs(); Thread.Sleep(500); }
        // slow examples (e.g. generating data first): keep looking for the main window; the runner enforces a timeout
        for (int i = 0; ; i++) {
            DismissDialogs();
            try {
                if (TryCaptureWinForms() || TryCaptureWpf()) {
                    Environment.Exit(Volatile.Read(ref s_failed) == 0 ? 0 : 3);
                }
            } catch (Exception exc) {
                Fail("Capture failed", exc);
                Environment.Exit(3);
            }
            if (i == 10) Log("no window found yet");
            Thread.Sleep(1000);
        }
    }

    #region Dialogs
    // Many examples greet the user with a MessageBox. Close such dialogs (class #32770) of this process
    // so the main window can show up; their text is recorded in the result.
    [System.Runtime.InteropServices.DllImport("user32.dll")] static extern bool EnumWindows(EnumProc cb, IntPtr l);
    [System.Runtime.InteropServices.DllImport("user32.dll")] static extern bool EnumChildWindows(IntPtr parent, EnumProc cb, IntPtr l);
    [System.Runtime.InteropServices.DllImport("user32.dll")] static extern uint GetWindowThreadProcessId(IntPtr h, out uint pid);
    [System.Runtime.InteropServices.DllImport("user32.dll")] static extern bool IsWindowVisible(IntPtr h);
    [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Unicode)] static extern int GetClassName(IntPtr h, System.Text.StringBuilder s, int n);
    [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Unicode)] static extern int GetWindowText(IntPtr h, System.Text.StringBuilder s, int n);
    [System.Runtime.InteropServices.DllImport("user32.dll")] static extern IntPtr PostMessage(IntPtr h, uint msg, IntPtr w, IntPtr l);
    delegate bool EnumProc(IntPtr h, IntPtr l);

    static void DismissDialogs() {
        try {
            uint me = (uint)Environment.ProcessId;
            EnumWindows((h, _) => {
                GetWindowThreadProcessId(h, out var pid);
                if (pid != me || !IsWindowVisible(h)) return true;
                var cls = new System.Text.StringBuilder(64); GetClassName(h, cls, 64);
                if (cls.ToString() != "#32770") return true;
                var text = new System.Text.StringBuilder();
                EnumChildWindows(h, (c, __) => {
                    var t = new System.Text.StringBuilder(1024); GetWindowText(c, t, 1024);
                    if (t.Length > 0) text.Append(t).Append(' ');
                    return true;
                }, IntPtr.Zero);
                lock (s_dialogs) s_dialogs.Add(text.ToString().Trim());
                Log("closing dialog: " + text);
                PostMessage(h, 0x0010 /* WM_CLOSE */, IntPtr.Zero, IntPtr.Zero);
                return true;
            }, IntPtr.Zero);
        } catch (Exception exc) { Log("dialog check failed: " + exc.Message); }
    }
    #endregion

    #region WinForms
    static bool TryCaptureWinForms() {
        var forms = System.Windows.Forms.Application.OpenForms;
        if (forms.Count == 0) return false;
        var form = forms[0]!;
        var result = new Dictionary<string, object?> { ["ui"] = "WinForms", ["window"] = form.GetType().FullName, ["title"] = null };
        form.Invoke(() => {
            result["title"] = form.Text;
            // modal dialogs (MessageBox etc.) on top of the main form are a hint that something went wrong
            result["openForms"] = forms.Cast<System.Windows.Forms.Form>().Select(f => f.GetType().FullName + ": " + f.Text).ToArray();
            try {
                form.WindowState = System.Windows.Forms.FormWindowState.Normal;
                form.TopMost = true; form.Activate(); form.Refresh();
                System.Windows.Forms.Application.DoEvents();
                SaveScreen(form.Bounds, Path.Combine(s_outDir!, s_name + ".screen.png"));
            } catch (Exception exc) { result["screenError"] = exc.Message; }

            var panels = FindPanels(form).ToList();
            result["panels"] = panels.Select(p => p.GetType().FullName).ToArray();
            result["screen"] = System.Windows.Forms.Screen.FromControl(form).Bounds.ToString();
            result["formBounds"] = form.Bounds.ToString();
            result["deviceDpi"] = form.DeviceDpi;
            for (int i = 0; i < panels.Count; i++) {
                var c = (System.Windows.Forms.Control)panels[i];
                result["panelGeometry" + Suffix(i)] = DescribeWindows(c);
                try { SaveScreen(c.RectangleToScreen(c.ClientRectangle), Path.Combine(s_outDir!, s_name + Suffix(i) + ".panel.png")); }
                catch (Exception exc) { result["panelScreenError"] = exc.Message; }
            }
            RenderPanels(panels, result);
        });
        WriteResult(result);
        return true;
    }

    [System.Runtime.InteropServices.DllImport("user32.dll")] static extern bool GetWindowRect(IntPtr h, out RECT r);
    [System.Runtime.InteropServices.DllImport("user32.dll")] static extern uint GetDpiForWindow(IntPtr h);
    struct RECT { public int Left, Top, Right, Bottom; }

    // Diagnostics: the panel's screen rectangle and those of all native child windows (e.g. the GLFW window
    // inside the OpenGL control), so a native window that does not follow the panel's size shows up in the result.
    static string[] DescribeWindows(System.Windows.Forms.Control c) {
        var list = new List<string> { $"panel {c.RectangleToScreen(c.ClientRectangle)} dpi {SafeDpi(c.Handle)}" };
        EnumChildWindows(c.Handle, (h, _) => {
            var cls = new System.Text.StringBuilder(64); GetClassName(h, cls, 64);
            GetWindowRect(h, out var r);
            list.Add($"{cls} visible={IsWindowVisible(h)} {{X={r.Left},Y={r.Top},Width={r.Right - r.Left},Height={r.Bottom - r.Top}}} dpi {SafeDpi(h)}");
            return true;
        }, IntPtr.Zero);
        return list.ToArray();
    }
    static uint SafeDpi(IntPtr h) { try { return GetDpiForWindow(h); } catch { return 0; } }

    static IEnumerable<object> FindPanels(System.Windows.Forms.Control c) {
        foreach (System.Windows.Forms.Control child in c.Controls) {
            if (IsIlnPanel(child)) yield return child;
            foreach (var p in FindPanels(child)) yield return p;
        }
    }
    #endregion

    #region WPF
    static bool TryCaptureWpf() {
        System.Windows.Application? app;
        try { app = System.Windows.Application.Current; } catch { return false; }
        if (app == null) return false;
        var result = new Dictionary<string, object?> { ["ui"] = "WPF" };
        bool found = false;
        app.Dispatcher.Invoke(() => {
            var win = app.MainWindow;
            if (win == null || !win.IsLoaded) return;
            found = true;
            result["window"] = win.GetType().FullName; result["title"] = win.Title;
            try {
                win.Topmost = true; win.Activate();
                var src = System.Windows.PresentationSource.FromVisual(win);
                double sx = src?.CompositionTarget?.TransformToDevice.M11 ?? 1, sy = src?.CompositionTarget?.TransformToDevice.M22 ?? 1;
                var rect = new Rectangle((int)(win.Left * sx), (int)(win.Top * sy), (int)(win.ActualWidth * sx), (int)(win.ActualHeight * sy));
                SaveScreen(rect, Path.Combine(s_outDir!, s_name + ".screen.png"));
            } catch (Exception exc) { result["screenError"] = exc.Message; }

            var panels = new List<object>();
            CollectWpf(win, panels);
            result["panels"] = panels.Select(p => p.GetType().FullName).ToArray();
            for (int i = 0; i < panels.Count; i++) {
                try {
                    Rectangle r;
                    if (panels[i] is System.Windows.Forms.Control c) r = c.RectangleToScreen(c.ClientRectangle);
                    else {
                        var fe = (System.Windows.FrameworkElement)panels[i];
                        var tl = fe.PointToScreen(new System.Windows.Point(0, 0));
                        var br = fe.PointToScreen(new System.Windows.Point(fe.ActualWidth, fe.ActualHeight));
                        r = Rectangle.FromLTRB((int)tl.X, (int)tl.Y, (int)br.X, (int)br.Y);
                    }
                    SaveScreen(r, Path.Combine(s_outDir!, s_name + Suffix(i) + ".panel.png"));
                } catch (Exception exc) { result["panelScreenError"] = exc.Message; }
            }
            RenderPanels(panels, result);
        });
        if (found) WriteResult(result);
        return found;
    }

    static void CollectWpf(object node, List<object> panels) {
        if (IsIlnPanel(node)) { panels.Add(node); return; }
        if (node is System.Windows.Forms.Integration.WindowsFormsHost host && host.Child != null) {
            if (IsIlnPanel(host.Child)) panels.Add(host.Child); else panels.AddRange(FindPanels(host.Child));
            return;
        }
        if (node is System.Windows.DependencyObject dobj) {
            foreach (var child in System.Windows.LogicalTreeHelper.GetChildren(dobj)) CollectWpf(child, panels);
        }
    }
    #endregion

    #region ILNumerics rendering
    static bool IsIlnPanel(object o) => o is ILNumerics.Drawing.Controls.WinForms.Panel || o is ILNumerics.Drawing.Controls.WPF.Panel;

    // Configure() first: it synchronizes changes the example made to panel.Scene into the scene the panel renders.
    static ILNumerics.Drawing.Scene CurrentScene(object panel) => panel switch {
        ILNumerics.Drawing.Controls.WinForms.Panel p => ConfigureAnd(p.Configure, p.GetCurrentScene),
        ILNumerics.Drawing.Controls.WPF.Panel p => ConfigureAnd(p.Configure, p.GetCurrentScene),
        _ => throw new ArgumentException("not an ILNumerics panel")
    };

    static ILNumerics.Drawing.Scene ConfigureAnd(Action configure, Func<long, ILNumerics.Drawing.Scene> get) {
        configure();
        return get(5000);
    }

    static ILNumerics.Drawing.Scene UserScene(object panel) => panel switch {
        ILNumerics.Drawing.Controls.WinForms.Panel p => p.Scene,
        ILNumerics.Drawing.Controls.WPF.Panel p => p.Scene,
        _ => throw new ArgumentException("not an ILNumerics panel")
    };

    static string? DriverName(object panel) => panel switch {
        ILNumerics.Drawing.Controls.WinForms.Panel p => p.RendererType.ToString(),
        ILNumerics.Drawing.Controls.WPF.Panel p => p.RendererType.ToString(),
        _ => null
    };

    /// <summary>Renders each panel's current scene offscreen with the software (GDI) renderer. Works without a GPU.</summary>
    static void RenderPanels(List<object> panels, Dictionary<string, object?> result) {
        var renders = new List<object>();
        for (int i = 0; i < panels.Count; i++) {
            var file = s_name + (i == 0 ? "" : "." + i) + ".render.png";
            try {
                result["driver" + (i == 0 ? "" : "." + i)] = DriverName(panels[i]);
                var scene = CurrentScene(panels[i]);
                scene.Configure();
                result["drawables" + (i == 0 ? "" : "." + i)] = scene.Find<ILNumerics.Drawing.Drawable>().Count();
                using (var drv = new ILNumerics.Drawing.GDIDriver(800, 600, scene, Color.White)) {
                    drv.Render();
                    drv.BackBuffer.SaveBitmap(Path.Combine(s_outDir!, file));
                }
                renders.Add(new { file, ok = true });
                // Diagnostics: the same, from the panel's (user side) Scene instead of the panel's render scene.
                try {
                    var userScene = UserScene(panels[i]);
                    using var drv2 = new ILNumerics.Drawing.GDIDriver(800, 600, userScene, Color.White);
                    drv2.Render();
                    drv2.BackBuffer.SaveBitmap(Path.Combine(s_outDir!, s_name + Suffix(i) + ".userscene.png"));
                } catch (Exception exc) { result["userSceneError" + Suffix(i)] = exc.Message; }
            } catch (Exception exc) {
                renders.Add(new { file, ok = false, error = exc.ToString() });
                Interlocked.Exchange(ref s_failed, 1);
            }
        }
        result["renders"] = renders;
    }
    #endregion

    #region helpers
    static string Suffix(int i) => i == 0 ? "" : "." + i;

    static void SaveScreen(Rectangle r, string path) {
        if (r.Width <= 0 || r.Height <= 0) return;
        using var bmp = new Bitmap(r.Width, r.Height);
        using (var g = Graphics.FromImage(bmp)) g.CopyFromScreen(r.Location, Point.Empty, r.Size);
        bmp.Save(path, ImageFormat.Png);
    }

    static void WriteResult(Dictionary<string, object?> result) {
        lock (s_dialogs) if (s_dialogs.Count > 0) result["dialogs"] = s_dialogs.ToArray();
        lock (s_firstChance) if (s_firstChance.Count > 0) result["exceptions"] = s_firstChance.ToArray();
        File.WriteAllText(Path.Combine(s_outDir!, s_name + ".result.json"),
            JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true }));
    }

    static void Fail(string what, object? exc) {
        Interlocked.Exchange(ref s_failed, 1);
        string earlier = "";
        lock (s_firstChance) if (s_firstChance.Count > 0) earlier = Environment.NewLine + "Earlier ILNumerics/OpenTK exceptions:" + Environment.NewLine + "  " + string.Join(Environment.NewLine + "  ", s_firstChance);
        try { File.AppendAllText(Path.Combine(s_outDir!, s_name + ".error.txt"), $"{what}:{Environment.NewLine}{exc}{earlier}{Environment.NewLine}"); } catch { }
    }

    static void Log(string msg) => Console.Error.WriteLine("[iln-smoke] " + msg);
    #endregion
}
