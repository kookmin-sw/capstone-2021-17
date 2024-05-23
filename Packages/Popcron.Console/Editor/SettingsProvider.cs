using UnityEditor;
using UnityEngine.UIElements;

namespace Popcron.Console
{
    public class ConsoleSettingsProvider : SettingsProvider
    {
        private SerializedObject settings;

        public ConsoleSettingsProvider(string path, SettingsScope scope = SettingsScope.Project) : base(path, scope)
        {

        }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            settings = new SerializedObject(Settings.Current);
        }

        public override void OnGUI(string searchContext)
        {
            SettingsInspector.Show(settings);
        }

        [SettingsProvider]
        public static SettingsProvider CreateMyCustomSettingsProvider()
        {
            ConsoleSettingsProvider provider = new ConsoleSettingsProvider("Project/Console", SettingsScope.Project)
            {
                keywords = new string[] { "console" }
            };

            return provider;
        }
    }
}