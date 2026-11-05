using System.Drawing;
using ILNumerics;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;
using static ILNumerics.Globals;
using static ILNumerics.ILMath; 

namespace ILNumerics.DrawingTestApplication.TestClasses.AntialiasingTests.Complex
{
    class VisualizationBasics : TestScene
    {
        public VisualizationBasics() : base(
            TestCategory.HL_BarPlots,
            "VisualizationBasics Test",
            @"[TestCase]:
Line plots multiple Y axes 
Multiple data sets are depicted in one plot with multiple Y-axes.")
        { }

        public override Scene GetScene()
        {
            // generate data 
            Array<float> X1 = linspace<float>(0, 1, 50);
            Array<float> Y1 = sin(X1 * 10) * 10f;
            Array<float> Y2 = cos(X1) * 5.5f;
            // create scene
            var scene = new Scene();
            // create first plot cube (upper left)
            var plotCube1 = scene.Add(new PlotCube() {
                // scattered plot
                Children = {new Points() {
    Positions = tosingle(rand(3, 100)),
    Size = 10, Color = Color.FromArgb(100, Color.Olive) }},
                ScreenRect = new RectangleF(0, 0, 0.8f, 0.8f),
                Axes = { 
	// reposition y axis: right
	YAxis = {
      Position = new Vector3(1,0,0),
      Label = {Visible = false}
    },
    XAxis = {Label = {Visible = false}}
  }
            });
            // create second plot cube (bottom left)
            var plotCube2 = scene.Add(new PlotCube() {
                Children = {new LinePlot(Y1, lineColor: Color.FromArgb(150, Color.DarkOliveGreen),
                             lineWidth:2, lineStyle: DashStyle.Dotted)},
                ScreenRect = new RectangleF(0, 0.6f, 0.8f, 0.4f),
                Axes = { 
	// reposition y axis: right
	YAxis = {
      Position = new Vector3(1,0,0),
      Label = {Visible = false}},
    XAxis = {
      Ticks = {Visible = false},
      Label = {Visible = false}}
  }
            });
            // create third plot cube (upper right)
            var plotCube3 = scene.Add(new PlotCube() {
                Children = { new LinePlot(X1, Y2, lineColor: Color.FromArgb(150, Color.DarkOliveGreen), lineWidth: 2) },
                ScreenRect = new RectangleF(0.625f, 0, 0.4f, 0.8f),
                Axes = {
    YAxis = {
        Label = {Visible = false}},
    XAxis = {
      Ticks = {Visible = false},
      Label = {Visible = false}}
  },

            });
            // rotate plot cube 270°
            plotCube3.RotateZ(3 * pi / 2);
            
            return scene;

        }
    }
}
