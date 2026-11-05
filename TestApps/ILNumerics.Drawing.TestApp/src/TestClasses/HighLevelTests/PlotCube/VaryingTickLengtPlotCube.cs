using System.Drawing;
using ILNumerics;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;
using ILNumerics.F2NET.Formatting;
using static ILNumerics.Globals;
using static ILNumerics.ILMath;

namespace ILNumerics.DrawingTestApplication.TestClasses.Plotting
{
    class VaryingTickLengtPlotCube : TestScene
    {
        public VaryingTickLengtPlotCube() : base(
            TestCategory.HL_PlotCube,
            "PlotCube axis ticks with varying positive / negative lengths",
            @"[TestCase]: change the default tick length of all axis ticks by pushing [+] &/ [-] buttons. Check to make sure that ticks run into correct direction, have correct lengths and that tick labels remain tightly coupled to the plot cubes outside edge / tick end.")
        { }

        public override Scene GetScene()
        {
            var scene = new Scene();

            var plotCube = scene.Add(new PlotCube(twoDMode: false)
            {
                new LinePlot(SpecialData.sincos1Df(250, 10).T),
                new Title("[ -> \\bf+ \\reset <- ]", tag: "plus", position: Positioning.TopLeft), 
                new Title("[ -> \\bf- \\reset <- ]", tag: "minus", position: Positioning.TopRight),
            });

            scene.First<Title>(tag: "plus").MouseClick += (object sender, ILNumerics.Drawing.MouseEventArgs e) => {
                if (e.DirectionUp) {
                    var pc = (sender as Title)?.FirstUp<PlotCube>();
                    if (pc != null) {
                        var tl = pc.Axes.XAxis.Ticks.TickLength + 0.2f;
                        pc.Axes.XAxis.Ticks.TickLength = tl;
                        pc.Axes.XAxis.Label.Text = "TickLength: " + tl;
                        pc.Axes.YAxis.Ticks.TickLength = tl;
                        pc.Axes.YAxis.Label.Text = "TickLength: " + tl;
                        pc.Axes.ZAxis.Ticks.TickLength = tl;
                        pc.Axes.ZAxis.Label.Text = "TickLength: " + tl;
                        pc.Configure();
                        e.Refresh = true;
                    }
                }
            };
            scene.First<Title>(tag: "minus").MouseClick += (object sender, ILNumerics.Drawing.MouseEventArgs e) => {
                if (e.DirectionUp) {
                    var pc = (sender as Title)?.FirstUp<PlotCube>();
                    if (pc != null) {
                        var tl = pc.Axes.XAxis.Ticks.TickLength - 0.2f;
                        pc.Axes.XAxis.Ticks.TickLength = tl;
                        pc.Axes.XAxis.Label.Text = "TickLength: " + tl;
                        pc.Axes.YAxis.Ticks.TickLength = tl;
                        pc.Axes.YAxis.Label.Text = "TickLength: " + tl;
                        pc.Axes.ZAxis.Ticks.TickLength = tl;
                        pc.Axes.ZAxis.Label.Text = "TickLength: " + tl;
                        pc.Configure();
                        e.Refresh = true;
                    }
                }
            };

            return scene;
        }

    }
}
