using System.Drawing;
using ILNumerics;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;
using static ILNumerics.Globals;
using static ILNumerics.ILMath;

namespace ILNumerics.DrawingTestApplication.TestClasses.AntialiasingTests
{
    class FillAreaDrawingTest : TestScene
    {
        public FillAreaDrawingTest() : base(
            TestCategory.HL_LinePlotting, 
            "FillAreaDrawingTest", 
            @"[TestCase]:
The fill area under the lineplot must be colored with default color(red), and the line plot must be colored also by default(blue).
") { }

        public override Scene GetScene()
        {            
            var scene = new Scene();


            Array<float> xy = tosingle(linspace(0, 0.5f, 100));
            xy["1;:"] = tosingle(rand(1, 100));

            scene.Add(new PlotCube(twoDMode: false)
            {
                new FillArea(xy),
            });


            return scene;
        }

    }
}
