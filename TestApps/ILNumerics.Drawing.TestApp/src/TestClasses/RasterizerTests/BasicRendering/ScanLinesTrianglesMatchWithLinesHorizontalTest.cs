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
    class ScanLinesTrianglesMatchWithLinesHorizontalTest : TestScene
    {
        public ScanLinesTrianglesMatchWithLinesHorizontalTest() : base(
            TestCategory.RA_BasicRendering,
            "ScanLinesTrianglesMatchWithLinesHorizontalTest",
            @"Lines as borders must exactly run on the edge of a triangle with the same vertices.
This checks HORIZONTAL lines only. 
The left start and right start of both, upper side border and lower side border must exactly match up with the triangle fill.
The upper line must be darker than the lower line, because triangle pixel are only drawn behind the upper line.
There must be no triangle pixels below the lower line.
There must always be a triangle pixel below the upper line.")
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
                    Indices = new[] {1,2,0,3},
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
