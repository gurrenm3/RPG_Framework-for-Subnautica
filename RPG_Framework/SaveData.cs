using Harmony;
using Oculus.Newtonsoft.Json;
using Oculus.Platform.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RPG_Framework
{
    
    class SaveData
    {
        private static SaveData saveData;
        public static string SaveDataPath = Environment.CurrentDirectory + "\\QMods\\RPG_Framework\\SaveData\\SaveData.json";

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


        public static SaveData GetSaveData()
        {
            if (saveData == null)
                saveData = LoadSave();

            return saveData;
        }

        public static SaveData LoadSave()
        {
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
                Save_SaveFile();    //Saving here to add new properties to json file
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

            FileInfo savePath = new FileInfo(SaveDataPath);
            if (!Directory.Exists(savePath.FullName.Replace("\\" + savePath.Name, "")))
                Directory.CreateDirectory(savePath.FullName.Replace("\\" + savePath.Name, ""));

            string output_Cfg = JsonConvert.SerializeObject(save, Formatting.Indented);

            StreamWriter serialize = new StreamWriter(SaveDataPath, false);
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
