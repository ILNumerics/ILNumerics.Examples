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
    class ScanLinesTrianglesMatchingLinesTest : TestScene
    {
        public ScanLinesTrianglesMatchingLinesTest() : base(
            TestCategory.RA_BasicRendering,
            "ScanLinesTrianglesMatchingLinesTest",
            @"Scan line generated triangles matching up with given start and end vertices; a line rendered along an edge? 
There must be a single gray (transparent) pixel ('point') marking the EXACT corners of the triangles.
A 1px transparent line runs along the edges of the triangle. Another line segment connects an outside point as color reference. 
All shapes must lay exactly above each other. 
Check the Top-Left rule: right and bottom edges are not drawn by triangles! There must be a line only. All other edges must be drawn for both, triangle and line.
OpenGL usually does work ok here. But check GDI! 
Do the dots really sit in the triangle corners? 
Do the edges match up for triangles / lines? 
Are right/bottom edges drawn only once? 
Tolerance accepted: 1px for points
Tolerance for edges: 0px!")
        { }

        public override Scene GetScene()
        {
            var scene = new Scene();

            Array<float> A = array(new float[,] { { 0, 0, 0 }, { 1, 1, 1 }, { 1, 0, 0 }, { 0f, 1, 1 } }).T;

            scene.Camera.Add(new Group() {
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
                },
                new LineStrip() {
                    Positions = A,
                    Indices = new[] { 0, 1, 2, 0, 3 },
                    Color = Color.FromArgb(100, Color.Green),
                    Antialiasing = false
                }
            });
            return scene;
        }

    }
}
