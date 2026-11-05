using System.Drawing;
using ILNumerics;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;
using static ILNumerics.Globals;
using static ILNumerics.ILMath;

namespace ILNumerics.DrawingTestApplication.TestClasses.AntialiasingTests
{
    class TriangleTest : TestScene
    {
        public TriangleTest() : base(
            TestCategory.RA_BasicRendering,
            "Alpha Blending Triangles Test",
            @"Two rectangles are rendered. 
Camera is at the default position(0,0,10), first rect is at position 0,0,0. Second rect - 0,0,-10
The second rect must be fully covered by first one.")
        { }

        public override Scene GetScene()
        {
            var scene = new Scene();

            Array<float> pos1 = array(new float[,] {                
                { -1, -1, 0},
                { -1,  1, 0},
                {  1,  1, 0},
                {  1, -1, 0}
            }).T;

            Array<float> pos2 = pos1 * 2;
            pos2[2, ":"] = -10;

            Array<int> triIndices = array(new int[] {
                0, 1, 2,
                2, 3, 0,
            });

            var mygroup = scene.Camera.Add(new Group()
            {                
                new Triangles() { Positions = pos1, Indices = triIndices, Color = Color.FromArgb(100, Color.Blue), Visible = true, Antialiasing = false},
                new Triangles() { Positions = pos2, Indices = triIndices, Color = Color.FromArgb(255, Color.Red), Visible = true, Antialiasing = false},
            });
            scene.Camera.ZoomFactor = 0.7f;


            return scene;
        }

    }
}
