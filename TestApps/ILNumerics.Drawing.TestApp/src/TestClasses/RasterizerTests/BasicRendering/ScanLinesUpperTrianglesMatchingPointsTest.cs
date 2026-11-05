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

namespace ILNumerics.DrawingTestApplication.TestClasses.BasicRendering {
    class ScanLinesUpperTrianglesMatchingPointsTest : TestScene {

        public ScanLinesUpperTrianglesMatchingPointsTest() : base(
            TestCategory.RA_BasicRendering,
            "ScanLinesUpperTrianglesMatchingPointsTest",
            @"Scan lines matching up with given start and end vertices? 
There must be a single black pixel (point) marking the EXACT corners of the triangles.
OpenGL usually does work ok here. But check GDI! Do the dots really sit in the triangle corners?
Tolerance accepted: 0px! Keep Top-left rasterizing rule in mind! ")
        { }

        public override Scene GetScene() {
            var scene = new Scene();

            Array<float> A = array(new float[,] { { 0, 0.95f, 0 }, { 0.95f, 0.95f, 1 }, { 0.95f, 0, 0 } }).T;

            scene.Screen.Add(new Group() {
                new Points() {
                    Positions = A,
                    Color = Color.FromArgb(255, Color.Black),
                    Size = 1,
                    Antialiasing = false
                },
                new Triangles() {
                    Positions = A,
                    Color = Color.FromArgb(100, Color.Red),
                    Antialiasing = false
                }
            });
            return scene;
        }

    }   
}
