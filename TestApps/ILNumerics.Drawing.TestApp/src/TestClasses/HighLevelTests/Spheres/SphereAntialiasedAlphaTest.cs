using System.Drawing;
using ILNumerics;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;
using static ILNumerics.Globals;
using static ILNumerics.ILMath;


namespace ILNumerics.DrawingTestApplication.TestClasses.AntialiasingTests
{
    class SphereAntialiasedAlphaTest : TestScene
    {
        public SphereAntialiasedAlphaTest() : base(
            TestCategory.HL_Spheres,
            "Anti-aliasing of Transparent Lines and Triangles",
            @"[TestCase]:
Wireframe: Anti-aliased, Transparent
Triangle fill: Anti-aliased, Transparent

Colors of transparent line pixels should be smooth blended with transparen triangle pixels.
Must be drawn without any artefacts on both renders!")
        { }
        
        public override Scene GetScene()
        {
            var scene = new Scene();

            scene.Camera.Add(new Sphere()
            {
                Wireframe = { Visible = true, Color = Color.FromArgb(100, Color.Black), Width = 2, Antialiasing = true },
                Fill = { Color = Color.FromArgb(100, Color.Green), Antialiasing = true }
            });

            scene.Camera.Configure();

            return scene;
        }
    }
}
