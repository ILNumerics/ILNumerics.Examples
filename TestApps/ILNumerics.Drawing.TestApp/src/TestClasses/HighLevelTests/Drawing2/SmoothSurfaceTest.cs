using System;
using System.Drawing;
using ILNumerics;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;
using static ILNumerics.Globals;
using static ILNumerics.ILMath;

namespace ILNumerics.DrawingTestApplication.TestClasses
{
    class SmoothSurfaceTest : TestScene {
        public SmoothSurfaceTest() : base(
            TestCategory.HL_SurfacePlots,
            "Smooth surface plot test",
            @"[TestCase]:
This creates a smooth surface plot with adjustable resolution, similar to the documentation example. Are the buttons working? Is the general result reasonable? Does mouse interaction work?") { }

        public override Scene GetScene() {
            Array<uint> pickValues = touint32(rand(1, 200) * 401 * 401);  // sequential indices into terrain matrix
            Array<float> xyPoints = tosingle(vertcat(pickValues / 401, // X 'positions' of sequential indices in terrain matrix
                                                        pickValues % 401)); // Y 'positions' of sequential indices in terrain matrix
            Array<float> Z = tosingle(SpecialData.terrain[pickValues]);  // pick random values by sequential indices

            // create the smooth surface
            var smooth = new SmoothSurface(xyPoints, Z,
                             resolution: new System.Drawing.Size(50, 50),
                             smoothingFactor: 7);

            // add it to a PlotCube:
            var scene = new Scene() {
                  new PlotCube(twoDMode: false) {
                        Children = { smooth },
                        Rotation = Matrix4.Rotation(new Vector3(1,0,0), 1.0) *
                                    Matrix4.Rotation(new Vector3(0,0,1), 1.0),
                        AspectRatioMode = AspectRatioMode.StretchToFill,
                        RotationMethod = RotationMethods.AltitudeAzimuth
                  }
            };

            // change the color of the original points (optional)
            smooth.PointsOriginal.Color = System.Drawing.Color.Gray;

            // some configuration to the surface (optional)
            smooth.Surface.Colormap = Colormaps.Jet;
            smooth.Surface.Add(new Colorbar());

            // add some 'GUI' 'buttons'
            var btnResH = scene.Screen.Add(new Title("Resol.: high", Positioning.TopRight, "Rh"));
            var btnResM = scene.Screen.Add(new Title("Resol.: mid", Positioning.TopCenter, "Rm"));
            var btnResL = scene.Screen.Add(new Title("Resol.: low", Positioning.TopLeft, "Rl"));
            var btnSmoothH = scene.Screen.Add(new Title("Smooth: high", Positioning.BottomRight, "Sh"));
            var btnSmoothM = scene.Screen.Add(new Title("Smooth: mid", Positioning.BottomCenter, "Sm"));
            var btnSmoothL = scene.Screen.Add(new Title("Smooth: low", Positioning.BottomLeft, "Sl"));
            btnResH.Movable = false; btnResM.Movable = false; btnResL.Movable = false;
            btnSmoothH.Movable = false; btnSmoothM.Movable = false; btnSmoothL.Movable = false;
            btnResL.MouseClick += (s, e) => updateRes(50, e);
            btnResM.MouseClick += (s, e) => updateRes(100, e);
            btnResH.MouseClick += (s, e) => updateRes(150, e);
            btnSmoothL.MouseClick += (s, e) => updateSmooth(5, e);
            btnSmoothM.MouseClick += (s, e) => updateSmooth( 10, e);
            btnSmoothH.MouseClick += (s, e) => updateSmooth( 20, e);
            return scene;

            void updateRes(int resolution, Drawing.MouseEventArgs ev) {
                var smoothSurf = scene.First<SmoothSurface>();
                if (smoothSurf == null) return;
                smoothSurf.Resolution = new System.Drawing.Size(resolution, resolution);
                smoothSurf.Configure(); ev.Refresh = true; ev.Cancel = true;
            }
            void updateSmooth(int smoothing, Drawing.MouseEventArgs ev) {
                var smoothSurf = scene.First<SmoothSurface>();
                if (smoothSurf == null) return;
                smoothSurf.SmoothingFactor = (uint)smoothing;
                smoothSurf.Configure(); ev.Refresh = true; ev.Cancel = true;
            }
        }

        private void BtnResHH_MouseClick(object sender, Drawing.MouseEventArgs e) {
            throw new NotImplementedException();
        }
    }
}
