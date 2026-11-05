using System;
using System.Drawing;
using ILNumerics;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;
using static ILNumerics.Globals;
using static ILNumerics.ILMath;

namespace ILNumerics.DrawingTestApplication.TestClasses
{
    class RotationSurfaceTest : TestScene {
        public RotationSurfaceTest() : base(
            TestCategory.HL_SurfacePlots,
            "Rotation modes test",
            @"[TestCase]:
Both standard interaction mouse rotation methods are demonstrated on a regular surface plot. Toogle the rotation mode by clicking on the button in the scene and test correct behavior! ") { }

        public override Scene GetScene() {

            // add it to a PlotCube:
            var scene = new Scene() {
                  new PlotCube(twoDMode: false) {
                        Children = { new Surface(SpecialData.sincf(40,50)) },
                        Rotation = Matrix4.Rotation(new Vector3(1,0,0), 1.0) *
                                    Matrix4.Rotation(new Vector3(0,0,1), 1.0),
                        AspectRatioMode = AspectRatioMode.StretchToFill, 
                        RotationMethod = RotationMethods.AltitudeAzimuth
                  }
            };

            var pc = scene.First<PlotCube>();
            pc.RotationMethod = RotationMethods.EulerAngles;
            pc.MouseDoubleClick += (s, e) => (s as PlotCube)?.Reset(); 

            // add some 'GUI' 'buttons'
            var btnRotMode = scene.Screen.Add(new Title("", Positioning.TopRight, tag: "rotmode"));
            updateLabel(btnRotMode.First<Drawing.Label>(), pc.RotationMethod); 

            void updateLabel(Drawing.Label label, RotationMethods method) {
                label.Text = "Rotation mode: \r\n" + method;
            }

            btnRotMode.Movable = false;
            btnRotMode.MouseClick += (s, e) => toogleMode(s, e);
            return scene;

            void toogleMode(object sender, Drawing.MouseEventArgs ev) {
                if (ev.DirectionUp) {
                    //var scene = (sender as Group)?.FirstUp<Scene>();
                    var pc1 = scene.First<PlotCube>();
                    if (pc1 != null) {
                        pc1.RotationMethod = next(pc1.RotationMethod);
                        pc1.Reset();
                        updateLabel(scene.Screen.First<Title>().First<Drawing.Label>(), pc1.RotationMethod);
                        scene.Configure();
                        ev.Refresh = true; ev.Cancel = false;
                    }
                }
            }
        }

        private void Pc_MouseDoubleClick(object sender, Drawing.MouseEventArgs e) {
            throw new NotImplementedException();
        }

        RotationMethods next(RotationMethods cur) {
            var vals = Enum.GetValues(typeof(RotationMethods)); 
            var curVal = Array.IndexOf(vals, cur);
            if (curVal >= 0) { 
                curVal = (curVal + 1) % vals.Length;
            }
            return (RotationMethods)curVal;
        }

    }
}
