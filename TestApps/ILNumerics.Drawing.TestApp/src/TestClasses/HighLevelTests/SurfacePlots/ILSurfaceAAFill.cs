using System.Drawing;
using ILNumerics;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;
using static ILNumerics.Globals;
using static ILNumerics.ILMath;

namespace ILNumerics.DrawingTestApplication.TestClasses.AntialiasingTests.Complex
{
    class SurfaceAAFill : TestScene
    {
        public SurfaceAAFill() : base(
            TestCategory.HL_SurfacePlots,
            "Solid Wireframe Test in Perspective Projection",
            @"[TestCase]:
Check on both renders how the solid wireframe looks. 
Wireframe must be smooth anti-aliased. on both renders check if all pixels are drawn exactly.
")
        { }

        public override Scene GetScene()
        {
            var scene = new Scene();


            scene.Camera.Add(new Surface(SpecialData.sincf(50, 50, 2.5f), colormap: Colormaps.Jet)
            {
                Wireframe = { Visible = true, Color = Color.Black, Width = 2, Antialiasing = true },
                Fill = { Visible = true }
            });

            scene.Camera.Position = new Vector3(25, 25, 300);
            scene.Camera.LookAt = new Vector3(25, 25, 0);

            scene.Camera.Configure();

            return scene;
        }
    }
}
