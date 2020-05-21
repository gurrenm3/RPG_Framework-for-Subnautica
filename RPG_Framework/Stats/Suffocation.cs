using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;

namespace RPG_Framework.Stats
{
    class Suffocation
    {
        private static Config cfg = Config.GetConfig();
        private static SaveData saveData = SaveData.GetSaveData();
        private static float startSuffocation = 3;

        public static void UpdateSuffocation(Player __instance)
        {
            StatObject stat = new StatObject()
            {
                Name = "Suffocation Time",
                Level = saveData.SuffocateResistLevel,
                MaxLevel = cfg.MaxSuffocateResistLevel,
                XP = saveData.SuffocateResist_XP,
                XPToNextLevel = saveData.SuffocateResist_XPToNextLevel,
                Modifier = cfg.SuffocateResistModifier
            };

            if (!StatMgr.CanLevelUp(stat))
            {
                __instance.suffocationTime = 8 + saveData.SuffocateResistLevel;
                SaveData.Save_SaveFile();
                return;
            }

            int gainedLevels = StatMgr.DoWhileLevelUp(stat);
            Notify_LevelUp(stat, gainedLevels, stat.Level);

            saveData.SuffocateResistLevel = stat.Level;
            saveData.SuffocateResist_XP = stat.XP;
            saveData.SuffocateResist_XPToNextLevel = stat.XPToNextLevel;
            SaveData.Save_SaveFile();

            __instance.suffocationTime = 8 + saveData.SuffocateResistLevel;
        }

        public static void Notify_LevelUp(StatObject stat, int gainedLevels, float currentResistance)
        {
            Log.InGameMSG(stat.Name + " has increased. You can now last " + (currentResistance + 8) + " seconds before suffocating from no air");

            if (stat.Level >= stat.MaxLevel)
                Log.InGameMSG(stat.Name + " is now max level");
        }
    }
}
