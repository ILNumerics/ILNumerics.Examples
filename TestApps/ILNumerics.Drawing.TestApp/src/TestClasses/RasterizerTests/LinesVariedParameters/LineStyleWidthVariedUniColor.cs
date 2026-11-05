using System.Drawing;
using ILNumerics;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;
using static ILNumerics.Globals;
using static ILNumerics.ILMath;

namespace ILNumerics.DrawingTestApplication.TestClasses
{
    class LineStyleWidthVariedUniColor : TestScene
    {
        public LineStyleWidthVariedUniColor() : base(
            TestCategory.RA_LinesVariedParameters,
            "Line Plot: varied style and width, single-colored",
            @"[TestCase]:
This creates a number of line plots that are all single-colored but vary in terms of line style and width. Click on the plot cube to switch antialiasing on or off.
Please check if the line styles are consistent for all widths. Focus on what happens at sharp edges and if the orientation of the line changes. ")
        { }

        

        public override Scene GetScene()
        {
            var scene = new Scene();
            var plotCube = scene.Add(new PlotCube());

            Array<float> X = linspace<float>(-pif, 0, 6);
            Array<float> Y = sin(X);
            Array<float> YPart1 = new float[] { 0, 0, -0.5f, 0 };
            Array<float> YPart2 = new float[] { 0, -0.5f, 0, 0 };            
            Array<float> XFinal = new float[] { 0.5f, 1.5f, 1.5f, 2, 3, 3.5f, 4, 4.5f, 5, 5.5f, 6.5f, 7, 7, 8 };
            Array<float> YFinal = horzcat(YPart1.T, Y, YPart2.T);            

            for (int i = 0; i < 6; i++) {
                plotCube.Add(new LinePlot(X: XFinal, Y: YFinal + i * 2, lineColor: Color.Black, lineWidth: i * 2 + 1, lineStyle: DashStyle.Solid, tag: "linePlot1"));
                plotCube.Add(new LinePlot(X: XFinal + 8, Y: YFinal + i * 2, lineColor: Color.Black, lineWidth: i * 2 + 1, lineStyle: DashStyle.Dashed, tag: "linePlot2"));
                plotCube.Add(new LinePlot(X: XFinal + 16, Y: YFinal + i * 2, lineColor: Color.Black, lineWidth: i * 2 + 1, lineStyle: DashStyle.Dotted, tag: "linePlot3"));
                plotCube.Add(new LinePlot(X: XFinal + 24, Y: YFinal + i * 2, lineColor: Color.Black, lineWidth: i * 2 + 1, lineStyle: DashStyle.PointDash, tag: "linePlot4"));

                plotCube.Add(new Drawing.Label("Linewidth: " + (i * 2 + 1).ToString()) { Position = new Vector3(2.0f, 0.2f + i * 2, 0) });                               
            }

            


            var label = plotCube.Axes.YAxis.Add(new Drawing.Label());
            label.Position = new Vector3(0.15f, 1.025f, 0);

            label.Font = new Font(FontFamily.GenericSansSerif, 20);
            label.Color = Color.OrangeRed;
            label.Text = "Antialiasing off";

           
            var linePlots = plotCube.Find<LinePlot>();
            plotCube.MouseClick += (_s, _a) => {
                if (!_a.DirectionUp) return;         

                if (linePlots != null) {
                    foreach (LinePlot element in linePlots) {
                        element.Line.Antialiasing = !element.Line.Antialiasing;
                        element.Configure();
                        if (element.Line.Antialiasing)
                        {
                            label.Text = "Antialiasing on";
                        }
                        else {
                            label.Text = "Antialiasing off";
                        }
                    }

                }
                _a.Refresh = true;
              
            };

    
            return scene;
        }
    }
}
