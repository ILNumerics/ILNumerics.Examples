using System.Drawing;
using ILNumerics;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;
using static ILNumerics.Globals;
using static ILNumerics.ILMath;

namespace ILNumerics.DrawingTestApplication.TestClasses
{
    class ErrorBarPlots : TestScene
    {
        public ErrorBarPlots() : base(
            TestCategory.HL_ErrorBarPlots,
            "Error Bar Plots general Test",
            @"[TestCase]:
This creates a simple error bar similar to the documentation example. Check the general features of the error bars: vertical and horizontal lines existing and consistent?

Now click on the plot cube! The plot flips horizontally? All bars, however, must stay the same. I.e. the bar with length 0 must still be at position /data value #5 from the right.")
        { }

        private readonly Array<float> XY = localMember<float>();

        public override Scene GetScene()
        {
            var scene = new Scene();

            XY.a = zeros<float>(2, 30);
            XY["0;:"] = linspace<float>(-1, 2, 30);
            XY["1;:"] = exp(XY["0;:"]);

            Array<float> L = abs(cos(XY[0, ":"])); 

            var pc = scene.Add(new PlotCube() {
                new ILNumerics.Drawing.Plotting.ErrorBarPlot(XY, L, errorbarWidth: 0.025f)
            });

            // register mouse event on the plot cube
            pc.MouseClick += (_s, _a) => {
                if (!_a.DirectionUp) return;
                // flip the y data 
                var ebp = pc.First<ErrorBarPlot>(); 
                if (ebp !=null) {
                    XY["1;:"] = fliplr(XY["1;:"]); 
                    ebp.Update(XY["0;:"], XY["1;:"]);
                    ebp.Configure();
                    _a.Refresh = true;
                }
            };
            return scene;
        }
    }
}
