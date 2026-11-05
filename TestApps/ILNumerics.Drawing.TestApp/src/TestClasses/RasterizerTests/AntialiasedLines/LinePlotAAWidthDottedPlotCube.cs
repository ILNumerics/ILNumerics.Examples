using System.Drawing;
using ILNumerics;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;
using static ILNumerics.Globals;
using static ILNumerics.ILMath;

namespace ILNumerics.DrawingTestApplication.TestClasses.AntialiasingTests
{
    class LinePlotAAWidthDottedPlotCube : TestScene
    {
        public LinePlotAAWidthDottedPlotCube() : base(
            TestCategory.RA_AntialiasedLines,
            "Anti-Aliased Dotted Lines Drawing in PlotCube",
            @"[TestCase]:
Check on both renders how the anti-aliased lines are drawn.
Check if lines edges are correctly anti-aliased(blured).")
        { }

        public override Scene GetScene()
        {
            var scene = new Scene();

            Array<float> circle = linspace<float>(-10, 10, 50);
            Array<float> res = sqrt(100 - (circle * circle));
            Array<float> linearray = zeros<float>(3, 600);
            int i = 0;
            for (int ptr = 0; ptr < 2 * circle.Length; ptr++)
            {
                if (ptr == circle.Length || ptr == 2 * circle.Length - 1) continue;
                // start point
                linearray[0, i] = 0;
                linearray[1, i] = 0;
                linearray[2, i] = 0;
                i++;

                // end point
                linearray[0, i] = ptr > circle.Length - 1 ? circle[ptr - circle.Length] : circle[ptr];
                linearray[1, i] = ptr > circle.Length - 1 ? -res[ptr - circle.Length] : res[ptr];
                linearray[2, i] = 0;
                i++;

                // NaN point
                linearray[":", i] = float.NaN;
                i++;
            }

            scene.Add(new PlotCube(twoDMode: false)
            {
                new LinePlot(tag: "myPlot", positions: linearray, lineStyle: DashStyle.Dotted)
                {
                    Line = { Antialiasing = true, Color = Color.Blue, Width = 2 }
                }
            });

            return scene;
        }

    }
}
