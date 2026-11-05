using System.Drawing;
using ILNumerics;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;
using static ILNumerics.Globals;
using static ILNumerics.ILMath;

namespace ILNumerics.DrawingTestApplication.TestClasses.AntialiasingTests.Complex
{
    class ContentFitModeLogarithmicMaintainRatios : TestScene
    {
        public ContentFitModeLogarithmicMaintainRatios() : base(
            TestCategory.HL_PlotCube,
            "PlotCube's ContentFitModes. Logarithmic Scales, MaintainRatios",
            @"[TestCase]:
UnitCube: the actual aspect ratio of the plot cubes _data content_ is not considered when fitting the plot cube into the rendering space (as controlled by AspectRatioMode). 
ContentXY: the plot cube renders into the available area for rendering in a way to maintain the _data aspect ratio_. 
Click on the scene to toogle ContentFitMode setting!
Difference is best observed by using a long, slim screen rect. See the plot cube stretching / or looking more like a cube. Here, linear content aspect ratio is 1:3. The effect is less obvious with logarithmic axes.
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
            var title = new Title($"ContentFitMode: {scene.First<PlotCube>().ContentFitMode}");
            scene.Add(title);

            scene.First<PlotCube>().ScaleModes.XAxisScale = AxisScale.Logarithmic;
            scene.First<PlotCube>().ScaleModes.YAxisScale = AxisScale.Logarithmic;
            scene.First<PlotCube>().AspectRatioMode = AspectRatioMode.MaintainRatios;
            
            scene.First<PlotCube>().Rotation = Matrix4.Rotation(Vector3.UnitX, .8f) * Matrix4.Rotation(Vector3.UnitZ, .6f);
            scene.MouseClick += (object sender, Drawing.MouseEventArgs e) => {
                if (e.DirectionUp) {
                    var cur = scene.First<PlotCube>().ContentFitMode;
                    if (cur == ContentFitModes.ContentXY) {
                        scene.First<PlotCube>().ContentFitMode = ContentFitModes.UnitCube;
                    } else {
                        scene.First<PlotCube>().ContentFitMode = ContentFitModes.ContentXY;
                    }
                    title.Label.Text = $"ContentFitMode: {scene.First<PlotCube>().ContentFitMode}";
                    e.Refresh = true;
                }
            };


            return scene;
        }

    }
}
