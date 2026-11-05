using ILNumerics;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ILNumerics.Globals;
using static ILNumerics.ILMath;

namespace ILNumerics.DrawingTestApplication.TestClasses.BasicRendering
{
    class RotatingLabels : TestScene
    {

        public RotatingLabels() : base(
            TestCategory.RA_BasicRendering,
            "RotatingLabels",
             @"")
        { }

        public override Scene GetScene()
        {
            var scene = new Scene();

            // create some data 
            Array<double> A = rand(1, 20) * 4.0 + arange(1.0, 20).T;

            // add + configure plot cube
            var plotCube = scene.Add(new PlotCube
            {
                // configure Axes 
                Axes = {
                    XAxis = {
                        Label = {
                            Text = @"\bfDates"
                        },
		                // automatic label positioning 
		                LabelPosition = new Vector3(1f, 0.2, 0),
                        LabelAnchor = new PointF(1, 0),
	                    // switch to manual tick mode
	                    Ticks = { Mode = TickMode.Manual, Width = 2 },
		                // we could add all ticks here - but do it in the loop below ...
	                },
                    YAxis = {
                        Label = {
                            Text = @"\bfTemperature \fontsize{+3}\vartheta \reset[°C]"
                        },
		                // automatic label positioning
		                LabelPosition = new Vector3(1, 1, 0),
                        LabelAnchor = new PointF(1, 0.3f)
                    },
                },
                // scale Y axis logarithmically 
                ScaleModes = { YAxisScale = AxisScale.Logarithmic },
                // add line plot and configure markers
                Children = {
                    new LinePlot(tosingle(A),
                    markerStyle: MarkerStyle.TriangleUp) {
                        Marker = {
                            Fill = { Color = Color.Red },
                            Size = 14
                        }
                    }
                }
            });
            // configure every tick individually: 
            for (int i = 1; i < A.Length; i += 2)
            {
                DateTime date = new DateTime(2000 + i, 2, i);
                plotCube.Axes.XAxis.Ticks.Mode = TickMode.Manual; 
                var tick = plotCube.Axes.XAxis.Ticks.Add(i, date.ToShortDateString());
                // first rotate some ticks to the left ...
                if (i > 14)
                {
                    tick.Label.Rotation = 0.5f;
                    tick.Label.Anchor = new PointF(0, 0);
                }
                else
                {
                    // ... rotate the rest to the right
                    tick.Label.Rotation = -0.5f;
                    tick.Label.Anchor = new PointF(1, 0);
                }
                tick.Label.Color = Color.FromArgb(40, 40, 50);
                tick.Label.Tag = i;
                //tick.Label.Font = new Font(tick.Label.Font, FontStyle.Bold);
            }
            return scene;
        }

    }
}

