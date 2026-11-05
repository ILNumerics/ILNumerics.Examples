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
    class ScanLinesTrianglesMatchAdjacentEdgesTest : TestScene
    {

        public ScanLinesTrianglesMatchAdjacentEdgesTest() : base(
            TestCategory.RA_BasicRendering,
            "ScanLinesTrianglesMatchAdjacentEdgesTest",
            @"Do triangles share edges with adjacent triangles exactly?
An edge which is shared between two neighboring triangles must not be visible.
The rectangle is created by placing two triangles exactly next to each other. 
There must be no diagonal visible. Not by missing / doubled pixels. 
Not by any color gradient disturbance.
Rotate, move, pan the rectangle with the mouse: around Z, around arbitrary axes. 
Any position, where a diagonal is visible? (Wrong!)")
        { }

        public override Scene GetScene()
        {
            var scene = new Scene();

            Array<float> A = array(new float[,] {
                { -0.5f, -0.5f, 0 }, { -0.5f, 0.5f, 0 },
                { 0.5f, 0.5f, 0 },{ 0.5f, -0.5f, 0 } }).T;

            scene.Camera.Add(new Group() {
                new Points() {
                    Positions = A,
                    Color = Color.FromArgb(255, Color.Black),
                    Size = 1,
                    Antialiasing = false
                },
                new Triangles() {
                    Positions = A,
                    Color = Color.FromArgb(150, Color.Red),
                    Indices = new[] {0,1,2,0,2,3},
                    Antialiasing = false
                },
            });
            return scene;
        }

    }
}
