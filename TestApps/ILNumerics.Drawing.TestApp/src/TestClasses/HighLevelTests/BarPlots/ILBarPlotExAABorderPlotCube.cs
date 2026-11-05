using System.Drawing;
using ILNumerics;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;
using static ILNumerics.Globals;
using static ILNumerics.ILMath;

namespace ILNumerics.DrawingTestApplication.TestClasses.AntialiasingTests.Complex
{
    class BarPlotExAABorderPlotCube : TestScene
    {
        public BarPlotExAABorderPlotCube() : base(
            TestCategory.HL_BarPlots,
            "BarPlotEx in PlotCube Solid Wireframe Antialiasing test",
            @"[TestCase]:
Wireframe is Anti-aliased width = 2px; 
Triangle fill is not Anti-aliased.
No Transparency.

To test the different projections, BarPlotEx is placed in Plotcube. 
Try to rotate, zoon and pan the plotted object. Wireframe must be drawn as smooth solid line.
Without any z-fighting artefacts, like in nvidia 'Solid Wireframe' whitepaper.")
        { }

        public override Scene GetScene()
        {
            var scene = new Scene();


            scene.Add(new PlotCube(twoDMode: false)
            {
                new BarPlotEx(Z: SpecialData.sincf(10, 10, 1.5f))
                {
                    BarWidth = 0.8f,
                    BaseValue = -0.5f,
                    Colormap = Colormaps.Jet,
                    ColorMode = BarPlotEx.ColorModes.Colormapped,

                    Border = { Visible = true, Color = Color.Black, Width = 2, Antialiasing = true },
                    Fill = { Visible = true, Antialiasing = false }
                }
            });
            scene.First<PlotCube>().Rotation = Matrix4.Rotation(Vector3.UnitX, .8f) * Matrix4.Rotation(Vector3.UnitZ, .6f);
            return scene;
        }
    }
}
