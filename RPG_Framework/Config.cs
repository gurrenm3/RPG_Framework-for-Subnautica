using Oculus.Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RPG_Framework
{
    class Config
    {
        private static Config Cfg;
        public static string ConfigPath = Environment.CurrentDirectory + "\\QMods\\RPG_Framework\\Config.json";

        public float XP_Multiplier { get; set; } = 1f;
        public float OnKillcreatureKillXP_Modifier { get; set; } = 0.10f;

        public float SwimXP_Modifier { get; set; } = 1.23f;
        public float WalkXP_Modifier { get; set; } = 1.23f;
        public float HealthXP_Modifier { get; set; } = 1.23f;

        public int MaxSwimSpeedBoost { get; set; } = 15;
        public int MaxWalkSpeedBoost { get; set; } = 10;
        public int MaxHealthBoost { get; set; } = 100;

        public int MaxWalkSpeedBoost_InBase { get; set; } = 5;
        public int MaxWalkSpeedBoost_InSub { get; set; } = 5;

        public static Config GetConfig()
        {
            if (Cfg == null) Cfg = LoadConfig();
            return Cfg;
        }

        public static Config LoadConfig()
        {
            Log.Output("Loading Config...");
            if (!File.Exists(ConfigPath) || File.ReadAllText(ConfigPath).Length == 0)
            {
                Log.Output("Config file doesn't exist or it is empty. Creating a new one");
                Cfg = new Config();
                SaveConfig();
                return Cfg;
            }

            try
            {
                Cfg = JsonConvert.DeserializeObject<Config>(File.ReadAllText(ConfigPath));
                SaveConfig();   //Saving so new properties get added to file
                Log.Output("Successfully loaded Config");
                return Cfg;
            }
            catch
            {
                Log.Output("Config has invalid JSON. Creating a new one");
                Cfg = new Config();
                SaveConfig();
                return Cfg;
            }
        }

        public static void SaveConfig() => SaveConfig(Cfg);

        public static void SaveConfig(Config config)
        {
            if (config == null) config = new Config();

            FileInfo configPath = new FileInfo(ConfigPath);
            if (!Directory.Exists(configPath.FullName.Replace("\\" + configPath.Name, "")))
                Directory.CreateDirectory(configPath.FullName.Replace("\\" + configPath.Name, ""));

            string output_Cfg = JsonConvert.SerializeObject(config, Formatting.Indented);

            StreamWriter serialize = new StreamWriter(ConfigPath, false);
            serialize.Write(output_Cfg);
            serialize.Close();
        }
    }
}
