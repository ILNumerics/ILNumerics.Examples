using System.Drawing;
using ILNumerics;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;
using static ILNumerics.Globals;
using static ILNumerics.ILMath;

namespace ILNumerics.DrawingTestApplication.TestClasses.AntialiasingTests
{
    class StipplePaternTest1 : TestScene
    {
        public StipplePaternTest1() : base(
            TestCategory.RA_BasicRendering,
            "Stipple Patern Rendering Width 2",
            @"Two dash dotted lines are drawn(width = 2px). 
Blue is a LinePlot Object with NaN point in the middle of dataset.
Red is a LineStrip Object with NaN point in the middle of dataset. 
Try to resize the window, stipple pattern must be continued through the points.")
        { }

        public override Scene GetScene()
        {
            var scene = new Scene();


            Array<float> positions1 = array(new float[,]{
                { 0,1,2,3,float.NaN,5,6,7, 8},
                { 2,1,2,1,2,1,2,1,2},
                { 0,0,0,0,0,0,0,0,0},
            }).T;

            Array<float> positions1Marker = array(new float[,]{
                { 0,1,2,3,4,5,6,7,8},
                { 2,1,2,1,2,1,2,1,2},
                { 0,0,0,0,0,0,0,0,0},
            }).T;

            Array<float> positions2 = array(new float[,]{
                { 0,1,2,3,float.NaN,5,6,7, 8},
                { 4,3,4,3,4,3,4,3,4},
                { 0,0,0,0,0,0,0,0,0},
            }).T;

            Array<float> positions2Marker = array(new float[,]{
                { 0,1,2,3,4,5,6,7,8},
                { 4,3,4,3,4,3,4,3,4},
                { 0,0,0,0,0,0,0,0,0},
            }).T;


            scene.Add(new PlotCube(twoDMode: false)
            {
                new LineStrip("linestrip") {Positions = positions1.T ,Color = Color.Red,DashStyle = DashStyle.PointDash, Width = 2},
                new LinePlot(positions:positions2.T, lineStyle:DashStyle.PointDash, lineWidth:2)
                {
                    Line = {Color = Color.Blue }
                },

                //draw markers
                new LinePlot(positions1Marker.T, markerStyle: MarkerStyle.Circle,lineColor:Color.Empty),
                new LinePlot(positions2Marker.T, markerStyle:MarkerStyle.Circle,lineColor:Color.Empty),
            });
            


            return scene;
        }

    }
}
