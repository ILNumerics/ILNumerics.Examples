using System.Drawing;
using ILNumerics;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;
using static ILNumerics.Globals;
using static ILNumerics.ILMath;

namespace ILNumerics.DrawingTestApplication.TestClasses.AntialiasingTests
{
    class TriangleLinesAAFill : TestScene
    {
        public TriangleLinesAAFill() : base(
            TestCategory.RA_AntialiasedTriangles,
            "Triangle Strip Anti-aliased vs Border Line",
            @"[TestCase]:
Switch to GDI Render(Triangles anti-aliasing is only in GDI available).
Check if the anti-aliased triangle is drawn correctly.
No green transparent pixel must be drawn outside the line border.")
        { }

        public override Scene GetScene()
        {
            var scene = new Scene();
            Array<float> Tri = array(new float[,] { { -1.0f, 0.3f, 0.0f }, { 0.0f, 0.0f, 0.0f }, { 1.0f, -0.3f, 0.0f } }).T;

            scene.Camera.Add(new LineStrip() { Positions = Tri, Width = 2, Antialiasing = true, Color = Color.Black, Visible = true, DashStyle = DashStyle.Solid/*, Pattern = -1^1, PatternScale = 1.0f */});
            
            //scene.Camera.Add(new Triangles() { Positions = Tri, Color = Color.FromArgb(128, Color.Blue) });

            /*Array<float> TriBottom = new float[,] { { -0.5f, -0.5f, 0.0f }, { 0.0f, -1.5f, 0.0f }, { 0.5f, -0.5f, 0.0f } };
            scene.Camera.Add(new LineStrip() { Positions = TriBottom, Width = 1, Antialiasing = false, Color = Color.FromArgb(128, Color.Red), Visible = true });
            scene.Camera.Add(new Triangles() { Positions = TriBottom, Color = Color.FromArgb(128, Color.Green) });

            /*
            Array<float> pos = new float[,] { { 0, 0, 0.0f }, { 0, 5, 0.0f }, { 5, 0, 0.0f }, { 5, 5, 0.0f }};
            Array<float> posBorder = new float[,] { { 0, 0, 0.0f }, { 0, 5, 0.0f }, { 5, 5, 0.0f }, { 5, 0, 0.0f }, { 0, 0, 0.0f } };

            scene.Camera.Add(new LineStrip() { Positions = posBorder, Width = 1, Antialiasing = false, Color = Color.FromArgb(155, Color.Blue), Visible = true });
            scene.Camera.Add(new TrianglesStrip() { Positions = pos, Color = Color.FromArgb(155, Color.Red), Antialiasing = false });
            //scene.Camera.Add(new Triangles() { Positions = pos, Color = Color.FromArgb(255, Color.Green) });

            //scene.Camera.Add(new Triangles() { Positions = new float[,] { { -1, 0, 0 }, { 0, 1, -10 }, { 1, 0, 0 } }, Color = Color.LightGray });
            //scene.Camera.Add(new Lines() { Positions = new float[,] { { -1, 0, 0 }, { 0, 1, -10 } }, Color = Color.Black });
            //scene.Camera.Add(new Lines() { Positions = new float[,] { { 0.1f, 0, 0 }, { 0, 1, -10 } }, Color = Color.Black });            
            
            scene.Camera.Position = new Vector3(2.5, 2.5, 50);
            scene.Camera.LookAt = new Vector3(2.5, 2.5, 0);
            scene.Camera.RotateZ(0.4f);*/

            //scene.Camera.Position = new Vector3(0, 0, 17);
            //scene.Camera.Configure();

            return scene;
        }
    }
}
