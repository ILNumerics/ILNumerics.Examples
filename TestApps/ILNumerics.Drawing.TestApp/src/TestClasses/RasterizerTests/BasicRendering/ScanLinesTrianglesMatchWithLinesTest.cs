using ILNumerics;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ILNumerics.Globals;
using static ILNumerics.ILMath;

namespace ILNumerics.DrawingTestApplication.TestClasses.BasicRendering
{
    class ScanLinesTrianglesMatchWithLinesTest : TestScene
    {
        public ScanLinesTrianglesMatchWithLinesTest() : base(
            TestCategory.RA_BasicRendering,
            "ScanLinesTrianglesMatchWithLinesTest",
            @"Lines as borders must exactly run on the edge of a triangle with the same vertices.
This checks NON-HORIZONTAL lines only. 
All lines must lay exactly on the border / edge of the triangles.
Pixels from top and left edges are drawn twice, by the line and by the triangle. So they appear darker.
Pixels from right edges and from strictly lower bottom edges are drawn only once, by the line.
No missing pixels along the lines!
No gaps between line and triangle!
No triangle pixels outside of the line borders!")
        { }

        public override Scene GetScene()
        {
            var scene = new Scene();

            Array<float> A = array(new float[,] {
                { -0.5f, -0.5f, 0 }, { -0.5f, 0.5f, 0 },
                { 0.5f, 0.5f, 0 }, { 0.5f, -0.5f, 0 } }).T;

            scene.Camera.Add(new Group(rotateAxis: Vector3.UnitZ, angle: 0.1) {
                new LineStrip() {
                    Positions = A,
                    Color = Color.FromArgb(30, Color.Red),
                    Indices = new[] {0,1,2,3,0},
                    Width = 1,
                    Antialiasing = false
                },
                new Triangles() {
                    Positions = A,
                    Color = Color.FromArgb(100, Color.Blue), // new float[,] { { 1, 0, 0 }, { 0, 1, 0 }, { 0, 0, 1 }, { 1, 1, 0 } },
                    Indices = new[] {0,1,2,0,2,3},
                    Antialiasing = false
                },
            });
            return scene;
        }

    }
}
