using Harmony;
using SMLHelper;
using Oculus.Newtonsoft.Json;
using Oculus.Platform.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SMLHelper.V2.Utility;

namespace RPG_Framework
{
    
    class SaveData
    {
        private static SaveData saveData;
        public static string saveFileName = "RPGSaveData.json";
        //public static string SaveDataPath = Environment.CurrentDirectory + "\\QMods\\RPG_Framework\\SaveData\\SaveData.json";
        public static string TempSaveDataPath = Path.Combine(SaveLoadManager.GetTemporarySavePath(), saveFileName);

        private static string LastSavePath = "";
        
        //public static string SaveDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LocalLow\\Unknown Worlds\\Subnautica\\Subnautica\\SavedGames\\" + SaveSlot, "SaveData.json");
        public static float resistBaseXP = 125f;//100f;

        public int PlayerLevel { get; set; } = 0;
        public float PlayerXP { get; set; } = 0f;
        public float Player_XPToNextLevel { get; set; } = 0f;


        #region Attack stuff

        #endregion

        #region Swim speed Stuff
        //Main Swim Speed stuff
        public float SwimDistanceTravelled { get; set; } = 0f;
        public int SwimSpeedLevel { get; set; } = 0;
        public float SwimSpeed_XP { get; set; } = 0f;
        public float SwimSpeed_XPToNextLevel { get; set; } = 6500f;
        #endregion

        #region Land speed stuff
        //Main Land Speed stuff
        public float WalkDistanceTravelled { get; set; } = 0f;
        public int WalkSpeedLevel { get; set; } = 0;
        public float WalkSpeed_XP { get; set; }
        public float WalkSpeed_XPToNextLevel { get; set; } = 6500f;
        #endregion


        #region Health stuff
        public int HealthBonusLevel { get; set; }
        public float Health_XP { get; set; }
        public float Health_XPToNextLevel { get; set; } = 1250f;
        #endregion


        #region Breath Period stuff
        public int BreathPeriodLevel { get; set; }
        public float BreathPeriod_XP { get; set; }
        public float BreathPeriod_XPToNextLevel { get; set; } = 1250f;
        #endregion


        #region Food Stuff
        public int FoodBonusLevel { get; set; }
        public float Food_XP { get; set; }
        public float Food_XPToNextLevel { get; set; } = 1500f;
        #endregion


        #region Damage Resistance

        //Suffocation isnt technically damage type but its going in here anyways
        public int SuffocateResistLevel { get; set; }
        public float SuffocateResist_XP { get; set; }
        public float SuffocateResist_XPToNextLevel { get; set; } = 30;


        public int AcidResistLevel { get; set; }
        public float AcidResist_XP { get; set; }
        public float AcidResist_XPToNextLevel { get; set; } = resistBaseXP;


        public int ColdResistLevel { get; set; }
        public float ColdResist_XP { get; set; }
        public float ColdResist_XPToNextLevel { get; set; } = resistBaseXP;


        public int CollideResistLevel { get; set; }
        public float CollideResist_XP { get; set; }
        public float CollideResist_XPToNextLevel { get; set; } = resistBaseXP;


        public int DrillResistLevel { get; set; }
        public float DrillResist_XP { get; set; }
        public float DrillResist_XPToNextLevel { get; set; } = resistBaseXP;


        public int ElectricResistLevel { get; set; }
        public float ElectricResist_XP { get; set; }
        public float ElectricResist_XPToNextLevel { get; set; } = resistBaseXP;


        public int ExplosiveResistLevel { get; set; }
        public float ExplosiveResist_XP { get; set; }
        public float ExplosiveResist_XPToNextLevel { get; set; } = resistBaseXP;


        public int FireResistLevel { get; set; }
        public float FireResist_XP { get; set; }
        public float FireResist_XPToNextLevel { get; set; } = resistBaseXP;


        public int HeatResistLevel { get; set; }
        public float HeatResist_XP { get; set; }
        public float HeatResist_XPToNextLevel { get; set; } = resistBaseXP;


        public int LaserCutterResistLevel { get; set; }
        public float LaserCutterResist_XP { get; set; }
        public float LaserCutterResist_XPToNextLevel { get; set; } = resistBaseXP;


        public int NormalResistLevel { get; set; }
        public float NormalResist_XP { get; set; }
        public float NormalResist_XPToNextLevel { get; set; } = resistBaseXP;


        public int PoisonResistLevel { get; set; }
        public float PoisonResist_XP { get; set; }
        public float PoisonResist_XPToNextLevel { get; set; } = resistBaseXP;


        public int PressureResistLevel { get; set; }
        public float PressureResist_XP { get; set; }
        public float PressureResist_XPToNextLevel { get; set; } = resistBaseXP;


        public int PunctureResistLevel { get; set; }
        public float PunctureResist_XP { get; set; }
        public float PunctureResist_XPToNextLevel { get; set; } = resistBaseXP;


        public int RadResistLevel { get; set; }
        public float RadResist_XP { get; set; }
        public float RadResist_XPToNextLevel { get; set; } = resistBaseXP;


        public int SmokeResistLevel { get; set; }
        public float SmokeResist_XP { get; set; }
        public float SmokeResist_XPToNextLevel { get; set; } = resistBaseXP;


        public int StarveResistLevel { get; set; }
        public float StarveResist_XP { get; set; }
        public float StarveResist_XPToNextLevel { get; set; } = resistBaseXP;


        public int UndefinedResistLevel { get; set; }
        public float UndefinedResist_XP { get; set; }
        public float UndefinedResist_XPToNextLevel { get; set; } = resistBaseXP;
        #endregion


        #region Air stuff
        public int AirBonusLevel { get; set; }
        public float Air_XP { get; set; }
        public float Air_XPToNextLevel { get; set; } = 1500f;
        #endregion

        public static SaveData GetSaveData() => GetSaveData(false);
        public static SaveData GetSaveData(bool reloadSave)
        {
            if (reloadSave || saveData == null)
                saveData = LoadSave();

            return saveData;
        }

        public static SaveData LoadSave()
        {
            string SaveDataPath = "";
            if (!Guard.IsStringValid(LastSavePath))
            {
                SaveDataPath = Path.Combine((Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).Replace("\\Roaming", "")
                + "\\LocalLow\\Unknown Worlds\\Subnautica\\Subnautica\\SavedGames\\" + SaveLoadManager.main.GetCurrentSlot(), saveFileName);
                
                LastSavePath = SaveDataPath;
            }
            else
                SaveDataPath = LastSavePath;

            Log.Output("Loading SaveData...");

            if (!File.Exists(SaveDataPath) || File.ReadAllText(SaveDataPath).Length == 0)
            {
                Log.Output("SaveData file doesn't exist or it is empty. Creating a new one");
                saveData = new SaveData();
                Save_SaveFile();
                return saveData;
            }

            try
            {
                saveData = JsonConvert.DeserializeObject<SaveData>(File.ReadAllText(SaveDataPath));
                //Save_SaveFile();    //Saving here to add new properties to json file
                Log.Output("Successfully loaded SaveData");
                return saveData;
            }
            catch
            {
                Log.Output("SaveData has invalid JSON. Creating a new one");
                saveData = new SaveData();
                Save_SaveFile();
                return saveData;
            }
        }

        public static void Save_SaveFile() => Save_SaveFile(saveData);

        public static void Save_SaveFile(SaveData save)
        {
            if (save == null) save = new SaveData();

            FileInfo savePath = new FileInfo(TempSaveDataPath);
            /*if (!Directory.Exists(savePath.FullName.Replace("\\" + savePath.Name, "")))
                Directory.CreateDirectory(savePath.FullName.Replace("\\" + savePath.Name, ""));*/

            string output_Cfg = JsonConvert.SerializeObject(save, Formatting.Indented);

            StreamWriter serialize = new StreamWriter(TempSaveDataPath, false);
            serialize.Write(output_Cfg);
            serialize.Close();
        }
    }

    [HarmonyPatch(typeof(SaveLoadManager))]
    [HarmonyPatch("StartNewSession")]
    public class LoadSaveData
    {
        [HarmonyPostfix]
        public static void Postfix()
        {
            Log.InGameMSG("Loading SaveData from file");
            SaveData.GetSaveData();

            //Set movement stuff
            Player.main.playerController.SetMotorMode(Player.main.motorMode);
        }
    }
}
