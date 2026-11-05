using System.Drawing;
using ILNumerics;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;
using static ILNumerics.Globals;
using static ILNumerics.ILMath;


namespace ILNumerics.DrawingTestApplication.TestClasses.AntialiasingTests
{
    class SphereAntialiasedWireframeAlphaTest : TestScene
    {
        public SphereAntialiasedWireframeAlphaTest() : base(
            TestCategory.HL_Spheres,
            "Anti-aliasing of Transparent Wireframe Only",
            @"[TestCase]:
Wireframe: Anti-aliased, Transparent(100)
Triangle fill: NOT Anti-aliased, Solid Green colored

Transparent wireframe is antialiased. Additional anti-aliasing pixels must be generated according to the alpha value of line color. 
Not from 255 to 0, but from 100 to 0! All wireframe pixels must be smooth blended with triangle fill.
Must be drawn without any artefacts on both renders!")
        { }

        public override Scene GetScene()
        {
            var scene = new Scene();

            scene.Camera.Add(new Sphere()
            {
                Wireframe = { Visible = true, Color = Color.FromArgb(100, Color.Black), Width = 2, Antialiasing = true },
                Fill = { Color = Color.Green, Antialiasing = false }
            });

            scene.Camera.Configure();

            return scene;
        }

    }
}
