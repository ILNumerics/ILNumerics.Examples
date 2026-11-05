using System.Drawing;
using ILNumerics;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;
using static ILNumerics.Globals;
using static ILNumerics.ILMath;

namespace ILNumerics.DrawingTestApplication.TestClasses.AntialiasingTests.TriangleStrip
{
    class TriangleFanAA : TestScene
    {
        public TriangleFanAA() : base(
            TestCategory.RA_AntialiasedTriangles,
            "TriangleFan Anti-aliasing",
            @"[TestCase]:
Switch to GDI Render(Triangles anti-aliasing is only in GDI available)
Check if the border line lie on the edge of triangle fan. Triangle fan must be drawn as a solid rectangle without holes between triangles.")
        { }        

        public override Scene GetScene() 
        {
            var scene = new Scene();            

            Array<float> pos = array(new float[,] { { 0, 0, 0 }, { -1, 0, 0 }, { -1, 1, 0 }, { 0, 1, 0 }, { 1, 1, 0 }, { 1, 0, 0 }, { 1, -1, 0 }, { 0, -1, 0 }, { -1, -1, 0 }, { -1, 0, 0 } }).T;

            scene.Camera.Add(new TrianglesFan(){ Positions = pos, Antialiasing = true, Color = Color.Red });            
            scene.Camera.Add(
                new LinePlot(positions: pos, lineColor: Color.Black, markerStyle: MarkerStyle.Dot) { Line = { Antialiasing = false} });

            scene.Camera.Position = new Vector3(0, 0, 50);

            foreach (var label in scene.Find<ILNumerics.Drawing.Label>()) label.Visible = false; 

            scene.Camera.Configure();

            return scene;
        }
    }
}
