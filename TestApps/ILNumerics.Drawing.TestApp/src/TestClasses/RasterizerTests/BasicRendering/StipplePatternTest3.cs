using System.Drawing;
using ILNumerics;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;
using static ILNumerics.Globals;
using static ILNumerics.ILMath;

namespace ILNumerics.DrawingTestApplication.TestClasses.AntialiasingTests
{
    class StipplePaternTest3 : TestScene
    {
        public StipplePaternTest3() : base(
            TestCategory.RA_BasicRendering,
            "Stipple Patern Rendering Width 3",
            @"Two dash linestrips are drawn(width = 1px and 5 px). 
Try to resize the window, stipple pattern must be continued through the points."
        )
        { }


        public override Scene GetScene()
        {
            var scene = new Scene();

            
            Array<float> pos1 = array(new float[,] { { 0, 1, 2, 3, 4, 5, 5, 7, 8, 9 }, { 0, 1.0f, -1.0f, 2.0f, float.NaN, -1.0f, 1.0f, -1.0f, 0, 0 } });
            Array<float> pos2 = pos1;
            pos2[1, full] = pos2[1, full] + 5;

            var pc = scene.Add(new PlotCube(twoDMode: true)
            {
                new LinePlot(pos1, lineColor: Color.Red, lineWidth: 1, lineStyle:DashStyle.PointDash, markerStyle:MarkerStyle.None),
                new LinePlot(pos2, lineColor: Color.Red, lineWidth: 3, lineStyle:DashStyle.PointDash, markerStyle:MarkerStyle.None) { Line= { Antialiasing = false } },
                new Circle(resolution:20)
                {
                    Border = {Color = Color.Blue, DashStyle = DashStyle.PointDash, Width = 3, Antialiasing = false } ,
                    Fill = { Color = Color.Empty }
                },
                new LineStrip() {
                    Positions = array(new float[,] {{ 0,-1,0}, { 2,2,1} }).T,
                }
            });
            pc.Lines.Visible = false;
            pc.Axes.Visible = false;            


            return scene;
        }

    }
}
