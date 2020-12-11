using HarmonyLib;
using Oculus.Newtonsoft.Json;
using System;
using System.IO;
using RPG_Framework.Lib.Web;

namespace RPG_Framework
{

    class SaveData
    {
        private static SaveData saveData;
        public static string saveFileName = "RPGSaveData.json";
        public static string saveSlot = "";
        //public static string SaveDataPath = Environment.CurrentDirectory + "\\QMods\\RPG_Framework\\SaveData\\SaveData.json";
        public static string TempSaveDataPath;

        //public static string SaveDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LocalLow\\Unknown Worlds\\Subnautica\\Subnautica\\SavedGames\\" + SaveSlot, "SaveData.json");
        public static float resistBaseXP = 125f;//100f;

        //public int PlayerLevel { get; set; } = 0;
        public float PlayerXP { get; set; } = 0f;
        //public float Player_XPToNextLevel { get; set; } = 0f;


        #region Attack stuff

        #endregion

        #region Swim speed Stuff
        //Main Swim Speed stuff
        //public float SwimDistanceTravelled { get; set; } = 0f;
        public int SwimSpeedLevel { get; set; } = 0;
        public float SwimSpeed_XP { get; set; } = 0f;
        public float SwimSpeed_XPToNextLevel { get; set; } = 6500f;
        #endregion

        #region Land speed stuff
        //Main Land Speed stuff
        //public float WalkDistanceTravelled { get; set; } = 0f;
        public int WalkSpeedLevel { get; set; } = 0;
        public float WalkSpeed_XP { get; set; } = 0;
        public float WalkSpeed_XPToNextLevel { get; set; } = 6500f;
        #endregion


        #region Health stuff
        public int HealthLevel { get; set; } = 0;
        public float Health_XP { get; set; } = 0;
        public float Health_XPToNextLevel { get; set; } = 15;
        #endregion


        #region Breath Period stuff
        public int BreathPeriodLevel { get; set; }
        public float BreathPeriod_XP { get; set; }
        public float BreathPeriod_XPToNextLevel { get; set; } = 325f;//1250f;
        #endregion


        /*#region Food Stuff
        public int FoodBonusLevel { get; set; }
        public float Food_XP { get; set; }
        public float Food_XPToNextLevel { get; set; } = 1500f;
        #endregion*/


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

        

        public static SaveData GetSaveData() => GetSaveData(false);
        public static SaveData GetSaveData(bool reloadSave)
        {
            if (reloadSave || saveData == null)
            {
                saveData = LoadSave();
                UpdateHandler updateHandler = new UpdateHandler();
                updateHandler.HandleUpdates();
            }

            saveData.CheckForNegatives();
            return saveData;
        }


        public static SaveData LoadSave()
        {
            string SaveDataPath = Path.Combine(SaveLoadManager.GetTemporarySavePath(), saveFileName);
            Logger.Log("Loading SaveData...", Lib.LogType.LogFile);
            Logger.Log("SaveData path:  "+SaveDataPath, Lib.LogType.LogFile);

            if (!File.Exists(SaveDataPath) || File.ReadAllText(SaveDataPath).Length == 0)
            {
                Logger.Log("SaveData file doesn't exist or it is empty. Creating a new one", Lib.LogType.LogFile);
                saveData = new SaveData();
                Save_SaveFile();
                return saveData;
            }

            try
            {
                saveData = JsonConvert.DeserializeObject<SaveData>(File.ReadAllText(SaveDataPath));
                //Save_SaveFile();    //Saving here to add new properties to json file
                Logger.Log("Successfully loaded SaveData", Lib.LogType.LogFile);
                return saveData;
            }
            catch
            {
                Logger.Log("SaveData has invalid JSON. Creating a new one", Lib.LogType.LogFile);
                saveData = new SaveData();
                Save_SaveFile();
                return saveData;
            }
        }

        public static void Save_SaveFile() => Save_SaveFile(saveData);

        public static void Save_SaveFile(SaveData save)
        {
            if (save == null) save = new SaveData();
            save.CheckForNegatives();

            TempSaveDataPath = Path.Combine(SaveLoadManager.GetTemporarySavePath(), saveFileName);
            FileInfo savePath = new FileInfo(TempSaveDataPath);

            string output_Cfg = JsonConvert.SerializeObject(save, Formatting.Indented);

            StreamWriter serialize = new StreamWriter(TempSaveDataPath, false);
            serialize.Write(output_Cfg);
            serialize.Close();
        }

        bool savedNegativeCheck = false;
        private void CheckForNegatives()
        {
            saveData.SwimSpeed_XP = FixNegatives(saveData.SwimSpeed_XP, saveData.SwimSpeed_XPToNextLevel);
            saveData.WalkSpeed_XP = FixNegatives(saveData.WalkSpeed_XP, saveData.WalkSpeed_XPToNextLevel);
            saveData.Health_XP = FixNegatives(saveData.Health_XP, saveData.Health_XPToNextLevel);
            saveData.BreathPeriod_XP = FixNegatives(saveData.BreathPeriod_XP, saveData.BreathPeriod_XPToNextLevel);
            saveData.SuffocateResist_XP = FixNegatives(saveData.SuffocateResist_XP, saveData.SuffocateResist_XPToNextLevel);
            saveData.AcidResist_XP = FixNegatives(saveData.AcidResist_XP, saveData.AcidResist_XPToNextLevel);
            saveData.ColdResist_XP = FixNegatives(saveData.ColdResist_XP, saveData.ColdResist_XPToNextLevel);
            saveData.CollideResist_XP = FixNegatives(saveData.CollideResist_XP, saveData.CollideResist_XPToNextLevel);
            saveData.DrillResist_XP = FixNegatives(saveData.DrillResist_XP, saveData.DrillResist_XPToNextLevel);
            saveData.ElectricResist_XP = FixNegatives(saveData.ElectricResist_XP, saveData.ElectricResist_XPToNextLevel);
            saveData.ExplosiveResist_XP = FixNegatives(saveData.ExplosiveResist_XP, saveData.ExplosiveResist_XPToNextLevel);
            saveData.FireResist_XP = FixNegatives(saveData.FireResist_XP, saveData.FireResist_XPToNextLevel);
            saveData.HeatResist_XP = FixNegatives(saveData.HeatResist_XP, saveData.HeatResist_XPToNextLevel);
            saveData.LaserCutterResist_XP = FixNegatives(saveData.LaserCutterResist_XP, saveData.LaserCutterResist_XPToNextLevel);
            saveData.NormalResist_XP = FixNegatives(saveData.NormalResist_XP, saveData.NormalResist_XPToNextLevel);
            saveData.PoisonResist_XP = FixNegatives(saveData.PoisonResist_XP, saveData.PoisonResist_XPToNextLevel);
            saveData.PressureResist_XP = FixNegatives(saveData.PressureResist_XP, saveData.PressureResist_XPToNextLevel);
            saveData.PunctureResist_XP = FixNegatives(saveData.PunctureResist_XP, saveData.PunctureResist_XPToNextLevel);
            saveData.RadResist_XP = FixNegatives(saveData.RadResist_XP, saveData.RadResist_XPToNextLevel);
            saveData.SmokeResist_XP = FixNegatives(saveData.SmokeResist_XP, saveData.SmokeResist_XPToNextLevel);
            saveData.StarveResist_XP = FixNegatives(saveData.StarveResist_XP, saveData.StarveResist_XPToNextLevel);
            saveData.UndefinedResist_XP = FixNegatives(saveData.UndefinedResist_XP, saveData.UndefinedResist_XPToNextLevel);

            if (!savedNegativeCheck)
            {
                savedNegativeCheck = true;
                Save_SaveFile();
            }
        }

        private float FixNegatives(float xp, float xpNextLevel)
        {
            if (xp > 0)
                return xp;
            return xpNextLevel / 2;
        }
    }


    [HarmonyPatch(typeof(SaveLoadManager))]
    [HarmonyPatch("InitializeNewGame")]
    public class LoadSaveData
    {
        [HarmonyPostfix]
        public static void Postfix()
        {
            SaveData.GetSaveData(true);
            
        }
    }


    [HarmonyPatch(typeof(SaveLoadManager))]
    [HarmonyPatch("SaveToDeepStorageAsync")]
    [HarmonyPatch(new Type[] { typeof(IOut<SaveLoadManager.SaveResult>) })]
    public class SavePatch
    {
        [HarmonyPrefix]
        public static bool Prefix()
        {
            SaveData.Save_SaveFile();
            Logger.Log("RPG data saved");

            return true;
        }
    }
}
