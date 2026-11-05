using System.Drawing;
using ILNumerics;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;
using static ILNumerics.Globals;
using static ILNumerics.ILMath;

namespace ILNumerics.DrawingTestApplication.TestClasses.AntialiasingTests.Complex
{
    class BarPlotExAABorder : TestScene
    {
        public BarPlotExAABorder() : base(
            TestCategory.HL_BarPlots,
            "BarPlotEx Solid Wireframe Antialiasing test",
            @"[TestCase]:
Wireframe is Anti-aliased width = 2px; 
Triangle fill is not Anti-aliased.
No Transparency.

Try to rotate, but NOT Zoom the plotted object. Wireframe must be drawn as smooth solid line.
On zoom and double click the camera matrix will be resseted. Here we need to check if any z-fighting artefacts are visible.
Must be drawn a solid wireframe like in nvidia 'Solid Wireframe' whitepaper.")
        { }

        public override Scene GetScene()
        {
            var scene = new Scene();
            
            scene.Camera.Add(new BarPlotEx(Z: SpecialData.sincf(10, 10, 2.5f))
            {
                BarWidth = 0.8f,
                BaseValue = -0.5f,
                Colormap = Colormaps.Jet,
                ColorMode = BarPlotEx.ColorModes.Solidmapped,

                Border = { Visible = true, Color = Color.Black, Width = 2, Antialiasing = true },
                Fill = { Visible = true, Antialiasing = false }
            });

            scene.Camera.Position = new Vector3(5, 5, 20);                        
            scene.Camera.LookAt = new Vector3(5, 5, 0);            
            scene.Camera.Configure();

            return scene;
        }
    }
}
