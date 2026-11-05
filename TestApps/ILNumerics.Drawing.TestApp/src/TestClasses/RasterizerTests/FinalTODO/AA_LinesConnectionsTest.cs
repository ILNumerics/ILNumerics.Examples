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
    class AA_LinesConnectionsTest : TestScene
    {

        public AA_LinesConnectionsTest() : base(
            TestCategory.RA_BasicRendering,
            "AA_LinesConnectionsTest",
             @"")
        { }

        public override Scene GetScene()
        {
            var scene = new Scene();

            Array<float> Y = linspace<float>(1, 20, 20);   
            
            var plotCube = scene.Add(new PlotCube());
            var linePlot1 = plotCube.Add(new LinePlot(Y, lineWidth: 2, tag: "linePlot1Tag"));
            var linePlot2 = plotCube.Add(new LinePlot(-Y, lineWidth: 2, tag: "linePlot2Tag"));

            plotCube.First<LinePlot>().Line.DashStyle = DashStyle.Dashed;
            plotCube.First<LinePlot>(tag: "linePlot2Tag").Line.Color = Color.Orange;
            linePlot2.Update(-Y + sin(Y));

            return scene;
        }

    }
}
