using System.Drawing;
using ILNumerics;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;
using static ILNumerics.Globals;
using static ILNumerics.ILMath;

namespace ILNumerics.DrawingTestApplication.TestClasses.AntialiasingTests.TriangleStrip
{
    class TriangleLinesAAFullWidth : TestScene
    {
        public TriangleLinesAAFullWidth() : base(
            TestCategory.RA_AntialiasedTriangles,
            "Triangle Strip and Lines Anti-aliased",
            @"[TestCase]:
Switch to GDI Render(Triangles anti-aliasing is only in GDI available).
Check if the anti-aliased line and anti-aliased triangle are blended correctly.
No green transparent pixel must be drawn outside the line border.")
        { }

        public override Scene GetScene()
        {
            var scene = new Scene();

            Array<float> pos = array(new float[,] { { 0, 0, 0 }, { 0, 5, 0 }, { 5, 0, 0 }, { 5, 5, 0 } }).T;
            Array<float> posBorder = array(new float[,] { { 0, 0, 0 }, { 0, 5, 0 }, { 5, 5, 0 }, { 5, 0, 0 }, { 0, 0, 0 } }).T;

            scene.Camera.Add(new LineStrip() { Positions = posBorder, Width = 4, Antialiasing = true, Color = Color.Black, Visible = true });
            scene.Camera.Add(new TrianglesStrip() { Positions = pos, Color = Color.FromArgb(255, Color.Green), Antialiasing = true });

            scene.Camera.Position = new Vector3(2.5, 2.5, 50);
            scene.Camera.LookAt = new Vector3(2.5, 2.5, 0);

            scene.Camera.Configure();

            return scene;
        }
    }
}

