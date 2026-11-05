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
    class ScanLinesTrianglesMatchingColorsTest : TestScene
    {

        public ScanLinesTrianglesMatchingColorsTest() : base(
            TestCategory.RA_BasicRendering,
            "ScanLinesTrianglesMatchingColorsTest",
            @"Exact color interpolation along the edges of a single triangle.  
Triangles are rendered as upper- and lower parts with a horizontal deviation between them. 
Color gradients must run smoothly along any edge without steps, interruption or other artefacts.
Top vertex: Green. Center: Blue. Right: Red")
        { }

        public override Scene GetScene()
        {
            var scene = new Scene();

            Array<float> A = array(new float[,] { { 0, 0, 0 }, { 1, 1, 0 }, { 1, 0, 0 } }).T;

            scene.Camera.Add(new Group() {
                new Triangles() {
                    Positions = A,
                    Color = null,
                    Colors = array(new float[,] { { 0,0,1 }, { 0,1,0 }, { 1,0,0 }, }).T,
                    Antialiasing = false
                },
            });
            return scene;
        }

    }
}
