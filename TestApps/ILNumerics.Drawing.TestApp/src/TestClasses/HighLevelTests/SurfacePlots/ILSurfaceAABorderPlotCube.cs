using System.Drawing;
using ILNumerics;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;
using static ILNumerics.Globals;
using static ILNumerics.ILMath;

namespace ILNumerics.DrawingTestApplication.TestClasses.AntialiasingTests.Complex
{
    class SurfaceAABorderPlotCube : TestScene
    {
        public SurfaceAABorderPlotCube() : base(
            TestCategory.HL_SurfacePlots,
            "Solid Wireframe Test in PlotCube",
            @"[TestCase]:
Check on both renders how the solid wireframe looks. 
Wireframe must be smooth anti-aliased. on both renders check if all pixels are drawn exactly.
")
        { }

        public override Scene GetScene()
        {
            var scene = new Scene();


            scene.Add(new PlotCube(twoDMode: false)
            {
                new Surface(SpecialData.sincf(50, 50, 2.5f), colormap: Colormaps.Jet)
                {                    
                    Wireframe = { Visible = true, Color = Color.Black, Width = 2, Antialiasing = true },
                    Fill = { Visible = true, Antialiasing = false },
                    //Children = { new Colorbar() }
                }
                
            });
            scene.First<PlotCube>().Rotation = Matrix4.Rotation(Vector3.UnitX, .8f) * Matrix4.Rotation(Vector3.UnitZ, .6f);

            /*var cb = scene.First<Colorbar>();
            cb.Background.Color = Color.FromArgb(230, 230, 255);           

            cb.Axis.Ticks.LabelTransformFunc = (ind, val) =>
            {
                return TickCollection.DefaultLabelTransformFunc(ind, val) + " nm";
            };*/
            
            
            return scene;
        }
    }
}
