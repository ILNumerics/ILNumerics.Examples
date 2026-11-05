using System.Drawing;
using ILNumerics;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;
using static ILNumerics.Globals;
using static ILNumerics.ILMath;

namespace ILNumerics.DrawingTestApplication.TestClasses.AntialiasingTests
{
    class TriangleRenderDirectionTest : TestScene
    {
        public TriangleRenderDirectionTest() : base(
            TestCategory.RA_BasicRendering,
            "TriangleRenderDirectionTest",
            @"Triangles must be drawn - regardless of the vertex definition order. 
Left triangle: CW. Right triangle: CCW.")

        { }

        public override Scene GetScene()
        {
            var scene = new Scene();

            Array<float> pos1 = array(new float[,] {
                { -1, -1, 0},
                { -1,  1, 0},
                {  0,  0, 0},
                { -1, -1, -.1f},
                { -1,  1, -.1f},
                {  0,  0, -.1f},
            }).T;

            Array<float> col = array(new float[,] {
                { 0, 1, 0},
                { 0, 1, 0},
                { 0, 1, 0},
                { 0, 0, 1},
                { 0, 0, 1},
                { 0, 0, 1},
            }).T;

            Array<int> triIndices = vector( 0, 1, 2, 3, 4, 5);
            Array<int> lineIndices = vector( 0, 1, 1, 2, 2, 0, 3, 4, 4, 5, 5, 3);

            var mygroup = scene.Camera.Add(new Group()
            {                
                new Triangles() { Positions = pos1, Indices = triIndices, Color = null, Colors = col, AutoNormals = false },
                // new Drawing.Lines() { Positions = pos1, Indices = lineIndices, Color = Color.Black, Width = 2 }
            });
            scene.First<ILNumerics.Drawing.Label>().Visible = false;
            scene.Configure(); 
            return scene;
        }

    }
}
