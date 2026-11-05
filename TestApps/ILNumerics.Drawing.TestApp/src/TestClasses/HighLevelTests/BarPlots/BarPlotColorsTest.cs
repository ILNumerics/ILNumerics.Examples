using System.Drawing;
using ILNumerics;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;
using static ILNumerics.Globals;
using static ILNumerics.ILMath; 

namespace ILNumerics.DrawingTestApplication.TestClasses.AntialiasingTests.Complex
{
    class BarPlotExColormapTest : TestScene
    {
        public BarPlotExColormapTest() : base(
            TestCategory.HL_BarPlots,
            "BarPlotEx Colors Test",
            @"[TestCase]:
BarPlotEx Colormapped. 
Strictly monotonic gradient fill color along the each bar according to defined colormap.")
        { }

        public override Scene GetScene()
        {
            var scene = new Scene();

            scene.Add(new PlotCube(twoDMode: false)
            {
                new BarPlotEx(Z: SpecialData.sincf(10, 10, 1.5f))
                {
                    BarWidth = 0.8f,
                    BaseValue = -1.0f,
                    Colormap = Colormaps.Jet,
                    ColorMode = BarPlotEx.ColorModes.Colormapped,

                    Border = { Visible = true, Color = Color.Black, Width = 1, Antialiasing = false },
                    Fill = { Visible = true, Antialiasing = false },

                    Children = { new Colorbar() }
                }
            });
            scene.First<PlotCube>().Rotation = Matrix4.Rotation(Vector3.UnitX, .8f) * Matrix4.Rotation(Vector3.UnitZ, .6f);
            return scene;
        }
    }
}
