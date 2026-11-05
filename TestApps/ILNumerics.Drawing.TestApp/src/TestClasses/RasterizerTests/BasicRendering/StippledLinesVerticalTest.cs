using ILNumerics;
using ILNumerics.Drawing;
using ILNumerics.Drawing.Plotting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ILNumerics.Globals;
using static ILNumerics.ILMath;

namespace ILNumerics.DrawingTestApplication.TestClasses.BasicRendering
{
    class StippledLinesVerticalTest : TestScene
    {
        public StippledLinesVerticalTest() : base(
            TestCategory.RA_BasicRendering,
            "StippledLinesVerticalTest",
            @"Vertical stippled lines must be rendered correctly and robust.
All dash patterns are used on vertical lines. The patterns must be complete and correct.
Move the shapes with the mouse. The patterns must not change.
Resize the panel / form. The patterns must not rescale but stay in the scale of pixels.")
        { }

        public override Scene GetScene()
        {
            var scene = new Scene();

            Array<float> A = array(new float[,] {
                { 0, -.5f, 0}, { 0, .5f, 0},
                { 1, -.5f, 0}, { 1, .5f, 0},
                { 2, -.5f, 0}, { 2, .5f, 0},
                { 3, -.5f, 0}, { 3, .5f, 0},
                { 4, -.5f, 0}, { 4, .5f, 0},
            }).T;

            scene.Camera.Add(new Group(scale: new Vector3(.1, 1, 1), translate: new Vector3(-.05, 0, 0)) {
                new Lines() {
                    Positions = A,
                    Color = Color.Black,
                    Indices = new[] {0,1},
                    Width = 1,
                    Antialiasing = false,
                    DashStyle = DashStyle.Dashed,
                },
                new Lines() {
                    Positions = A,
                    Color = Color.Black,
                    Indices = new[] {2,3},
                    Width = 1,
                    Antialiasing = false,
                    DashStyle = DashStyle.Dotted,
                },
                new Lines() {
                    Positions = A,
                    Color = Color.Black,
                    Indices = new[] {4,5},
                    Width = 1,
                    Antialiasing = false,
                    DashStyle = DashStyle.PointDash,
                },
                new Lines() {
                    Positions = A,
                    Color = Color.Black,
                    Indices = new[] {6,7},
                    Width = 1,
                    Antialiasing = false,
                    DashStyle = DashStyle.Solid,
                },
                new Lines() {
                    Positions = A,
                    Color = Color.Black,
                    Indices = new[] {8,9},
                    Width = 1,
                    Antialiasing = false,
                    DashStyle = DashStyle.UserPattern,
                    Pattern = (short)0xf2
                },
            });
            return scene;
        }

    }
}
