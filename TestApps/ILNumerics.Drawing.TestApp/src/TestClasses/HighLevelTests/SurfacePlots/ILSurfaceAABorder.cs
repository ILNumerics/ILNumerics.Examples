using System.Drawing;
using ILNumerics;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;
using static ILNumerics.Globals;
using static ILNumerics.ILMath;

namespace ILNumerics.DrawingTestApplication.TestClasses.AntialiasingTests.Complex
{
    class SurfaceAABorder:TestScene
    {
        public SurfaceAABorder() : base(
            TestCategory.HL_SurfacePlots,
            "Surface Wireframe Z-fighting Test",
            @"[TestCase]:
Switch to the GDI Renderer. Check if all wireframe lines are drawn exactly, and each pixel of wireframe lines are completly drawn.
Try to rotate the surface plot. And check the lines drawing.")
        { }

        public override Scene GetScene()
        {
            var scene = new Scene();
            
            scene.Camera.Add(new Surface(SpecialData.sincf(50, 50, 2.5f))
            {
                Wireframe = { Visible = true, Color = Color.Black, Width = 1, Antialiasing = false },
                Fill = { Visible = true, Antialiasing = false, Color = Color.LightGray }
            });
            
            
            scene.Camera.Position = new Vector3(25, 25, 300);
            scene.Camera.LookAt = new Vector3(25, 25, 0);

            scene.Camera.Configure();

            return scene;
        }
    }
}
