using Harmony;
using Oculus.Newtonsoft.Json;
using RPG_Framework.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace RPG_Framework
{
    class Config
    {
        private static Config Cfg;
        public static string ConfigPath = Environment.CurrentDirectory + "\\QMods\\RPG_Framework\\Config.json";
        public static float DefResistanceXPModifier = 1.2f;
        public static int DefResistanceMaxLvl = 38;

        public KeyCode SpeedBoostToggle { get; set; } = KeyCode.CapsLock;

        public float XP_Multiplier { get; set; } = 1f;
        public float OnKillcreatureKillXP_Modifier { get; set; } = 0.25f;

        public float Swim_XPNextLevel_Multiplier { get; set; } = 2.3f;
        public float Walk_XPNextLevel_Multiplier { get; set; } = 2.3f;
        public float Health_XPNextLevel_Multiplier { get; set; } = 1.3f;
        //public float AirXP_Modifier { get; set; } = 1.3f;
        public float BreathPeriod_XPNextLevel_Multiplier { get; set; } = 1.35f;
        //public float FoodXP_Modifier { get; set; } = 1.2f;

        public int MaxSwimSpeedLevel { get; set; } = 25;
        public int MaxWalkSpeedLevel { get; set; } = 25;
        public int MaxHealthLevel { get; set; } = 150;
        //public int MaxAirBoost { get; set; } = 100;
        public int MaxBreathPeriodLevel { get; set; } = 40;
        //public int MaxFoodBoost { get; set; } = 250;

        /*public int MaxWalkSpeedBoost_InBase { get; set; } = 5;
        public int MaxWalkSpeedBoost_InSub { get; set; } = 5;*/
        public float PercentBreathPeriodPerLevel { get; } = 0.225f;



        //
        //Resistance modifiers
        //
        public float PercentResistancePerLevel { get; } = 2.5f;
        public int MaxResistanceLevel { get; set; } = 38;
        public float Resistance_XPNextLevel_Multiplier { get; set; } = 1.5f;

        public float SuffocateResist_XPNextLevel_Multiplier { get; set; } = 2.3f;

        public float AcidResist_XPNextLevel_Multiplier { get; set; } = DefResistanceXPModifier;
        public float ColdResist_XPNextLevel_Multiplier { get; set; } = DefResistanceXPModifier;
        public float CollideResist_XPNextLevel_Multiplier { get; set; } = DefResistanceXPModifier;
        public float DrillResist_XPNextLevel_Multiplier { get; set; } = DefResistanceXPModifier;
        public float ElectricResist_XPNextLevel_Multiplier { get; set; } = DefResistanceXPModifier;
        public float ExplosiveResist_XPNextLevel_Multiplier { get; set; } = DefResistanceXPModifier;
        public float FireResist_XPNextLevel_Multiplier { get; set; } = DefResistanceXPModifier;
        public float HeatResist_XPNextLevel_Multiplier { get; set; } = DefResistanceXPModifier;
        public float LaserCutterResist_XPNextLevel_Multiplier { get; set; } = DefResistanceXPModifier;
        public float NormalResist_XPNextLevel_Multiplier { get; set; } = DefResistanceXPModifier;
        public float PoisonResist_XPNextLevel_Multiplier { get; set; } = DefResistanceXPModifier;
        public float PressureResist_XPNextLevel_Multiplier { get; set; } = DefResistanceXPModifier;
        public float PunctureResist_XPNextLevel_Multiplier { get; set; } = DefResistanceXPModifier;
        public float RadResist_XPNextLevel_Multiplier { get; set; } = DefResistanceXPModifier;
        public float SmokeResist_XPNextLevel_Multiplier { get; set; } = DefResistanceXPModifier;
        public float StarveResist_XPNextLevel_Multiplier { get; set; } = DefResistanceXPModifier;
        public float UndefinedResist_XPNextLevel_Multiplier { get; set; } = DefResistanceXPModifier;



        //
        //max resistance levels
        //
        public int MaxSuffocateResistLevel { get; set; } = 10;

        public int MaxAcidResistLevel { get; set; } = DefResistanceMaxLvl;
        public int MaxColdResistLevel { get; set; } = DefResistanceMaxLvl;
        public int MaxCollideResistLevel { get; set; } = DefResistanceMaxLvl;
        public int MaxDrillResistLevel { get; set; } = DefResistanceMaxLvl;
        public int MaxElectricResistLevel { get; set; } = DefResistanceMaxLvl;
        public int MaxExplosiveResistLevel { get; set; } = DefResistanceMaxLvl;
        public int MaxFireResistLevel { get; set; } = DefResistanceMaxLvl;
        public int MaxHeatResistLevel { get; set; } = DefResistanceMaxLvl;
        public int MaxLaserCutterResistLevel { get; set; } = DefResistanceMaxLvl;
        public int MaxNormalResistLevel { get; set; } = DefResistanceMaxLvl;
        public int MaxPoisonResistLevel { get; set; } = DefResistanceMaxLvl;
        public int MaxPressureResistLevel { get; set; } = DefResistanceMaxLvl;
        public int MaxPunctureResistLevel { get; set; } = DefResistanceMaxLvl;
        public int MaxRadResistLevel { get; set; } = DefResistanceMaxLvl;
        public int MaxSmokeResistLevel { get; set; } = DefResistanceMaxLvl;
        public int MaxStarveResistLevel { get; set; } = DefResistanceMaxLvl;
        public int MaxUndefinedResistLevel { get; set; } = DefResistanceMaxLvl;

        /// <summary>
        /// Use this to read and load Config
        /// </summary>
        /// <returns></returns>
        public static Config GetConfig()
        {
            if (Cfg == null) Cfg = LoadConfig();
            SaveConfig();   //Saving so new properties get added to file
            return Cfg;
        }

        /// <summary>
        /// Load config from file. Use GetConfig() instead unless you want to read a new config from file
        /// </summary>
        /// <returns></returns>
        public static Config LoadConfig()
        {
            Config cfg;
            Log.Output("Loading Config...");
            
            if (!File.Exists(ConfigPath) || File.ReadAllText(ConfigPath).Length == 0)
            {
                Log.Output("Config file doesn't exist or it is empty. Creating a new one");
                cfg = new Config();
                return cfg;
            }

            try
            {
                if(Cfg == null) Log.Output("Initializing RPG Framework");
                cfg = JsonConvert.DeserializeObject<Config>(File.ReadAllText(ConfigPath));

                DefResistanceMaxLvl = cfg.MaxResistanceLevel;
                DefResistanceXPModifier = cfg.Resistance_XPNextLevel_Multiplier;
                
                try { var testKey = cfg.SpeedBoostToggle; }
                catch { cfg.SpeedBoostToggle = KeyCode.CapsLock; }
                
                Log.Output("Successfully loaded Config");
                return cfg;
            }
            catch
            {
                Log.Output("Config has invalid JSON. Creating a new one");
                cfg = new Config();
                return cfg;
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

    [HarmonyPatch(typeof(GameSettings))]
    [HarmonyPatch("SaveAsync")]
    public class SaveConfig_Patch
    {
        [HarmonyPostfix]
        public static void Postfix()
        {
            Log.Output("Saving RPG Config");
            Config.SaveConfig();
            Log.Output("RPG Config saved");
        }
    }
}
