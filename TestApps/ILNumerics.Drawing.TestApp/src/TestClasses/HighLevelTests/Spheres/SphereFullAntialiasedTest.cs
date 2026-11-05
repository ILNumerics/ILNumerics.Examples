using System.Drawing;
using ILNumerics;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;
using static ILNumerics.Globals;
using static ILNumerics.ILMath;

namespace ILNumerics.DrawingTestApplication.TestClasses.AntialiasingTests
{
    class SphereFullAntialiasedTest : TestScene
    {

        public SphereFullAntialiasedTest() : base(
            TestCategory.HL_Spheres,
            "Anti-aliasing of Triangles and Wireframe",
            @"[TestCase]:
Wireframe: Anti-aliased, Solid Colored
Triangle fill: Anti-aliased, Solid Colored

On both renders the lines of Wireframe must be drawn exactly with Z-Buffer Offset, anti-aliased. 
Must be drawn without any artefacts on both renders!")
        { }

        public override Scene GetScene()
        {
            var scene = new Scene();

            scene.Camera.Add(new Sphere()
            {
                Wireframe = { Visible = true, Color = Color.Black, Width = 2, Antialiasing = true },
                Fill = { Color = Color.Green, Antialiasing = true }
            });

            scene.Camera.Configure();

            return scene;
        }
    }
}
