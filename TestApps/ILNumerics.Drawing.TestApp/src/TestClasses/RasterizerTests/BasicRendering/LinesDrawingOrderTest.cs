using System.Drawing;
using ILNumerics;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;
using static ILNumerics.Globals;
using static ILNumerics.ILMath;

namespace ILNumerics.DrawingTestApplication.TestClasses.AntialiasingTests
{
    class LinesDrawingOrderTest : TestScene
    {
        public LinesDrawingOrderTest() : base(
            TestCategory.CT_CustomerTests,
            "LinesDrawingOrderTest",
            @"Issue from Siemens.
Lines Rendering Order must be the same by using the both renders.
Blue Line must be on top, Red Line - on bottom."
        ) { }

        
        public override Scene GetScene()
        {
            var scene = new Scene();

            var mygroup = scene.Camera.Add(new Group()
            {
                new LinePlot(array(new float[,] { { -1.0f, -1.0f }, { 1.0f, 1.0f } }).T, lineColor: Color.Red, lineWidth: 20),
                new LinePlot(array(new float[,] { { -1.0f, 1.0f }, { 1.0f, -1.0f } }).T, lineColor: Color.Blue, lineWidth: 20),
            });
            

            return scene;
        }

    }
}
