using System.Drawing;
using ILNumerics;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;
using static ILNumerics.Globals;
using static ILNumerics.ILMath;

namespace ILNumerics.DrawingTestApplication.TestClasses
{
    class LineStyleWidthAlphaVariedMultiColor : TestScene
    {
        public LineStyleWidthAlphaVariedMultiColor() : base(
            TestCategory.RA_LinesVariedParameters,
            "Line Plot: varied style, width and alpha, multi-colored",
            @"[TestCase]:
This creates a number of line plots that are all single-colored but vary in terms of line style, width and transparency. Click on the plot cube to switch antialiasing on or off.
Please check if the line styles are consistent for all widths. Focus on what happens at sharp edges and if the orientation of the line changes. Also, make sure the color 
gradient is consistent. Check if the transperancy of the line has an influence.")
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

            Array<float> RainbowColors = array(new float[,] { { 255, 91, 165, 255 }, { 255, 132, 188, 255 }, { 255, 197, 218, 255 },
                    { 219, 73, 172, 255 }, { 228, 114, 191, 255 }, { 240, 173, 219, 255 }, { 153, 87, 205, 255 }, { 176, 124, 218, 255 },
                    { 209, 178, 234, 255 }, { 67, 142, 200, 255 }, { 107, 167, 214, 255 }, { 168, 204, 232, 255 }, { 59, 198, 182, 255 },
                    { 100, 212, 199, 255 } }).T;
            RainbowColors = RainbowColors / 255;

            for (int i = 0; i < 6; i++)
            {
                var linePlot1 = plotCube.Add(new LinePlot(X: XFinal, Y: YFinal + i * 2, lineColor: Color.Black, lineWidth: i * 2 + 1, lineStyle: DashStyle.Solid) { Alpha = 0.6f });
                var linePlot2 = plotCube.Add(new LinePlot(X: XFinal + 8, Y: YFinal + i * 2, lineColor: Color.Black, lineWidth: i * 2 + 1, lineStyle: DashStyle.Dashed) { Alpha = 0.6f });
                var linePlot3 = plotCube.Add(new LinePlot(X: XFinal + 16, Y: YFinal + i * 2, lineColor: Color.Black, lineWidth: i * 2 + 1, lineStyle: DashStyle.Dotted) { Alpha = 0.6f });
                var linePlot4 = plotCube.Add(new LinePlot(X: XFinal + 24, Y: YFinal + i * 2, lineColor: Color.Black, lineWidth: i * 2 + 1, lineStyle: DashStyle.PointDash) { Alpha = 0.6f });

                RainbowColors["3;:"] = ones<float>(1, 14) * 0.6f;
                linePlot1.Line.Color = null;
                linePlot1.Line.Colors = RainbowColors;

                linePlot2.Line.Color = null;
                linePlot2.Line.Colors = RainbowColors;

                linePlot3.Line.Color = null;
                linePlot3.Line.Colors = RainbowColors;

                linePlot4.Line.Color = null;
                linePlot4.Line.Colors = RainbowColors;

                linePlot1 = plotCube.Add(new LinePlot(X: XFinal, Y: YFinal + i * 2 + 12.5f, lineColor: Color.Black, lineWidth: i * 2 + 1, lineStyle: DashStyle.Solid) { Alpha = 0.2f });
                linePlot2 = plotCube.Add(new LinePlot(X: XFinal + 8, Y: YFinal + i * 2 + 12.5f, lineColor: Color.Black, lineWidth: i * 2 + 1, lineStyle: DashStyle.Dashed) { Alpha = 0.2f });
                linePlot3 = plotCube.Add(new LinePlot(X: XFinal + 16, Y: YFinal + i * 2 + 12.5f, lineColor: Color.Black, lineWidth: i * 2 + 1, lineStyle: DashStyle.Dotted) { Alpha = 0.2f });
                linePlot4 = plotCube.Add(new LinePlot(X: XFinal + 24, Y: YFinal + i * 2 + 12.5f, lineColor: Color.Black, lineWidth: i * 2 + 1, lineStyle: DashStyle.PointDash) { Alpha = 0.2f });

                RainbowColors["3;:"] = ones<float>(1, 14) * 0.2f;
                linePlot1.Line.Color = null;
                linePlot1.Line.Colors = RainbowColors;

                linePlot2.Line.Color = null;
                linePlot2.Line.Colors = RainbowColors;

                linePlot3.Line.Color = null;
                linePlot3.Line.Colors = RainbowColors;

                linePlot4.Line.Color = null;
                linePlot4.Line.Colors = RainbowColors;

                linePlot1 = plotCube.Add(new LinePlot(X: XFinal + 35, Y: YFinal + i * 2, lineColor: Color.Black, lineWidth: i * 2 + 1, lineStyle: DashStyle.Solid) { Alpha = 0.8f });
                linePlot2 = plotCube.Add(new LinePlot(X: XFinal + 8 + 35, Y: YFinal + i * 2, lineColor: Color.Black, lineWidth: i * 2 + 1, lineStyle: DashStyle.Dashed) { Alpha = 0.8f });
                linePlot3 = plotCube.Add(new LinePlot(X: XFinal + 16 + 35, Y: YFinal + i * 2, lineColor: Color.Black, lineWidth: i * 2 + 1, lineStyle: DashStyle.Dotted) { Alpha = 0.8f });
                linePlot4 = plotCube.Add(new LinePlot(X: XFinal + 24 + 35, Y: YFinal + i * 2, lineColor: Color.Black, lineWidth: i * 2 + 1, lineStyle: DashStyle.PointDash) { Alpha = 0.8f });

                RainbowColors["3;:"] = ones<float>(1, 14) * 0.8f;
                linePlot1.Line.Color = null;
                linePlot1.Line.Colors = RainbowColors;

                linePlot2.Line.Color = null;
                linePlot2.Line.Colors = RainbowColors;

                linePlot3.Line.Color = null;
                linePlot3.Line.Colors = RainbowColors;

                linePlot4.Line.Color = null;
                linePlot4.Line.Colors = RainbowColors;

                linePlot1 = plotCube.Add(new LinePlot(X: XFinal + 35, Y: YFinal + i * 2 + 12.5f, lineColor: Color.Black, lineWidth: i * 2 + 1, lineStyle: DashStyle.Solid) { Alpha = 0.4f });
                linePlot2 = plotCube.Add(new LinePlot(X: XFinal + 8 + 35, Y: YFinal + i * 2 + 12.5f, lineColor: Color.Black, lineWidth: i * 2 + 1, lineStyle: DashStyle.Dashed) { Alpha = 0.4f });
                linePlot3 = plotCube.Add(new LinePlot(X: XFinal + 16 + 35, Y: YFinal + i * 2 + 12.5f, lineColor: Color.Black, lineWidth: i * 2 + 1, lineStyle: DashStyle.Dotted) { Alpha = 0.4f });
                linePlot4 = plotCube.Add(new LinePlot(X: XFinal + 24 + 35, Y: YFinal + i * 2 + 12.5f, lineColor: Color.Black, lineWidth: i * 2 + 1, lineStyle: DashStyle.PointDash) { Alpha = 0.4f });

                RainbowColors["3;:"] = ones<float>(1, 14) * 0.4f;
                linePlot1.Line.Color = null;
                linePlot1.Line.Colors = RainbowColors;

                linePlot2.Line.Color = null;
                linePlot2.Line.Colors = RainbowColors;

                linePlot3.Line.Color = null;
                linePlot3.Line.Colors = RainbowColors;

                linePlot4.Line.Color = null;
                linePlot4.Line.Colors = RainbowColors;

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
