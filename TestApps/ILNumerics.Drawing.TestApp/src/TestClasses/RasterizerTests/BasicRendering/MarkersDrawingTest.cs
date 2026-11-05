using System.Drawing;
using ILNumerics;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;
using static ILNumerics.Globals;
using static ILNumerics.ILMath;

namespace ILNumerics.DrawingTestApplication.TestClasses.AntialiasingTests
{
    class MarkersDrawingTest : TestScene
    {
        public MarkersDrawingTest() : base(
            TestCategory.RA_BasicRendering,
            "MarkersDrawingTest",
            @"Small markers require exact lines and triangles rasterization.
Check that the borders are completely closed. No pixel missing?
Check that the markers fill area covers the inner area of the markers completely.
Check that round markers (dot,circle) apear reasonably round."
        )
        { }


        public override Scene GetScene()
        {
            var scene = new Scene();

            Array<float> A = ones<float>(1, 10);
            
            // add plot cube with legend
            var plotCube = scene.Add(new PlotCube {
                new Legend {
                    Location = new PointF(1,0.02f),
                    Anchor = new PointF(1,0)
                }
            });
            
            // get enumerator for all marker styles
            var styles = System.Enum.GetValues(typeof(MarkerStyle));
            float offset = 0;
            
            // iterate marker styles 
            foreach (MarkerStyle style in styles)
            {
                // add new line plot for every marker style
                plotCube.Add(new LinePlot(
                  A + offset++, tag: style, markerStyle: style));
            }


            return scene;
        }

    }
}
