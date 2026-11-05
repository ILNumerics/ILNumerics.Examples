using System.Drawing;
using ILNumerics;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;
using static ILNumerics.Globals;
using static ILNumerics.ILMath;

namespace ILNumerics.DrawingTestApplication.TestClasses
{
    class SphereAntialiasedWireframeTest : TestScene
    {
        public SphereAntialiasedWireframeTest() : base(
            TestCategory.HL_Spheres,
            "Anti-aliasing of Solid Wireframe Only",
            @"[TestCase]:
Wireframe: Anti-aliased, Solid Colored
Triangle fill: NOT Anti-aliased, Solid Colored

Wireframe is antialiased. Additional anti-aliasing pixels must be generated and smooth blended with triangle fill.
Must be drawn without any artefacts on both renders!")
        { }

        public override Scene GetScene()
        {
            var scene = new Scene();

            scene.Camera.Add(new Sphere()
            {
                Wireframe = { Visible = true, Color = Color.Black, Width = 2, Antialiasing = true },
                Fill = { Color = Color.Green, Antialiasing = false }
            });

            scene.Camera.Configure();

            return scene;
        }
    }
}
