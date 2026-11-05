using System.Drawing;
using ILNumerics;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;
using static ILNumerics.Globals;
using static ILNumerics.ILMath;

namespace ILNumerics.DrawingTestApplication.TestClasses
{
    class SplinePlotTest : TestScene
    {
        public SplinePlotTest() : base(
            TestCategory.HL_SplinePlotTest,
            "Spline Plot general Test",
            @"[TestCase]:
This creates a simple spline plot similar to the documentation example. Check the general features of the spline plot: vertical and horizontal lines existing and consistent?")
        { }

       
        private readonly Array<float> X = localMember<float>();
        private readonly Array<float> Y = localMember<float>();
        

        public override Scene GetScene()
        {
            var scene = new Scene();

            X.a = linspace<float>(1, 10, 10);
            Y.a = sin(X);
            var pc = scene.Add(new PlotCube() {
                new ILNumerics.Drawing.Plotting.SplinePlot(Y, markerStyle: MarkerStyle.Circle)
            });

            // register mouse event on the plot cube
            pc.MouseClick += (_s, _a) => {
                if (!_a.DirectionUp) return;
                // flip the y data 
                var ebp = pc.First<SplinePlot>(); 
                if (ebp !=null) {
                    Y.a = fliplr(Y); 
                    ebp.Update(Y);
                    ebp.Configure();
                    _a.Refresh = true;
                }
            };
            return scene;
        }
    }
}
