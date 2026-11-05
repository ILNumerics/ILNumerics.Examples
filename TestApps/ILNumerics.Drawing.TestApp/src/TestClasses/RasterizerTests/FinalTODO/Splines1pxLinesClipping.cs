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
    class MatchingClipping : TestScene
    {

        public MatchingClipping() : base(
            TestCategory.RA_BasicRendering,
            "1px Lines Clipping",
             @"Line segments must clip at the plot cube borders exactly. Move (pan) the lines with the right mouse so that they reach over the limits of the plot cube and get clipped. Check that the segments get cut off exactly. No pixels must reach over the limits. No line pixels must overlay the border pixels. The last pixel _inside_ the plot cube borders must be the last pixels drawn for each line segment. Make sure to test all 4 sides: left, right, top, bottom.")
        { }

        public override Scene GetScene()
        {
            var scene = new Scene();

            // define random values vector
            Array<float> Points = tosingle(rand(1, 10));


            // add plot cube and two line plots 
            var plotCube = scene.Add(new PlotCube(twoDMode: true) {
                new SplinePlot(positions: Points, resolution: null /* :10*/, markerStyle:MarkerStyle.Circle, lineColor: Color.CornflowerBlue),
                new SplinePlot(positions: Points, resolution: 10, useSplinePathFor1D: false, lineColor: Color.OrangeRed),
                new LinePlot(positions:Points, lineWidth: 1, markerStyle: MarkerStyle.Circle),
                new Legend("using splinepath()", "using spline()", "using LinePlot")
            });
            plotCube.Limits.Set(new Vector3(0, -0.2, 1), new Vector3(10, 1, -1));
            return scene;
        }

    }
}
