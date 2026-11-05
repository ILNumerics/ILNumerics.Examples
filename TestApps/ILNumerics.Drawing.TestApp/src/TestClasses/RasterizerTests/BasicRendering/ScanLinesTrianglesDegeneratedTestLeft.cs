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
    class ScanLinesTrianglesDegeneratedTestLeft : TestScene
    {

        public ScanLinesTrianglesDegeneratedTestLeft() : base(
            TestCategory.RA_BasicRendering,
            "ScanLinesTrianglesDegeneratedTestLeft",
             @"Degenerated triangles must include the start vertex.
A very sharp corner for the upper left vertex. Must inclue the vertex itself. 
No missing pixels allowed! Check the left vertex.")
        { }

        public override Scene GetScene()
        {
            var scene = new Scene();

            Array<float> A = array(new float[,] { { 0, 0.4f, 0 }, { 1, 0.42f, 0 }, { 1, 0.4f, 0 } }).T;

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
                },
            });
            return scene;
        }

    }
}
