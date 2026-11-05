using System.Drawing;
using ILNumerics;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;
using static ILNumerics.Globals;
using static ILNumerics.ILMath;

namespace ILNumerics.DrawingTestApplication.TestClasses.AntialiasingTests
{
    class PointsAATest : TestScene
    {
        public PointsAATest() : base(
            TestCategory.RA_AntialiasedPoints,
            "Anti-Aliased Points",
            @"[TestCase]:
Not set yet.")
        { }

        public override Scene GetScene()
        {
            var scene = new Scene();

            

            for (int i = 0; i < 40; i++)
            {
                var point = scene.Camera.Add(new Points());
                point.Size = i + 1;
                point.Positions.Update(new float[] { i, 0.0f, 0 });
                point.Antialiasing = true;
            }

            for (int i = 0; i < 40; i++)
            {
                var point = scene.Camera.Add(new Points());
                point.Size = i + 1;
                point.Positions.Update(new float[] { i, 1.0f, 0 });
                point.Antialiasing = false;
            }
            scene.Camera.Position = new Vector3(10, 0, 100);
            scene.Camera.LookAt = new Vector3(10, 0, 0);
            


            /*scene.Camera.Add(new Points());
            var points = scene.First<Points>();
            points.Size = 20;
            points.Antialiasing = true;
            //points.Positions.Update(new float[] { 0, 0, 0 });
            points.Positions.Update(tosingle(randn(3, 50)));*/



            return scene;
        }

    }
}
