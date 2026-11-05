using ILNumerics;
using ILNumerics.Drawing;


namespace ILNumerics.Drawing
{
    /// <summary>
    /// Enumeration of all possible implemented test categories.
    /// </summary>
    public enum TestCategory
    {
        // High Level Tests  HL_ prefix!
        HL_BarPlots,
        HL_LinePlotting,
        HL_Spheres,
        HL_SurfacePlots,
        HL_ErrorBarPlots, 
        HL_CandleStickPlots,
        HL_BoxPlotTest,
        HL_SplinePlotTest,
        HL_PlotCube,


        // Rasterizer Tests RA_ prefix!
        RA_BasicRendering,
        RA_AntialiasedLines,
        RA_AntialiasedTriangles,
        RA_AntialiasedPoints,
        RA_LinesVariedParameters,
        RA_PointsVariedParameters,
        RA_IntersectionLinesVariedParameters,

        // Scene Graph Tests SG_ prefix!
        SG_PickingTests,
        SG_TextRendering,

        // Customer issues
        CT_CustomerTests
    }


    /// <summary>
    /// Abstract class description for all graphical tests.
    /// </summary>
    public abstract class TestScene
    {
        private TestCategory TestCategory;
        private string TestName;
        private string TestDescription;

        /// <summary>
        /// Test scene constructor.
        /// </summary>
        public TestScene(TestCategory testCategory, string testName, string testDescription)
        {
            TestCategory = testCategory;
            TestName = testName;
            TestDescription = testDescription;
        }        
        
        /// <summary>
        /// Method returns user defined scene.
        /// </summary>
        /// <returns>Object Scene.</returns>
        abstract public Scene GetScene();

        /// <summary>
        /// Method returns user defined test name.
        /// </summary>
        /// <returns>String containing test name.</returns>
        //abstract public string GetName();
        public string GetName() { return TestName; }

        /// <summary>
        /// Method returns user defined test short description.
        /// </summary>
        /// <returns>String containing test description.</returns>
        public string GetDescription() { return TestDescription; }

        /// <summary>
        /// Method returns user defined test category.
        /// </summary>
        /// <returns>String containing test category.</returns>
        public string GetCategory() { return TestCategory.ToString(); }
    }

    public class ErrorTestScene : TestScene {
        public ErrorTestScene(TestCategory testCategory, string testName, string testDescription) : base(testCategory, testName, testDescription) {
        }
        public ErrorTestScene() : base(TestCategory.CT_CustomerTests, "error", "error test placeholder") { }
        public override Scene GetScene() {
            return new Scene() { Camera = { new Title("ERROR") } }; 
        }
    }
}
