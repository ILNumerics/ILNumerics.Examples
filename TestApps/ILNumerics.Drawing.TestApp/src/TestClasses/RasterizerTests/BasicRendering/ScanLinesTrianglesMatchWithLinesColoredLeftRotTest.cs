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
    class ScanLinesTrianglesMatchWithLinesColoredLeftRotTest : TestScene
    {
        public ScanLinesTrianglesMatchWithLinesColoredLeftRotTest() : base(
            TestCategory.RA_BasicRendering,
            "ScanLinesTrianglesMatchWithLinesColoredLeftRotTest",
            @"Lines as borders must exactly run on the edge of a triangle with the same vertices.
This checks NON-HORIZONTAL lines only. 
The lines run exactly on the triangle edges. 
Due to the way scanlines work and the defintion of the Top-Left rule no PARTLY sharing of pixels here!.
The color gradient must run smoothly along the length of the line.
Color gradient must match up with the triangle color.")
        { }

        public override Scene GetScene()
        {
            var scene = new Scene();

            Array<float> A = array(new float[,] {
                { -0.5f, -0.5f, 0 }, { -0.5f, 0.5f, 0 },
                { 0.5f, 0.5f, 0 }, { 0.5f, -0.5f, 0 } }).T;
            Array<float> C = array(new float[,] { { 1, 0, 0, 0.3f }, { 0, 1, 0, 0.3f }, { 0, 0, 1, 0.3f }, { 1, 1, 0, 0.3f } }).T;
            Array<float> C2 = C.C;
            C2["end;:"] = 0.9f;

            scene.Camera.Add(new Group(rotateAxis: Vector3.UnitZ, angle: -0.1) {
                new LineStrip() {
                    Positions = A,
                    Color = null,
                    Colors = C,
                    Indices = new[] {0,1,2,3,0},
                    Width = 1,
                    Antialiasing = false
                },
                new Triangles() {
                    Positions = A,
                    Color = null,
                    Colors = C2,
                    Indices = new[] {0,1,2,0,2,3},
                    Antialiasing = false,
                },
            });
            return scene;
        }

    }
}
