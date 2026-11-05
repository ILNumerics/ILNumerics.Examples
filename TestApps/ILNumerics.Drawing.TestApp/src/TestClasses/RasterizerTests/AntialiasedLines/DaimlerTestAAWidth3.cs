using System.Drawing;
using ILNumerics;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;
using ILNumerics.Toolboxes;
using static ILNumerics.Globals;
using static ILNumerics.ILMath;

namespace ILNumerics.DrawingTestApplication.TestClasses.AntialiasingTests.Lines
{
    class DaimlerTestAAWidth3 : TestScene
    {
        public DaimlerTestAAWidth3() : base(
            TestCategory.CT_CustomerTests,
            "Daimler: AA Width 2 Line Plot defined with large set of points",
            @"Issue from Daimler.
Black Plot is defined with 2000 points. Antialiased, 2px Width, Dashed Pattern.

Red Plot is a DOWNSAMPLED version of Black Plot defined with 80 points. Antialiased, 2px Width, Dashed Pattern.

Red and Black Plot should be drawn without any artefacts using both renders GDI and OpenGL")
        { }

        public override Scene GetScene()
        {
            var scene = new Scene();

            Array<double> X = linspace(-pi, pi, 2000);
            Array<double> V = sin(X * X);
            Array<double> X2 = linspace(-pi, pi, 80);
            Array<double> V2 = sin(X2 * X2);

            scene.Add(new PlotCube(twoDMode: false)
            {
                new LinePlot(X: X, Y: V, lineStyle: DashStyle.Dashed)
                {
                    Line = { Antialiasing = true, Color = Color.Black, Width = 2 }
                },

                new LinePlot(X: X2, Y:V2+1, lineStyle: DashStyle.Dashed)
                {
                    Line = { Antialiasing = true, Color = Color.Red, Width = 2 }
                },


            });

            return scene;
        }
    }
}
