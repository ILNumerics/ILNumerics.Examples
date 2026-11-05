using System.Drawing;
using ILNumerics;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;
using static ILNumerics.Globals;
using static ILNumerics.ILMath;

namespace ILNumerics.DrawingTestApplication.TestClasses
{ 
    class BoxPlotTest : TestScene
    { 
        public BoxPlotTest() : base(
            TestCategory.HL_BoxPlotTest,
            "Box Plot general Test",
            @"[TestCase]:
This creates a simple box plot similar to the documentation example. Check the general features of the box plot: vertical and horizontal lines existing and consistent?

Now click on the plot cube! The plot flips horizontally? All bars, however, must stay the same. I.e. the bar with length 0 must still be at position /data value #5 from the right.")
        { }

        private readonly Array<float> XY = localMember<float>();
        private readonly Array<float> X = localMember<float>();
        private readonly Array<float> Y = localMember<float>();
        

        public override Scene GetScene()
        {
            var scene = new Scene();

            X.a = linspace<float>(1, 2, 5);
            Y.a = array(new float[,] { { 1, 1, 1, 1, 2, 2, 3, 4, 3, 2, 1, 2, 2, 5, 4, 4, 5, 12, 10 },
                { 3, 2, 3, 2, 2, 2, 3, 4, 5, 5, 6, 6, 6, 6, 0, 4, 5, 4, 5 }, { 5, 6, 7, 5, 6, 6, 5, 5, 5, 6, 7, 8, 8, 8, 9, 9, 15, 16, 7 },
                { 2, 3, 3, 4, 2, 5, 5, 6, 7, 8, 8, 9, 10, 10, 8, 9, 2, 3, 4 }, { 7, 6, 6, 7, 7, 8, 8, 1, 8, 9, 9, 7, 10, 10, 13, 13, 12, 12, 14 } }).T;          

            var pc = scene.Add(new PlotCube() {
                new ILNumerics.Drawing.Plotting.BoxPlot(X,Y, boxWidth: 0.1)
            });

            // register mouse event on the plot cube
            pc.MouseClick += (_s, _a) => {
                if (!_a.DirectionUp) return;
                // flip the y data 
                var ebp = pc.First<BoxPlot>(); 
                if (ebp !=null) {
                    Y.a = fliplr(Y); 
                    ebp.Update(X, Y);
                    ebp.Configure();
                    _a.Refresh = true;
                }
            };
            return scene;
        }
    }
}
