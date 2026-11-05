//using System.Windows.Forms;
using ILNumerics.Drawing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;

namespace ILNumerics.Drawing.Tests
{
    /// <summary>
    /// Object TestAppController, controls switching between test scenes.
    /// </summary>
    public class ScenesCollection
    {
        static ScenesCollection s_instance = new ScenesCollection();
        public static ScenesCollection Instance {
            get {
                if (s_instance == null) {
                    s_instance = new ScenesCollection();
                }
                return s_instance;
            }
        }

        /// <summary>
        /// A public list if all tests defined for this assembly.
        /// </summary>
        private IDictionary<string, TestScene> testScenes;
        public IDictionary<string, TestScene> Scenes => testScenes;

        private string getAssemblyVersion(Scene scene)
        {
            FileVersionInfo myFileVersionInfo = FileVersionInfo.GetVersionInfo(scene.GetType().Module.Assembly.Location);
            return myFileVersionInfo.FileVersion;
        }

        /// <summary>
        /// Constructor, creates a list of all user defined tests in current assembly.
        /// </summary>
        public ScenesCollection()
        {

            var baseType = typeof(TestScene);

            testScenes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => {
                    try { return a.GetTypes(); } catch (ReflectionTypeLoadException ex) { return ex.Types.Where(t => t != null)!; } catch { return Array.Empty<Type>(); }
                })
                .Where(t => t != null && baseType.IsAssignableFrom(t!) && t!.IsClass && !t!.IsAbstract)
                .ToDictionary(
                    t => t!.Name,     // key: type name
                    t => Activator.CreateInstance(t) as TestScene ?? new ErrorTestScene(TestCategory.CT_CustomerTests, "Error", "This item serves as ERROR placeholder."))
                .OrderByDescending(t => t.Value.GetCategory(), StringComparer.OrdinalIgnoreCase)
                .ToDictionary(k => k.Key, v => v.Value); 
            
        }

        /// <summary>
        /// Method searches a scene by its type name and returns the scene object to assign to render target.
        /// </summary>
        /// <param name="testName">The scene name to search for.</param>
        /// <returns>Object Scene with user defined scene or null if scene was not found.</returns>
        public (Scene scene, string description) GetSceneByName(string testName, string driverType = "") {

            if (!testScenes.ContainsKey(testName)) {
                throw new ArgumentException($"The scene '{testName}' was not found.");
            }

            var instance = testScenes[testName]; 
            if (instance == null) {
                throw new InvalidOperationException($"Error handling scene type '{testName}': invalid type or cannot instantiate.");
            }
            var retScene = instance.GetScene();

            // add label for scene
            string a_Description = "";
            if (driverType.Length > 0)
                a_Description = driverType;

            instance.GetScene().Screen.Add(new Drawing.Label(a_Description, tag: "RenderType") {
                Font = new System.Drawing.Font(FontFamily.GenericSansSerif, 10),
                Position = new Vector3(0, 0, 0),
                Anchor = new PointF(0, 0)
            });

            // show the assembly version:
            retScene.Screen.Add(new Drawing.Label("Assembly Version: " + getAssemblyVersion(retScene), tag: "AssemblyVersion") {
                Position = new Vector3(0, 1, 0),
                Anchor = new PointF(0, 1)
            });

            return (retScene, instance.GetDescription());
        }
    }        
}
