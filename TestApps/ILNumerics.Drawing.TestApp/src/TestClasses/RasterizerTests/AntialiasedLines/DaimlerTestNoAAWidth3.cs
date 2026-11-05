using System.Drawing;
using ILNumerics;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;
using ILNumerics.Toolboxes;
using static ILNumerics.Globals;
using static ILNumerics.ILMath;

namespace ILNumerics.DrawingTestApplication.TestClasses.AntialiasingTests.Lines
{
    class DaimlerTestNoAAWidth3 : TestScene
    {
        public DaimlerTestNoAAWidth3() : base(
            TestCategory.CT_CustomerTests,
            "Daimler: NO-AA Width 3 Line Plot defined with large set of points",
            @"Issue from Daimler.
Black Plot is defined with 2000 points. NO Antialiased, 3px Width, Dashed Pattern.

Red Plot is a DOWNSAMPLED version of Black Plot defined with 80 points. NO Antialiased, 3px Width, Dashed Pattern.

Red and Black Plot should be drawn without any artefacts using both renders GDI and OpenGL")
        { }

        public override Scene GetScene()
        {
            var scene = new Scene();

            Array<float> linearray1 = linspace<float>(0, 10, 2000);            
            Array<float> downSampled = Interpolation.interp1(linearray1, Xn: linspace<float>(0, linearray1.Length - 1, 2), method: InterpolationMethod.spline);

            Array<double> X = linspace(-pi, pi, 2000);
            Array<double> V = sin(X * X);

            int downSampleFactor = 80;
            Array<double> downSampledCurve = Interpolation.interp1(V, Xn: linspace<double>(0, V.Length - 1, downSampleFactor), method: InterpolationMethod.spline);

            scene.Add(new PlotCube(twoDMode: false)
            {               
                new LinePlot(X: X, Y: V, lineStyle: DashStyle.Dashed)
                {
                    Line = { Antialiasing = false, Color = Color.Black, Width = 3 }
                },

                new LinePlot(X: linspace(-pi, pi, downSampleFactor), Y:downSampledCurve+1, lineStyle: DashStyle.Dashed)
                {
                    Line = { Antialiasing = false, Color = Color.Red, Width = 3 }
                },


            });

            return scene;
        }
    }
}
