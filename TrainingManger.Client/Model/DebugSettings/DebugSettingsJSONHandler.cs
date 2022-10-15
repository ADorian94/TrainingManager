using Newtonsoft.Json;

namespace TrainingManager.Model.DebugSettings
{
    public class DebugSettingsJSON
    {
        public string APIConnection { get; set; }
    }

    public class DebugSettingsJSONHandler
    {
        private const string DEBUG_FOLDER = "Debug";
        private const string JSON_FILE = "DebugSettings.json";
        private static Lazy<DebugSettingsJSONHandler> Lazy => new Lazy<DebugSettingsJSONHandler>(() => new DebugSettingsJSONHandler(), true);
        public static DebugSettingsJSONHandler Instance => Lazy.Value;

        public static string ExternalStorage { get; private set; }
        public DebugSettingsJSON JSONFile { get; set; }

        private DebugSettingsJSONHandler()
        {
            JSONFile = new DebugSettingsJSON();
            string jsonFile = Path.Combine(ExternalStorage, DEBUG_FOLDER, JSON_FILE);

            if (File.Exists(jsonFile))
                JSONFile = JsonConvert.DeserializeObject<DebugSettingsJSON>(File.ReadAllText(jsonFile));
        }


        //PUBLIC
        public static void InitializeJSONPath(string path) => ExternalStorage = path;
    }
}
