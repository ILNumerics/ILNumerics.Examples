using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;

// Smoke + render test for all examples.
//
//   SmokeRunner --root <repo> --out <results dir> --hook <SmokeHook.dll> [--config Release] [--delay 8000] [--timeout 120]
//               [--renderer GDI|OpenGL|default] [--only <path part>]
//
// For every project in ILNumerics.Examples.slnx (except tests/ and Benchmarks/) it starts the built example
// with no license key, lets it run, and classifies the outcome:
//   windowed apps  the startup hook captures a screenshot and renders the ILNumerics scene offscreen (GDI)
//   console apps   must exit with code 0
//   web apps       must answer HTTP requests; /Image is fetched as the render result
// A render counts as blank when (almost) every pixel has the background color.
// Example folders listed in tests/SmokeTest/known-failures.txt are reported but do not fail the run.

var opt = ParseArgs(args);
string root = Path.GetFullPath(opt.GetValueOrDefault("root", "."));
string outDir = Path.GetFullPath(opt.GetValueOrDefault("out", "smoke-results"));
string hook = Path.GetFullPath(opt["hook"]);
string config = opt.GetValueOrDefault("config", "Release");
int delay = int.Parse(opt.GetValueOrDefault("delay", "8000"));
int timeoutSec = int.Parse(opt.GetValueOrDefault("timeout", "120"));
string? only = opt.GetValueOrDefault("only");
// Renderer forced in all windowed examples. GDI (software) is deterministic and needs no GPU; pass "default" to
// keep ILNumerics' own choice (OpenGL with GDI fallback).
string renderer = opt.GetValueOrDefault("renderer", "GDI");

Directory.CreateDirectory(outDir);
var knownFailures = ReadList(Path.Combine(root, "tests", "SmokeTest", "known-failures.txt"));

var projects = XDocument.Load(Path.Combine(root, "ILNumerics.Examples.slnx"))
    .Descendants("Project").Select(p => (string)p.Attribute("Path")!)
    .Where(p => !p.StartsWith("tests/") && !p.StartsWith("Benchmarks/"))
    .Where(p => only == null || p.Contains(only, StringComparison.OrdinalIgnoreCase))
    .OrderBy(p => p).ToList();

var results = new List<Result>();
foreach (var rel in projects) {
    var r = RunOne(rel);
    r.KnownFailure = knownFailures.Contains(r.Name);
    results.Add(r);
    Console.WriteLine($"{(r.Passed ? "PASS" : r.KnownFailure ? "KNOWN" : "FAIL"),-5} {rel}  {r.Message}");
    if (!r.Passed) PrintDetails(r);
}

WriteSummary(results);
int newFailures = results.Count(r => !r.Passed && !r.KnownFailure);
var fixedNow = results.Where(r => r.Passed && r.KnownFailure).Select(r => r.Name).ToList();
if (fixedNow.Count > 0) Console.WriteLine("Now passing, remove from known-failures.txt: " + string.Join(", ", fixedNow));
Console.WriteLine($"{results.Count(r => r.Passed)} passed, {newFailures} failed, {results.Count(r => !r.Passed && r.KnownFailure)} known failures.");
return newFailures == 0 ? 0 : 1;

Result RunOne(string relProject) {
    var projPath = Path.Combine(root, relProject.Replace('/', Path.DirectorySeparatorChar));
    var projDir = Path.GetDirectoryName(projPath)!;
    var x = XDocument.Load(projPath);
    string Prop(string n) => x.Descendants(n).Select(e => e.Value.Trim()).LastOrDefault() ?? "";
    var exampleDir = Path.GetDirectoryName(relProject)!.Replace('\\', '/');
    var name = exampleDir.Replace('/', '_').Replace(' ', '_');   // unique file prefix for results
    var res = new Result { Project = relProject, Name = exampleDir };
    var asm = Prop("AssemblyName") is { Length: > 0 } a ? a : Path.GetFileNameWithoutExtension(projPath);
    var tfm = Prop("TargetFramework");
    var isWeb = (string?)x.Root!.Attribute("Sdk") == "Microsoft.NET.Sdk.Web";
    res.Kind = isWeb ? "web" : Prop("OutputType").Equals("WinExe", StringComparison.OrdinalIgnoreCase) ? "windowed" : "console";

    var binDir = Path.Combine(projDir, "bin", config, tfm);
    var exe = Path.Combine(binDir, asm + ".exe");
    if (!File.Exists(exe)) { res.Message = "executable not found: " + Path.GetRelativePath(root, exe); return res; }
    res.IlnVersion = FileVersionInfo.GetVersionInfo(Path.Combine(binDir, "ILNumerics.Core.dll")) is { } v && File.Exists(v.FileName) ? v.ProductVersion : null;

    foreach (var f in Directory.GetFiles(outDir, name + ".*")) File.Delete(f);
    res.Prefix = name;

    var psi = new ProcessStartInfo(exe) {
        WorkingDirectory = binDir, UseShellExecute = false,
        RedirectStandardOutput = true, RedirectStandardError = true, RedirectStandardInput = true,
    };
    psi.Environment["ILN_SMOKE_OUT"] = outDir;
    psi.Environment["ILN_SMOKE_NAME"] = name;
    psi.Environment["ILN_SMOKE_DELAY"] = delay.ToString();
    if (!renderer.Equals("default", StringComparison.OrdinalIgnoreCase)) psi.Environment["ILN_SMOKE_RENDERER"] = renderer;
    if (tfm.EndsWith("-windows")) psi.Environment["DOTNET_STARTUP_HOOKS"] = hook;
    int port = 0;
    if (isWeb) {
        port = 5100 + results.Count;
        psi.Environment["ASPNETCORE_URLS"] = $"http://127.0.0.1:{port}";
        psi.Environment["ASPNETCORE_ENVIRONMENT"] = "Development";
    }

    var output = new StringBuilder();
    using var p = new Process { StartInfo = psi };
    p.OutputDataReceived += (_, e) => { if (e.Data != null) lock (output) output.AppendLine(e.Data); };
    p.ErrorDataReceived += (_, e) => { if (e.Data != null) lock (output) output.AppendLine(e.Data); };
    var sw = Stopwatch.StartNew();
    p.Start(); p.BeginOutputReadLine(); p.BeginErrorReadLine();
    p.StandardInput.Close();

    string? renderFile = null;
    if (isWeb) {
        renderFile = ProbeWeb(port, name, res);
        if (!p.HasExited) p.Kill(true);
        p.WaitForExit(10_000);
    } else if (!p.WaitForExit(timeoutSec * 1000)) {
        p.Kill(true); p.WaitForExit(10_000);
        res.Message = $"timed out after {timeoutSec}s (hung, or waiting for input / a dialog)";
    }
    p.WaitForExit();
    res.Seconds = Math.Round(sw.Elapsed.TotalSeconds, 1);
    File.WriteAllText(Path.Combine(outDir, name + ".log.txt"), output.ToString());

    var errFile = Path.Combine(outDir, name + ".error.txt");
    if (File.Exists(errFile)) res.Message = FirstLines(File.ReadAllText(errFile), 3);
    var resultFile = Path.Combine(outDir, name + ".result.json");
    if (File.Exists(resultFile)) res.Details = File.ReadAllText(resultFile);
    if (output.ToString().Contains("licens", StringComparison.OrdinalIgnoreCase)) res.LicenseMention = true;

    if (res.Message != null) return res;
    if (!isWeb && p.ExitCode != 0) { res.Message = $"exit code {p.ExitCode}: " + FirstLines(output.ToString(), 3); return res; }
    if (res.Kind == "windowed") {
        if (res.Details == null) { res.Message = "no window was captured"; return res; }
        using var doc = JsonDocument.Parse(res.Details);
        if (!doc.RootElement.TryGetProperty("renders", out var renders) || renders.GetArrayLength() == 0) {
            res.Passed = true; res.Message = "window shown, no ILNumerics panel found"; return res;
        }
        renderFile = renders[0].GetProperty("file").GetString();
    }
    if (renderFile != null) {
        var path = Path.Combine(outDir, renderFile);
        if (!File.Exists(path)) { res.Message ??= "no render produced"; return res; }
        res.Render = renderFile;
        res.Coverage = Coverage(path);
        if (res.Coverage < 0.5) { res.Message = $"render looks blank ({res.Coverage:0.00}% non-background pixels)"; return res; }
    }
    if (isWeb && res.Message != null) return res;
    res.Passed = true;
    res.Message ??= res.Coverage is double c ? $"{c:0.0}% of render covered" : "ok";
    return res;
}

// Full diagnostics for a failed example, in a collapsible group of the CI log.
void PrintDetails(Result r) {
    Console.WriteLine($"::group::Details {r.Name}");
    foreach (var ext in new[] { ".error.txt", ".result.json", ".log.txt" }) {
        var f = Path.Combine(outDir, r.Prefix + ext);
        if (!File.Exists(f)) continue;
        var lines = File.ReadAllLines(f);
        Console.WriteLine($"--- {ext} ---");
        foreach (var l in lines.Take(80)) Console.WriteLine(l);
        if (lines.Length > 80) Console.WriteLine($"... ({lines.Length - 80} more lines)");
    }
    Console.WriteLine("::endgroup::");
}

string? ProbeWeb(int port, string name, Result res) {
    using var http = new HttpClient { Timeout = TimeSpan.FromSeconds(30) };
    var deadline = DateTime.UtcNow.AddSeconds(timeoutSec);
    while (DateTime.UtcNow < deadline) {
        try {
            var home = http.GetAsync($"http://127.0.0.1:{port}/").Result;
            if (!home.IsSuccessStatusCode) { res.Message = $"GET / returned {(int)home.StatusCode}"; return null; }
            var img = http.GetAsync($"http://127.0.0.1:{port}/Image?rotX=10&rotY=20&rotZ=30").Result;
            if (!img.IsSuccessStatusCode) { res.Message = $"GET /Image returned {(int)img.StatusCode}"; return null; }
            var file = name + ".render.png";
            File.WriteAllBytes(Path.Combine(outDir, file), img.Content.ReadAsByteArrayAsync().Result);
            return file;
        } catch (AggregateException) { Thread.Sleep(1000); }
    }
    res.Message = "web app did not answer";
    return null;
}

// Percentage of pixels that differ from the most common (background) color.
static double Coverage(string png) {
    using var bmp = new Bitmap(png);
    var counts = new Dictionary<int, int>();
    int total = 0;
    for (int y = 0; y < bmp.Height; y += 2)
        for (int x = 0; x < bmp.Width; x += 2) {
            int argb = bmp.GetPixel(x, y).ToArgb();
            counts[argb] = counts.GetValueOrDefault(argb) + 1; total++;
        }
    return total == 0 ? 0 : 100.0 * (total - counts.Values.Max()) / total;
}

void WriteSummary(List<Result> rs) {
    var ver = rs.Select(r => r.IlnVersion).FirstOrDefault(v => v != null) ?? "unknown";
    var sb = new StringBuilder();
    sb.AppendLine($"## Example smoke test (ILNumerics {ver}, .NET {Environment.Version}, renderer {renderer}, no license key)");
    sb.AppendLine();
    sb.AppendLine($"{rs.Count(r => r.Passed)} passed, {rs.Count(r => !r.Passed && !r.KnownFailure)} failed, {rs.Count(r => !r.Passed && r.KnownFailure)} known failures. Screenshots and renders are in the `smoke-results` artifact.");
    sb.AppendLine();
    sb.AppendLine("| Result | Example | Kind | Time | Details |");
    sb.AppendLine("|---|---|---|---|---|");
    foreach (var r in rs.OrderBy(r => r.Passed).ThenBy(r => r.Project)) {
        var status = r.Passed ? (r.KnownFailure ? "✅ (fixed?)" : "✅") : r.KnownFailure ? "⚠️ known" : "❌";
        var msg = (r.Message ?? "").Replace("|", "\\|").Replace("\r", "").Replace("\n", "<br>");
        if (r.LicenseMention) msg += " · output mentions licensing";
        sb.AppendLine($"| {status} | {r.Project} | {r.Kind} | {r.Seconds}s | {msg} |");
    }
    File.WriteAllText(Path.Combine(outDir, "summary.md"), sb.ToString());
    File.WriteAllText(Path.Combine(outDir, "results.json"), JsonSerializer.Serialize(rs, new JsonSerializerOptions { WriteIndented = true }));
    var gh = Environment.GetEnvironmentVariable("GITHUB_STEP_SUMMARY");
    if (!string.IsNullOrEmpty(gh)) File.AppendAllText(gh, sb.ToString());
}

static string FirstLines(string s, int n) => string.Join(" ", s.Split('\n').Select(l => l.Trim()).Where(l => l.Length > 0).Take(n));

static HashSet<string> ReadList(string path) => File.Exists(path)
    ? File.ReadAllLines(path).Select(l => l.Split('#')[0].Trim()).Where(l => l.Length > 0).ToHashSet(StringComparer.OrdinalIgnoreCase)
    : new HashSet<string>();

static Dictionary<string, string> ParseArgs(string[] a) {
    var d = new Dictionary<string, string>();
    for (int i = 0; i + 1 < a.Length; i += 2) d[a[i].TrimStart('-')] = a[i + 1];
    return d;
}

class Result {
    public string Project { get; set; } = "";
    public string Name { get; set; } = "";   // example folder, e.g. Plotting/BarPlotExample
    public string Prefix { get; set; } = ""; // result file prefix
    public string Kind { get; set; } = "";
    public bool Passed { get; set; }
    public bool KnownFailure { get; set; }
    public string? Message { get; set; }
    public double Seconds { get; set; }
    public double? Coverage { get; set; }
    public string? Render { get; set; }
    public string? IlnVersion { get; set; }
    public bool LicenseMention { get; set; }
    public string? Details { get; set; }
}
