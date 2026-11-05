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
    class LineSegmentsMatchingNeighbors : TestScene
    {

        public LineSegmentsMatchingNeighbors() : base(
            TestCategory.RA_BasicRendering,
            "LineSegmentsMatchingNeighbors",
             @"Adjacent line segments must exactly match their line beginning and ends. This must be valid for all angles. Rotate the lines (CTRL + drag left mouse) to check this for all angles.")
        { }

        public override Scene GetScene()
        {
            Array<float> Y = linspace<float>(1, 20, 20);                                   // generate data
            var scene = new Scene();                                                              // create new scene
            var plotCube = scene.Add(new PlotCube());                                             // add to plotCube
            var linePlot1 = plotCube.Add(new LinePlot(Y, lineWidth: 2, tag: "linePlot1Tag"));             // add to linePlot
            var linePlot2 = plotCube.Add(new LinePlot(-Y, lineWidth: 2, tag: "linePlot2Tag"));        // add to linePlot

            plotCube.First<LinePlot>().Line.DashStyle = DashStyle.Solid;                         // modify line style 
            plotCube.First<LinePlot>(tag: "linePlot2Tag").Line.Color = Color.Orange;              // modify line color
            linePlot2.Update(-Y + sin(Y));													// modify line positions
            return scene;

        }

    }
}
