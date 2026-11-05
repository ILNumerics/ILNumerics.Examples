using System.Drawing;
using ILNumerics;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;
using static ILNumerics.Globals;
using static ILNumerics.ILMath;

namespace ILNumerics.DrawingTestApplication.TestClasses
{
    class IntersectionLinesColorVariedWidthVaried : TestScene
    {
        public IntersectionLinesColorVariedWidthVaried() : base(
            TestCategory.RA_IntersectionLinesVariedParameters,
            "Line Plot: varied color and width",
            @"[TestCase]:
This creates a number of line plots that are all single-colored but vary in terms of width. Each of the crooked lines intersect with a vertical line. Click to switch the 
color of the crooked lines from red to empty. The remaining vertical lines should be consistent. Also the behavior should be the same for different line widths.")
        { }

        

        public override Scene GetScene()
        {
            var scene = new Scene();
            var plotCube = scene.Add(new PlotCube());
            

            int n = 13;
            Array<float> alpha = linspace<float>(0, 360, n);
            Array<float> X0 = zeros<float>(2, n);
            Array<float> Y0 = zeros<float>(2, n);
            for (int i = 0; i < n; i++)
            {
                X0[":", i] = 0.5f * array((float)-sin((float)alpha[i] * pi / 180), (float)sin((float)alpha[i] * pi / 180));
                Y0[":", i] = 0.5f * array((float)-cos((float)alpha[i] * pi / 180), (float)cos((float)alpha[i] * pi / 180));
            }

            Array<float> X1 = vertcat(vec<float>(0, 2, n * 2).T, vec<float>(0, 2, n * 2).T);
            Array<float> Y1 = vertcat(ones<float>(1, n + 1), ones<float>(1, n + 1) * 2);

            Array<float> YOffset = vec<float>(0, 2, 5).T;
            Array<int> LineWidth = new int[] { 1, 5, 10 };

            for (int k = 0; k < 3; k++)
            {
                for (int j = 0; j < n; j++)
                {
                    plotCube.Add(new LinePlot(X0[":", j] + X1[":", j], Y0[":", j] + 1.5f + YOffset[k] + 14, lineColor: Color.Red, lineWidth: (int)LineWidth[0], tag: "CrookedLine"));
                    plotCube.Add(new LinePlot(X1[":", j], Y1[":", j] + YOffset[k] + 14, lineColor: Color.Green, lineWidth: (int)LineWidth[k]));

                    plotCube.Add(new LinePlot(X0[":", j] + X1[":", j], Y0[":", j] + 1.5f + YOffset[k] + 7, lineColor: Color.Red, lineWidth: (int)LineWidth[1], tag: "CrookedLine"));
                    plotCube.Add(new LinePlot(X1[":", j], Y1[":", j] + YOffset[k] + 7, lineColor: Color.Blue, lineWidth: (int)LineWidth[k]));

                    plotCube.Add(new LinePlot(X0[":", j] + X1[":", j], Y0[":", j] + 1.5f + YOffset[k], lineColor: Color.Red, lineWidth: (int)LineWidth[2], tag: "CrookedLine"));
                    plotCube.Add(new LinePlot(X1[":", j], Y1[":", j] + YOffset[k], lineColor: Color.Purple, lineWidth: (int)LineWidth[k]));                

                    
                }

                plotCube.Add(new Drawing.Label("Vertical Line: Width: " + ((int)LineWidth[k]).ToString()) { Position = new Vector3(1.75f, 16.25f + k * 2, 0),
                    Font = new Font(FontFamily.GenericSansSerif, 8), Color = Color.Orange });

                plotCube.Add(new Drawing.Label("Vertical Line: Width: " + ((int)LineWidth[k]).ToString()) { Position = new Vector3(1.75f, 9.25f + k * 2, 0),
                    Font = new Font(FontFamily.GenericSansSerif, 8), Color = Color.Green });

                plotCube.Add(new Drawing.Label("Vertical Line: Width: " + ((int)LineWidth[k]).ToString()) { Position = new Vector3(1.75f, 2.25f + k * 2, 0),
                    Font = new Font(FontFamily.GenericSansSerif, 8), Color = Color.Purple });

                plotCube.Add(new Drawing.Label("Crooked Line: Width: " + ((int)LineWidth[k]).ToString()) { Position = new Vector3(12.5f, 21 - k * 7, 0),
                    Font = new Font(FontFamily.GenericSansSerif, 12), Color = Color.Red });


            }

            plotCube.Limits.Set(new Vector3(-1, 0, -5), new Vector3(25, 22, 5));

            var linePlots = plotCube.Find<LinePlot>("CrookedLine");
            plotCube.MouseClick += (_s, _a) => {
                if (!_a.DirectionUp) return;

                if (linePlots != null)
                {                    
                    foreach (LinePlot element in linePlots)
                    {
                        if (element.Line.Color == Color.Red)
                        {
                            element.Line.Color = Color.Empty;
                        }
                        else if (element.Line.Color == Color.Empty) {
                            element.Line.Color = Color.Red;
                        }
                        element.Configure();                       
                    }  
                }
                _a.Refresh = true;

            };

            return scene;
        }
    }
}
