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
    class ScanLinesTrianglesMatchWithLinesVerticalTest : TestScene
    {
        public ScanLinesTrianglesMatchWithLinesVerticalTest() : base(
            TestCategory.RA_BasicRendering,
            "ScanLinesTrianglesMatchWithLinesVerticalTest",
            @"Lines as borders must exactly run on the edge of a triangle with the same vertices.
This checks VERTICAL lines only. 
The upper and lower start of both, left side border and right side border must exactly match up with the triangle fill.
The left line must be darker than the lower line, because triangle pixel are only drawn behind the left line (top-left rule).
There must be no triangle pixels below the right line.
There must always be a triangle pixel benaeth each pixel from the left line.")
        { }

        public override Scene GetScene()
        {
            var scene = new Scene();

            Array<float> A = array(new float[,] {
                { -0.5f, -0.5f, 0 }, { -0.5f, 0.5f, 0 },
                { 0.5f, 0.5f, 0 }, { 0.5f, -0.5f, 0 } }).T;

            scene.Camera.Add(new Group() {
                new Lines() {
                    Positions = A,
                    Color = Color.FromArgb(100, Color.Red),
                    Indices = new[] {0,1,2,3},
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
