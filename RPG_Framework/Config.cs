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
        public static float DefResistanceXPModifier = 1.5f;
        public static int DefResistanceMaxLvl = 38;

        public float XP_Multiplier { get; set; } = 1f;
        public float OnKillcreatureKillXP_Modifier { get; set; } = 0.10f;

        public float SwimXP_Modifier { get; set; } = 2.23f;
        public float WalkXP_Modifier { get; set; } = 2.23f;
        public float HealthXP_Modifier { get; set; } = 1.25f;
        public float AirXP_Modifier { get; set; } = 1.2f;
        public float BreathPeriodXP_Modifier { get; set; } = 1.25f;
        public float FoodXP_Modifier { get; set; } = 1.2f;

        public int MaxSwimSpeedBoost { get; set; } = 20;
        public int MaxWalkSpeedBoost { get; set; } = 15;
        public int MaxHealthBoost { get; set; } = 150;
        public int MaxAirBoost { get; set; } = 100;
        public int MaxBreathPeriodBoost { get; set; } = 40;
        public int MaxFoodBoost { get; set; } = 250;

        public int MaxWalkSpeedBoost_InBase { get; set; } = 5;
        public int MaxWalkSpeedBoost_InSub { get; set; } = 5;
        public float PercentBreathPeriodPerLevel { get; } = 0.225f;



        //
        //Resistance modifiers
        //
        public float PercentResistancePerLevel { get; } = 2.5f;
        public int MaxResistanceLevel { get; set; } = 38;
        public float ResistanceXPModifier { get; set; } = 1.5f;

        public float SuffocateResistModifier { get; set; } = 1.35f;

        public float AcidResistModifier { get; set; } = DefResistanceXPModifier;
        public float ColdResistModifier { get; set; } = DefResistanceXPModifier;
        public float CollideResistModifier { get; set; } = DefResistanceXPModifier;
        public float DrillResistModifier { get; set; } = DefResistanceXPModifier;
        public float ElectricResistModifier { get; set; } = DefResistanceXPModifier;
        public float ExplosiveResistModifier { get; set; } = DefResistanceXPModifier;
        public float FireResistModifier { get; set; } = DefResistanceXPModifier;
        public float HeatResistModifier { get; set; } = DefResistanceXPModifier;
        public float LaserCutterResistModifier { get; set; } = DefResistanceXPModifier;
        public float NormalResistModifier { get; set; } = DefResistanceXPModifier;
        public float PoisonResistModifier { get; set; } = DefResistanceXPModifier;
        public float PressureResistModifier { get; set; } = DefResistanceXPModifier;
        public float PunctureResistModifier { get; set; } = DefResistanceXPModifier;
        public float RadResistModifier { get; set; } = DefResistanceXPModifier;
        public float SmokeResistModifier { get; set; } = DefResistanceXPModifier;
        public float StarveResistModifier { get; set; } = DefResistanceXPModifier;
        public float UndefinedResistModifier { get; set; } = DefResistanceXPModifier;



        //
        //max resistance levels
        //
        public int MaxSuffocateResistLevel { get; set; } = 17;

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
                DefResistanceMaxLvl = Cfg.MaxResistanceLevel;
                DefResistanceXPModifier = Cfg.ResistanceXPModifier;
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
