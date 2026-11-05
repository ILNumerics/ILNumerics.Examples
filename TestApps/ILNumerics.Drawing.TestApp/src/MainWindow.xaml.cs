using ILNumerics.Drawing;
using ILNumerics.Drawing.Tests;
using ILNumerics.DrawingTestApplication;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ILNumerics.VisualDrawingTestApp.Windows {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        Drawing.Controls.WinForms.Panel ilnPanel_WF_GDI = new Drawing.Controls.WinForms.Panel() { RendererType = Drawing.RendererTypes.GDI };
        Drawing.Controls.WinForms.Panel ilnPanel_WF_OGL = new Drawing.Controls.WinForms.Panel() { RendererType = Drawing.RendererTypes.OpenGL };
        Scene m_scene; 

        public MainWindow() {
            //GDIDriver.IsGDIPlusSupported = false;
            Settings.ThrowWhenStarterPlan = true; 
            InitializeComponent();
            winFormsHost1.Child = ilnPanel_WF_GDI;
            winFormsHost2.Child = ilnPanel_WF_OGL;
            var sceneFactory = () => new Scene() {
                Camera = { new Sphere() }
            };
            refreshPanels(sceneFactory);
            winFormsHost1.InvalidateVisual();
            winFormsHost2.InvalidateVisual(); 
        }

        void refreshPanels(Func<Scene> sceneFactory) {

            ilnPanel_WF_GDI.Scene = sceneFactory();
            ilnPanel_WF_OGL.Scene = sceneFactory();
            ilnPanel_WPFGDI.Scene = sceneFactory();
            ilnPanel_WPFOGL.Scene = sceneFactory();
#if NETFRAMEWORK
            var fwvers = "net461"; 
#else 
            var fwvers = "net8.0-windows";
#endif
            // text rendering info
            var gdiText = GDIDriver.IsGDIPlusSupported ? "Text: GDI+" : "Text: SkiaSharp"; 

            title(ilnPanel_WF_GDI.Scene, $"WinForms, {ilnPanel_WF_GDI.RendererType}, {fwvers}, {gdiText}"); 
            title(ilnPanel_WF_OGL.Scene, $"WinForms, {ilnPanel_WF_OGL.RendererType}, {fwvers}, {gdiText}"); 
            title(ilnPanel_WPFGDI.Scene, $"WPF, {ilnPanel_WPFGDI.RendererType}, {fwvers}, {gdiText}"); 
            title(ilnPanel_WPFOGL.Scene, $"WPF, {ilnPanel_WPFOGL.RendererType}, {fwvers}, {gdiText}"); 

            void title(Scene scene, string text) {
                var label = scene.Screen.First<Drawing.Label>(tag: "stat"); 
                if (label == null) {
                    label = new Drawing.Label() { Tag = "stats", Anchor = new System.Drawing.PointF(0, 0), Position = new Vector3() };
                    scene.Screen.Add(label); 
                }
                label.Text = text;
            }
            ilnPanel_WF_GDI.Scene.Configure();
            ilnPanel_WF_OGL.Scene.Configure();
            ilnPanel_WPFGDI.Scene.Configure();
            ilnPanel_WPFOGL.Scene.Configure();
            ilnPanel_WF_GDI.Invalidate();
            ilnPanel_WF_OGL.Invalidate(); 
            ilnPanel_WPFGDI.Invalidate(); 
            ilnPanel_WPFOGL.Invalidate(); 
        }

        private void listScenes_Selected(object sender, RoutedEventArgs e) {
            var kv = listScenes.SelectedValue as TestScene;
            if (kv != null) {

                var sceneFactory = () => ScenesCollection.Instance.GetSceneByName(kv.GetType().Name).scene;
                
                refreshPanels(sceneFactory); 

            }
        }

        //private void treeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e) {
        //    var node = treeView.SelectedItem;

        //    this.Title = node.Text;
        //    currTestName = node.Text;
        //    var scene = tApp.GetSceneByName(currTestName);
        //    if (scene != null) {
        //        ilPanel1.Scene = scene;
        //        ilPanel1.Scene.Configure();
        //        ilPanel1.Refresh();

        //        ilPanel2.Scene = tApp.GetSceneByName(currTestName, "[GDI Renderer]");
        //        ilPanel2.Scene.Configure();
        //        ilPanel2.Refresh();

        //        textBox1.Text = tApp.getSceneDescription;
        //    }

        //}
    }
}