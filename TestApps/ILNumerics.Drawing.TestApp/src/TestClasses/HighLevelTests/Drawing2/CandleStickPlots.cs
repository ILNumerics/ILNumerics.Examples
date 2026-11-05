using System.Drawing;
using ILNumerics;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;
using static ILNumerics.Globals;
using static ILNumerics.ILMath;

namespace ILNumerics.DrawingTestApplication.TestClasses
{
    class CandleStickPlots : TestScene
    {
        public CandleStickPlots() : base(
            TestCategory.HL_CandleStickPlots,
            "Candle Stick Plots general Test",
            @"[TestCase]:
This creates a simple candle stick plot similar to the documentation example. Check the general features of the candle sticks: vertical and horizontal lines existing and consistent? All candle boxes look complete? Are they on the vertical lines? (no wholes)

Now click on the plot cube! The plot flips horizontally? All bars, however, must stay the same. I.e. the filled bar now is at first and second-last position.")
        { }

        private readonly Array<float> XY = localMember<float>();

        public override Scene GetScene()
        {
            var scene = new Scene();

            XY.a = zeros<float>(5, 30);
            XY["0;:"] = linspace<float>(-1, 2, 30);
            XY["1;:"] = exp(XY["0;:"]);
            XY["2;:"] = XY["1;:"] + 2; 
            XY["3;:"] = XY["1;:"] + (XY["2;:"]- XY["1;:"]) / 4; 
            XY["4;:"] = XY["1;:"] + (XY["2;:"]- XY["1;:"]) / 3;

            // exchange 2nd and last O/C values, bull -> bear(?)
            Array<float> tmp = XY["3;1,end"];
            XY["3;1,end"] = XY["4;1,end"];
            XY["4;1,end"] = tmp;

            var pc = scene.Add(new PlotCube() {
                new ILNumerics.Drawing.Plotting.Candlestick(XY, bodyWidth: 0.05f, bullFrameWidth : 1, bullFillColor: Color.AliceBlue)
            });

            // register mouse event on the plot cube
            pc.MouseClick += (_s, _a) => {
                if (!_a.DirectionUp) return;
                // flip the y data 
                var ebp = pc.First<Candlestick>(); 
                if (ebp !=null) {
                    XY["1:4;:"] = fliplr(XY["1:4;:"]); 
                    ebp.Update(Low: XY["1;:"], High: XY["2;:"], Open: XY["3;:"], Close: XY["4;:"]);
                    ebp.Configure();
                    _a.Refresh = true;
                }
            };
            return scene;
        }
    }
}
