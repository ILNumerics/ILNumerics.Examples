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
    class MissingSegmentBug : TestScene
    {

        public MissingSegmentBug() : base(
            TestCategory.RA_BasicRendering,
            "MissingSegmentBug",
             @"")
        { }

        public override Scene GetScene()
        {
            var scene = new Scene();

            // generate data
            Array<float> X = -tosingle(linspace(1 * pi, 10 * pi, 25));
            Array<float> Y = X * sin(X);
            Array<float> Z = X * cos(X);
            // create plot cube
            var myPlotCube = scene.Add(new PlotCube(twoDMode: false)
            {
			    // plot original function points
			    //new LinePlot(X, Y, Z, lineColor:Color.Black, lineWidth: 2, markerStyle: MarkerStyle.Circle),
                // plot smooth SplinePlot
			    new SplinePlot(X, Y, Z, tag:"mySplinePlot", lineColor:Color.Green, lineWidth: 1, markerStyle: MarkerStyle.Circle)
            });
            
            // manipulate alpha value
            //scene.First<SplinePlot>("mySplinePlot").Alpha = 0.25f;
            // // rotate plot cube to make the spiral data in XYZ visible
            myPlotCube.Rotation = Matrix4.Rotation(new Vector3(1, 0, 0), Math.PI / 2.5)
                          * Matrix4.Rotation(new Vector3(0, 0, 1), -3 * Math.PI / 3.5);
            return scene;
        }

    }
}
