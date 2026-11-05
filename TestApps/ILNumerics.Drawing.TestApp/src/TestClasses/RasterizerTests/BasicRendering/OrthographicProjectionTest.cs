using System.Drawing;
using ILNumerics;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;
using ILNumerics.Toolboxes;
using static ILNumerics.Globals;
using static ILNumerics.ILMath;

namespace ILNumerics.DrawingTestApplication.TestClasses.AntialiasingTests
{
    class OrthographicProjectionTest : TestScene
    {
        public OrthographicProjectionTest() : base(
            TestCategory.CT_CustomerTests,
            "Orthographic Projection",
            @"Issue from G.Thorwald. PLEASE IGNORE THIS TEST!!! IT IS DRAFT VERSION!!! IN DEVELOPMENT!")
        { }

        public override Scene GetScene()
        {
            var scene = new Scene();

            Gear largeGear = new Gear(inR: 0.8F, outR: 50.0F, toothR: 45.0F, toothCount: 12, thickness: 5.0F);
            scene.Camera.Add(largeGear);

            scene.Camera.Projection = Projection.Orthographic;
            scene.Camera.LookAt = new Vector3(0.5, 0.4, 0.3);
            scene.Camera.Position = new Vector3(0, 0, 60.0f);
            return scene;
        }

    }
}
