using System.Drawing;
using ILNumerics;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;
using static ILNumerics.Globals;
using static ILNumerics.ILMath;

namespace ILNumerics.DrawingTestApplication.TestClasses
{
    class PointsSizeAlphaVariedUniColor : TestScene
    {
        public PointsSizeAlphaVariedUniColor() : base(
            TestCategory.RA_PointsVariedParameters,
            "Points: varied size and transparency, single-colored",
            @"[TestCase]:
This creates a number of points that are all single-colored but vary in terms of style and transparency. Click on the plot cube to switch antialiasing on or off.
Please, focus on the influence of antialiasing on different sizes and transparencies.")
        { }

        

        public override Scene GetScene()
        {
            var scene = new Scene();
            var plotCube = scene.Add(new PlotCube());

            int numberOfPoints = 20;

            Array<float> pointsPositions = zeros<float>(3, numberOfPoints);
            pointsPositions["0;:"] = linspace<float>(0, 1, numberOfPoints);

            for (int i = 0; i < 20; i++)
            {
                pointsPositions["1", i] = 0.1f;
                plotCube.Add(new Points() { Size = i + 1, Positions = pointsPositions[":", i], Color = Color.FromArgb(255, 250, 128, 114) });
                pointsPositions["1", i] = 0.0f;
                plotCube.Add(new Points() { Size = i + 1, Positions = pointsPositions[":", i], Color = Color.FromArgb(204, 250, 128, 114) });
                pointsPositions["1", i] = -0.1f;
                plotCube.Add(new Points() { Size = i + 1, Positions = pointsPositions[":", i], Color = Color.FromArgb(153, 250, 128, 114) });
                pointsPositions["1", i] = -0.2f;
                plotCube.Add(new Points() { Size = i + 1, Positions = pointsPositions[":", i], Color = Color.FromArgb(76, 250, 128, 114) });
            }

            plotCube.Add(new Drawing.Label("Alpha: 1") { Position = new Vector3(0.6f, 0.125f, 0), Font = new Font(FontFamily.GenericSansSerif, 12) });
            plotCube.Add(new Drawing.Label("Alpha: 0.8") { Position = new Vector3(0.6f, 0.025f, 0), Font = new Font(FontFamily.GenericSansSerif, 12) });
            plotCube.Add(new Drawing.Label("Alpha: 0.6") { Position = new Vector3(0.6f, -0.075f, 0), Font = new Font(FontFamily.GenericSansSerif, 12) });
            plotCube.Add(new Drawing.Label("Alpha: 0.4") { Position = new Vector3(0.6f, -0.175f, 0), Font = new Font(FontFamily.GenericSansSerif, 12) });            

            plotCube.Limits.Set(new Vector3(0.1f, -0.225f, -1), new Vector3(1.1f, 0.15f, 1));

            var label = plotCube.Axes.YAxis.Add(new Drawing.Label());
            label.Position = new Vector3(0.15f, 1.025f, 0);

            label.Font = new Font(FontFamily.GenericSansSerif, 20);
            label.Color = Color.OrangeRed;
            label.Text = "Antialiasing off";

            var points = plotCube.Find<Points>();
            plotCube.MouseClick += (_s, _a) => {
                if (!_a.DirectionUp) return;

                if (points != null)
                {
                    foreach (Points element in points)
                    {
                        element.Antialiasing = !element.Antialiasing;
                        element.Configure();
                        if (element.Antialiasing)
                        {
                            label.Text = "Antialiasing on";
                        }
                        else
                        {
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
