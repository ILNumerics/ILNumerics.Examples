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
    class ScanLinesLowerRotatedTriangles1MatchingPointsTest : TestScene
    {
        public ScanLinesLowerRotatedTriangles1MatchingPointsTest() : base(
            TestCategory.RA_BasicRendering,
            "ScanLinesLowerRotatedTriangles1MatchingPointsTest",
            @"Scan lines matching up with given start and end vertices? 
There must be a single black pixel (point) marking the exact resetcorners of the triangles.
Tolerance accepted: 0px! Keep Top-Left rasterizing rule in mind! 
Also: The descending of the upper edge must start (y index: 0) with a horizontal _row_ of more than one pixel!")
        { }

        public override Scene GetScene()
        {
            var scene = new Scene();

            Array<float> A = array(new float[,] { { 0.01f, 0.01f, 0 }, { 0.95f, 0.95f, 0 }, { 0.95f, 0.01f, 0 } }).T;

            scene.Screen.Add(new Group(rotateAxis: Vector3.UnitZ, angle: -0.005) {
                new Triangles() {
                    Positions = A,
                    Color = Color.FromArgb(100, Color.Red),
                    Antialiasing = false
                },
                new Points() {
                    Positions = A,
                    Color = Color.FromArgb(255, Color.Black),
                    Size = 1,
                    Antialiasing = false
                }
            });
            return scene;
        }

    }
}
