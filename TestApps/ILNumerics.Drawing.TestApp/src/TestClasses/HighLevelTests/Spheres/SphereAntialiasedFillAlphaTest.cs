using System.Drawing;
using ILNumerics;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;
using static ILNumerics.Globals;
using static ILNumerics.ILMath;


namespace ILNumerics.DrawingTestApplication.TestClasses.AntialiasingTests
{
    class SphereAntialiasedFillAlphaTest : TestScene
    {
        public SphereAntialiasedFillAlphaTest() : base(
            TestCategory.HL_Spheres,
            "Anti-aliasing of Transparent Triangles Only",
            @"[TestCase]:
Wireframe: NOT Anti-aliased, Solid Black colored
Triangle fill: Anti-aliased, Transparent(200)

Colors of line pixels should be smooth blended with transparent triangle pixels.
Please check how the wireframe on the other side of sphere is blended with fill.
Must be drawn without any artefacts on both renders!")
        { }

        public override Scene GetScene()
        {
            var scene = new Scene();

            scene.Camera.Add(new Sphere()
            {
                Wireframe = { Visible = true, Color = Color.Black, Width = 2, Antialiasing = false },
                Fill = { Color = Color.FromArgb(200, Color.Green), Antialiasing = true }
            });

            scene.Camera.Configure();

            return scene;
        }
    }
}
