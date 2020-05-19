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

        public int PlayerLevel { get; set; }
        public float PlayerXP { get; set; }

        //Swim Speed stuff
        public float SwimSpeed_PassiveIncrease { get; set; } = 0f;
        public float SwimSpeed_NextPassiveIncrease { get; set; } = 0f;
        public float SwimDistanceTravelled { get; set; } = 0f;
        public float AddedForwardSwimSpeed { get; set; } = 0f;
        public float AddedBackwardSwimSpeed { get; set; } = 0f;
        public float AddedStrafeSwimSpeed { get; set; } = 0f;
        public float AddedSwimAcceleration { get; set; } = 0f;


        //Land Speed stuff
        public float LandSpeed_PassiveIncrease { get; set; } = 0f;
        public float LandSpeed_NextPassiveIncrease { get; set; } = 0f;
        public float LandDistanceTravelled { get; set; } = 0f;
        public float AddedForwardLandSpeed { get; set; } = 0f;
        public float AddedBackwardLandSpeed { get; set; } = 0f;
        public float AddedStrafeLandSpeed { get; set; } = 0f;
        public float AddedLandAcceleration { get; set; } = 0f;

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

                //Set next level stuff for proper notifications
                float swimNextLevel = (float)Math.Truncate((saveData.SwimDistanceTravelled * Config.GetConfig().SwimSpeedBoost_Modifier)) + 1;
                saveData.SwimSpeed_NextPassiveIncrease = swimNextLevel;

                float landNextLevel = (float)Math.Truncate((saveData.LandDistanceTravelled * Config.GetConfig().LandSpeedBoost_Modifier)) + 1;
                saveData.LandSpeed_NextPassiveIncrease = landNextLevel;

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
