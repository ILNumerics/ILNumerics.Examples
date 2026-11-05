using System.Drawing;
using ILNumerics;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;
using static ILNumerics.Globals;
using static ILNumerics.ILMath;

namespace ILNumerics.DrawingTestApplication.TestClasses
{
    class LineStyleWidthAlphaVariedUniColor : TestScene
    {
        public LineStyleWidthAlphaVariedUniColor() : base(
            TestCategory.RA_LinesVariedParameters,
            "Line Plot: varied style, width and alpha, single-colored",
            @"[TestCase]:
This creates a number of line plots that are all single-colored but vary in terms of line style, width and transparency. Click on the plot cube to switch antialiasing on or off.
Please check if the line styles are consistent for all widths. Focus on what happens at sharp edges and if the orientation of the line changes. Check if the transperancy of
the line has an influence.")
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

            for (int i = 0; i < 6; i++)
            {
                plotCube.Add(new LinePlot(X: XFinal, Y: YFinal + i * 2, lineColor: Color.Black, lineWidth: i * 2 + 1, lineStyle: DashStyle.Solid) { Alpha = 0.6f });
                plotCube.Add(new LinePlot(X: XFinal + 8, Y: YFinal + i * 2, lineColor: Color.Black, lineWidth: i * 2 + 1, lineStyle: DashStyle.Dashed) { Alpha = 0.6f });
                plotCube.Add(new LinePlot(X: XFinal + 16, Y: YFinal + i * 2, lineColor: Color.Black, lineWidth: i * 2 + 1, lineStyle: DashStyle.Dotted) { Alpha = 0.6f });
                plotCube.Add(new LinePlot(X: XFinal + 24, Y: YFinal + i * 2, lineColor: Color.Black, lineWidth: i * 2 + 1, lineStyle: DashStyle.PointDash) { Alpha = 0.6f });
                
                plotCube.Add(new LinePlot(X: XFinal, Y: YFinal + i * 2 + 12.5f, lineColor: Color.Black, lineWidth: i * 2 + 1, lineStyle: DashStyle.Solid) { Alpha = 0.2f });
                plotCube.Add(new LinePlot(X: XFinal + 8, Y: YFinal + i * 2 + 12.5f, lineColor: Color.Black, lineWidth: i * 2 + 1, lineStyle: DashStyle.Dashed) { Alpha = 0.2f });
                plotCube.Add(new LinePlot(X: XFinal + 16, Y: YFinal + i * 2 + 12.5f, lineColor: Color.Black, lineWidth: i * 2 + 1, lineStyle: DashStyle.Dotted) { Alpha = 0.2f });
                plotCube.Add(new LinePlot(X: XFinal + 24, Y: YFinal + i * 2 + 12.5f, lineColor: Color.Black, lineWidth: i * 2 + 1, lineStyle: DashStyle.PointDash) { Alpha = 0.2f });

                plotCube.Add(new LinePlot(X: XFinal + 35, Y: YFinal + i * 2, lineColor: Color.Black, lineWidth: i * 2 + 1, lineStyle: DashStyle.Solid) { Alpha = 0.8f });
                plotCube.Add(new LinePlot(X: XFinal + 8 + 35, Y: YFinal + i * 2, lineColor: Color.Black, lineWidth: i * 2 + 1, lineStyle: DashStyle.Dashed) { Alpha = 0.8f });
                plotCube.Add(new LinePlot(X: XFinal + 16 + 35, Y: YFinal + i * 2, lineColor: Color.Black, lineWidth: i * 2 + 1, lineStyle: DashStyle.Dotted) { Alpha = 0.8f });
                plotCube.Add(new LinePlot(X: XFinal + 24 + 35, Y: YFinal + i * 2, lineColor: Color.Black, lineWidth: i * 2 + 1, lineStyle: DashStyle.PointDash) { Alpha = 0.8f });

                plotCube.Add(new LinePlot(X: XFinal + 35, Y: YFinal + i * 2 + 12.5f, lineColor: Color.Black, lineWidth: i * 2 + 1, lineStyle: DashStyle.Solid) { Alpha = 0.4f });
                plotCube.Add(new LinePlot(X: XFinal + 8 + 35, Y: YFinal + i * 2 + 12.5f, lineColor: Color.Black, lineWidth: i * 2 + 1, lineStyle: DashStyle.Dashed) { Alpha = 0.4f });
                plotCube.Add(new LinePlot(X: XFinal + 16 + 35, Y: YFinal + i * 2 + 12.5f, lineColor: Color.Black, lineWidth: i * 2 + 1, lineStyle: DashStyle.Dotted) { Alpha = 0.4f });
                plotCube.Add(new LinePlot(X: XFinal + 24 + 35, Y: YFinal + i * 2 + 12.5f, lineColor: Color.Black, lineWidth: i * 2 + 1, lineStyle: DashStyle.PointDash) { Alpha = 0.4f });

                plotCube.Add(new Drawing.Label("Linewidth: " + (i * 2 + 1).ToString()) { Position = new Vector3(3.0f, 0.4f + i * 2, 0), Font = new Font(FontFamily.GenericSansSerif, 8) });
                plotCube.Add(new Drawing.Label("Linewidth: " + (i * 2 + 1).ToString()) { Position = new Vector3(3.0f, 12.9f + i * 2, 0), Font = new Font(FontFamily.GenericSansSerif, 8) });

                plotCube.Add(new Drawing.Label("Linewidth: " + (i * 2 + 1).ToString()) { Position = new Vector3(38.0f, 0.4f + i * 2, 0), Font = new Font(FontFamily.GenericSansSerif, 8) });
                plotCube.Add(new Drawing.Label("Linewidth: " + (i * 2 + 1).ToString()) { Position = new Vector3(38.0f, 12.9f + i * 2, 0), Font = new Font(FontFamily.GenericSansSerif, 8) });

            }

            plotCube.Add(new Drawing.Label("Alpha: 0.2") { Position = new Vector3(15.0f, 23.0f, 0), Font = new Font(FontFamily.GenericSansSerif, 14) });
            plotCube.Add(new Drawing.Label("Alpha: 0.4") { Position = new Vector3(50.0f, 23.0f, 0), Font = new Font(FontFamily.GenericSansSerif, 14) });
            plotCube.Add(new Drawing.Label("Alpha: 0.6") { Position = new Vector3(15.0f, 10.5f, 0), Font = new Font(FontFamily.GenericSansSerif, 14) });
            plotCube.Add(new Drawing.Label("Alpha: 0.8") { Position = new Vector3(50.0f, 10.5f, 0), Font = new Font(FontFamily.GenericSansSerif, 14) });


            plotCube.Limits.Set(new Vector3(-5, -2, -1), new Vector3(70, 24, 1));

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
