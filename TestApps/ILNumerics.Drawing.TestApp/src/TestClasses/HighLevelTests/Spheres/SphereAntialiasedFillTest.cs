using System.Drawing;
using ILNumerics;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;
using static ILNumerics.Globals;
using static ILNumerics.ILMath;

namespace ILNumerics.DrawingTestApplication.TestClasses.AntialiasingTests
{
    class SphereAntialiasedFillTest:TestScene
    {

        public SphereAntialiasedFillTest() : base(
            TestCategory.HL_Spheres,
            "Sphere Basic Rendering. Depth Buffer Test on Convex Surfaces",
            @"[TestCase]:
Wireframe: NOT Anti-aliased, Solid Black colored
Triangle fill: NOT Anti-aliased, Solid Green colored

Each pixel of all wireframe lines must be drawn exactly. This case is for depth and polygon offset testing.
Must be drawn without any artefacts on both renders!")
        { }

        public override Scene GetScene()
        {
            var scene = new Scene();

            scene.Camera.Add(new Sphere()
            {
                Wireframe = { Visible = true, Color = Color.Black, Width = 1, Antialiasing = false },
                Fill = { Color = Color.Green, Antialiasing = false }
            });

            scene.Camera.Configure();

            return scene;
        }
    }
}
