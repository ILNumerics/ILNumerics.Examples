using System.Drawing;
using ILNumerics;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;
using static ILNumerics.Globals;
using static ILNumerics.ILMath;

namespace ILNumerics.DrawingTestApplication.TestClasses.AntialiasingTests.Complex
{
    class AspectRatioLogarithmicPlotCube : TestScene
    {
        public AspectRatioLogarithmicPlotCube() : base(
            TestCategory.HL_PlotCube,
            "PlotCube's AspectRatio Modes, Logarithmic Scales, ContentFitMode:UnitCube",
            @"[TestCase]:
StretchToFill: The plot cube is tightly fit into the available space of the DataScreenRect, keeping labels visible. Every dimension may be scaled individually.
MaintainRatios: all dimensions of the plot cube are scaled by the same factor, determined from the dimension, having the _smallest_ space available.
Click on the scene to toogle AspectRatioMode setting!
Difference is best observed by using a long, slim screen rect. See the plot cube stretching / or looking more like a cube. Here, content aspect ratio is 1:3.
")
        { }

        public override Scene GetScene()
        {
            var scene = new Scene();


            scene.Add(new PlotCube(twoDMode: false)
            {
                new Surface(SpecialData.sincf(150, 50, 2.5f), colormap: Colormaps.Jet)
                {                    
                    Wireframe = { Visible = true, Color = Color.DarkGray, Width = 1 },
                    Fill = { Visible = true, Antialiasing = false },
                    Children = {
                        new Colorbar()
                    }
                }, 
                
            });
            var title = new Title($"AspectRatioMode: {scene.First<PlotCube>().AspectRatioMode}");
            scene.Add(title); 

            scene.First<PlotCube>().ContentFitMode = ContentFitModes.UnitCube;

            scene.First<PlotCube>().ScaleModes.XAxisScale = AxisScale.Logarithmic;
            scene.First<PlotCube>().ScaleModes.YAxisScale = AxisScale.Logarithmic; 

            scene.First<PlotCube>().Rotation = Matrix4.Rotation(Vector3.UnitX, .8f) * Matrix4.Rotation(Vector3.UnitZ, .6f);
            scene.MouseClick += (object sender, Drawing.MouseEventArgs e) => {
                if (e.DirectionUp) {
                    var cur = scene.First<PlotCube>().AspectRatioMode;
                    if (cur == AspectRatioMode.MaintainRatios) {
                        scene.First<PlotCube>().AspectRatioMode = AspectRatioMode.StretchToFill;
                    } else {
                        scene.First<PlotCube>().AspectRatioMode = AspectRatioMode.MaintainRatios;
                    }
                    title.Label.Text = $"AspectRatioMode: {scene.First<PlotCube>().AspectRatioMode}";
                    e.Refresh = true;
                }
            };


            return scene;
        }

    }
}
