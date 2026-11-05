using ILNumerics;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ILNumerics.Globals;
using static ILNumerics.ILMath;

namespace ILNumerics.DrawingTestApplication.TestClasses.BasicRendering
{
    class LinesPointsMatching : TestScene
    {

        public LinesPointsMatching() : base(
            TestCategory.RA_BasicRendering,
            "LinesPointsMatching",
             @"")
        { }

        public override Scene GetScene()
        {
            var scene = new Scene();

            int count = 100;            
            var points = scene.Camera.Add(Shapes.Point);            
            points.Positions.Update(tosingle(rand(3, count)));
            
            points.Positions.Update(count, count, tosingle(randn(3, count)));
            points.Colors.Update(repmat(array<float>(0f, 1f, 0f, 1f), 1, count));
            points.Colors.Update(count, count, tosingle(rand(4, count)));
            points.Color = null;

            // create a line shape
            var line = scene.Camera.Add(new Lines
            {
                // reuse the positions buffer from our points 
                Positions = points.Positions
            });
            return scene;
        }

    }
}
