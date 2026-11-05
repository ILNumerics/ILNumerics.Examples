using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using ILNumerics;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;
using static ILNumerics.Globals;
using static ILNumerics.ILMath; 

namespace ILNumerics.DrawingTestApplication.TestClasses.AntialiasingTests.Complex
{
    class TextRenderStress : TestScene
    {
        const int NR_LABELS = 99; 
        public TextRenderStress() : base(
            TestCategory.SG_TextRendering,
            "Many label rendering",
            @"[TestCase]:
500 labels are created on the scene with random text and size. Clicking on 
the scene will cause those labels to be disposed and another 500 labels to 
be created. The purpose is to stress the graphics hardware texture buffers 
and the ILNumerics text rendering resource implementation. Make sure that 
all labels show reasonable chars and or digits only. No flickering shall occur!")
        { }

        public override Scene GetScene()
        {
            // create scene
            var scene = new Scene();
            // create first plot cube (upper left)
            var plotCube1 = scene.Add(new PlotCube(
                new LinePlot(tosingle(rand(3, 100)))));

            scene.MouseClick += Scene_MouseClick;
            generateLabels(scene);
            return scene;

        }
        private void Scene_MouseClick(object sender, ILNumerics.Drawing.MouseEventArgs e) {
            var scene = sender as Scene;
            generateLabels(scene);
            e.Cancel = true;
            e.Refresh = true; 
        }
        private void generateLabels(Scene scene) {
            var labels = scene.Find<Drawing.Label>("delete").ToArray(); 
            foreach (var l in labels) {
                l.Parent.Remove(l);
                l.Dispose(); 
            }
            Array<float> pos = tosingle(rand(2, NR_LABELS));
            Array<int> r = toint32(rand(4, NR_LABELS) * vector<double>(Chars.Length, sizes.Length, fonts.Length, conf.Length));
            var pc = scene.First<PlotCube>(); 
            for (int i = 0; i < NR_LABELS; i++) {
                var l = new Drawing.Label(tag: "delete");
                l.Position = new Vector3(pos.GetValue(0, i), pos.GetValue(1, i),0.5f);
                generateText(r[full, i], l);
                pc.Add(l); 
            }
            pc.Limits.Set(new Vector3(0,0,0), new Vector3(1, 1, 1));

            //scene.Configure(); 

        }

        string Chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789°+-=*/^!\"§$%&()?´`~#_.;<>";
        int[] sizes = new[] { 6, 6, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17,18,19,20,21,22,23,24,25 };
        string[] fonts = new string[] { "Courier", "Arial", "Helvetica", "Comic Sans MS", "Consolas", "Wingdings" };
        FontStyle[] conf = new[] { FontStyle.Regular, FontStyle.Bold, FontStyle.Italic, FontStyle.Underline,
                            FontStyle.Italic | FontStyle.Bold, FontStyle.Italic | FontStyle.Underline,
                            FontStyle.Underline | FontStyle.Bold, FontStyle.Bold| FontStyle.Italic | FontStyle.Underline }; 
        void generateText(InArray<int> r, ILNumerics.Drawing.Label l) {
            using (Scope.Enter(r)) {
                // sample single char from all variations
                var c = Chars[r.GetValue(0)];
                var s = sizes[r.GetValue(1)];
                var f = fonts[r.GetValue(2)];
                var sty = conf[r.GetValue(3)];
                l.Font = new Font(f, s, sty);
                if (l.Font == null) {
                    Debugger.Break(); 
                }
                l.Text = $"{c}"; 
            }
        }
        
    }
}
