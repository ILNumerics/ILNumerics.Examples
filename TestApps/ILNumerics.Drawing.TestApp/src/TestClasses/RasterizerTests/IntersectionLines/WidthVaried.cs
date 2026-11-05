using System.Drawing;
using ILNumerics;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;
using static ILNumerics.Globals;
using static ILNumerics.ILMath;

namespace ILNumerics.DrawingTestApplication.TestClasses
{
    class WidthVaried : TestScene
    {
        public WidthVaried() : base(
            TestCategory.RA_IntersectionLinesVariedParameters,
            "Intersection between horizontal Line Plot and vertical Line Plots: Varied width",
            @"[TestCase]:
This creates two horizontal line plots that intersect with a number of vertical line plots. The vertical line plots vary in width and should 
all display the same behavior in terms of their intersection with the horizontal line.")
        { }

        

        public override Scene GetScene()
        {
            var scene = new Scene();
            var plotCube = scene.Add(new PlotCube());

            Array<float> XHorizontalLine = new float[] { 1, 5 };
            Array<float> YHorizontalLine = ones<float>(2, 1);

            Array<float> XVerticalLines = array(new float[,] { { 2, 2 }, { 3, 3 }, { 4, 4 } }).T;
            Array<float> YVerticalLines = array(new float[,] { { 0, 2 }, { 0, 2 }, { 0, 2 } }).T;

            Array<int> LineWidth = new int[] { 1, 5, 10 };

            // Top
            for (int i = 0; i < XVerticalLines.S[1]; i++) {
                plotCube.Add(new LinePlot(XVerticalLines[":", i], YVerticalLines[":", i] + 4,
                    lineColor: Color.CornflowerBlue, lineWidth: (int)LineWidth[i]));

                plotCube.Add(new Drawing.Label("Width: " + ((int)LineWidth[i]).ToString()) {
                    Position = new Vector3((float)XVerticalLines[0, i], (float)YVerticalLines[1, i] + 4 + 0.125f, 0),
                    Font = new Font(FontFamily.GenericSansSerif, 8),
                    Color = Color.Black
                });
            }
            var horizontalLinePlot01 =  plotCube.Add(new LinePlot(XHorizontalLine, YHorizontalLine + 4, lineColor: Color.OrangeRed, lineWidth: 10));
            horizontalLinePlot01.Add(new Drawing.Label("In this test the horizontal line was drawn AFTER the vertical lines were drawn.\n" +
                "As a result, the horizontal line should lie above the vertical lines.\n" + 
                "Please zoom in and out and make sure this is always the case for all line widths.") 
            { Position = new Vector3(3, 7, 0), Font = new Font(FontFamily.GenericSansSerif, 8), Color = Color.Black });


            // Bottom
            var horizontalLinePlot02 = plotCube.Add(new LinePlot(XHorizontalLine, YHorizontalLine, lineColor: Color.Purple, lineWidth: 10));
            for (int i = 0; i < XVerticalLines.S[1]; i++) {                
                plotCube.Add(new LinePlot(XVerticalLines[":", i], YVerticalLines[":", i],
                    lineColor: Color.Green, lineWidth: (int)LineWidth[i]));

                plotCube.Add(new Drawing.Label("Width: " + ((int)LineWidth[i]).ToString()) {
                    Position = new Vector3((float)XVerticalLines[0, i], (float)YVerticalLines[1, i] + 0.125f, 0),
                    Font = new Font(FontFamily.GenericSansSerif, 8),
                    Color = Color.Black
                });
            }
            horizontalLinePlot02.Add(new Drawing.Label("In this test the horizontal line was drawn BEFORE the vertical lines were drawn.\n" +
                "As a result, the vertical lines should lie above the horizontal line.\n" +
                "Please zoom in and out and make sure this is always the case for all line widths.") 
            { Position = new Vector3(3, 3, 0), Font = new Font(FontFamily.GenericSansSerif, 8), Color = Color.Black });

            plotCube.Limits.Set(new Vector3(0, -1, -5), new Vector3(6, 8, 5));
            
            return scene;
        }
    }
}
