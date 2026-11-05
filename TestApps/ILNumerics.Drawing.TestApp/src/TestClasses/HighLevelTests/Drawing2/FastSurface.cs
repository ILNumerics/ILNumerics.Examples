using System;
using System.Drawing;
using ILNumerics;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;
using static ILNumerics.Globals;
using static ILNumerics.ILMath;

namespace ILNumerics.DrawingTestApplication.TestClasses
{
    class FastSurfaceTest : TestScene {
        public FastSurfaceTest() : base(
            TestCategory.HL_SurfacePlots,
            "Fast surface plot test",
            @"[TestCase]:
This creates a fast surface plot with many data elements. Is the general result reasonable? Does mouse interaction work?") { }

        public override Scene GetScene() {

            Array<float> Z = tosingle(SpecialData.terrain);  // pick random values by sequential indices

            // create the smooth surface
            var surface = new FastSurface();

            // add it to a PlotCube:
            var scene = new Scene() {
                  new PlotCube(twoDMode: false) { surface } 
            };

            var plotcube = scene.First<PlotCube>();
            plotcube.Rotation = Matrix4.Rotation(new Vector3(1, 0, 0), 1.0) *
                        Matrix4.Rotation(new Vector3(0, 0, 1), 1.0);

            plotcube.AspectRatioMode = AspectRatioMode.StretchToFill;
            plotcube.MouseDoubleClick += Plotcube_MouseDoubleClick;

            // change the color of the original points (optional)
            surface.Update(Z: Z, colormap: Colormaps.Jet);
            surface.Add(new Colorbar());
            surface.Configure(); 
            return scene;
        }

        private void Plotcube_MouseDoubleClick(object sender, Drawing.MouseEventArgs e) {
            (sender as PlotCube).Reset();
            e.Refresh = true; 
            e.Cancel = true;
        }

        private void BtnResHH_MouseClick(object sender, Drawing.MouseEventArgs e) {
            throw new NotImplementedException();
        }
    }
}
