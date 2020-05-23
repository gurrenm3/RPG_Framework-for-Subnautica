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
        private static SaveData saveData;
        private static float startSuffocation = 3;

        public static void UpdateSuffocation(Player __instance)
        {
            saveData = SaveData.GetSaveData();
            StatObject stat = new StatObject()
            {
                Name = "Suffocation Time",
                Level = saveData.SuffocateResistLevel,
                MaxLevel = cfg.MaxSuffocateResistLevel,
                XP = saveData.SuffocateResist_XP,
                XPToNextLevel = saveData.SuffocateResist_XPToNextLevel,
                Modifier = cfg.SuffocateResist_XPNextLevel_Multiplier
            };

            if (!StatMgr.CanLevelUp(stat))
            {
                __instance.suffocationTime = 8 + saveData.SuffocateResistLevel;
                return;
            }

            int gainedLevels = StatMgr.DoWhileLevelUp(stat);
            Notify_LevelUp(stat, gainedLevels, stat.Level);

            saveData.SuffocateResistLevel = stat.Level;
            saveData.SuffocateResist_XP = stat.XP;
            saveData.SuffocateResist_XPToNextLevel = stat.XPToNextLevel;

            __instance.suffocationTime = 8 + saveData.SuffocateResistLevel;
        }

        public static void Notify_LevelUp(StatObject stat, int gainedLevels, float currentResistance)
        {
            System.Random random = new System.Random();
            Suffocation s = new Suffocation();
            StatMgr mgr = new StatMgr();

            int responseNum = random.Next(0, s.Notifs.Count - 1);
            Log.InGameMSG(String.Format(s.Notifs[responseNum], stat.Name, 8 + stat.Level));


            if (stat.Level >= stat.MaxLevel)
            {
                int maxRandom = random.Next(0, mgr.maxLevelNotifs.Count - 1);
                Log.InGameMSG(String.Format(mgr.maxLevelNotifs[maxRandom], stat.Name));
            }
        }

        List<string> Notifs = new List<string>
        {
            "{0} has gone up. You're now able to last {1} seconds before suffocating",
            "{0} has gone up. Currently you can last {1} seconds before suffocating",
            "{0} has gone up. You can last {1} seconds before suffocating",
            "{0} has gone up. You can survive {1} seconds before suffocating",
            "{0} has gone up. You're now able to last {1} seconds before passing out",
            "{0} has gone up. Currently you can last {1} seconds before passing out",
            "{0} has gone up. You can last {1} seconds before passing out",
            "{0} has gone up. You can survive {1} seconds before passing out",
            "{0} has gone up. You're now able to last {1} seconds before drowning",
            "{0} has gone up. Currently you can last {1} seconds before drowning",
            "{0} has gone up. You can last {1} seconds before drowning",
            "{0} has gone up. You can survive {1} seconds before drowning",

            "{0} has been raised. You're now able to last {1} seconds before suffocating",
            "{0} has been raised. Currently you can last {1} seconds before suffocating",
            "{0} has been raised. You can last {1} seconds before suffocating",
            "{0} has been raised. You can survive {1} seconds before suffocating",
            "{0} has been raised. You're now able to last {1} seconds before passing out",
            "{0} has been raised. Currently you can last {1} seconds before passing out",
            "{0} has been raised. You can last {1} seconds before passing out",
            "{0} has been raised. You can survive {1} seconds before passing out",
            "{0} has been raised. You're now able to last {1} seconds before drowning",
            "{0} has been raised. Currently you can last {1} seconds before drowning",
            "{0} has been raised. You can last {1} seconds before drowning",
            "{0} has been raised. You can survive {1} seconds before drowning",

            "{0} has increased. You're now able to last {1} seconds before suffocating",
            "{0} has increased. Currently you can last {1} seconds before suffocating",
            "{0} has increased. You can last {1} seconds before suffocating",
            "{0} has increased. You can survive {1} seconds before suffocating",
            "{0} has increased. You're now able to last {1} seconds before passing out",
            "{0} has increased. Currently you can last {1} seconds before passing out",
            "{0} has increased. You can last {1} seconds before passing out",
            "{0} has increased. You can survive {1} seconds before passing out",
            "{0} has increased. You're now able to last {1} seconds before drowning",
            "{0} has increased. Currently you can last {1} seconds before drowning",
            "{0} has increased. You can last {1} seconds before drowning",
            "{0} has increased. You can survive {1} seconds before drowning",


            "Your {0} has gone up. You're now able to last {1} seconds before suffocating",
            "Your {0} has gone up. Currently you can last {1} seconds before suffocating",
            "Your {0} has gone up. You can last {1} seconds before suffocating",
            "Your {0} has gone up. You can survive {1} seconds before suffocating",
            "Your {0} has gone up. You're now able to last {1} seconds before passing out",
            "Your {0} has gone up. Currently you can last {1} seconds before passing out",
            "Your {0} has gone up. You can last {1} seconds before passing out",
            "Your {0} has gone up. You can survive {1} seconds before passing out",
            "Your {0} has gone up. You're now able to last {1} seconds before drowning",
            "Your {0} has gone up. Currently you can last {1} seconds before drowning",
            "Your {0} has gone up. You can last {1} seconds before drowning",
            "Your {0} has gone up. You can survive {1} seconds before drowning",

            "Your {0} has been raised. You're now able to last {1} seconds before suffocating",
            "Your {0} has been raised. Currently you can last {1} seconds before suffocating",
            "Your {0} has been raised. You can last {1} seconds before suffocating",
            "Your {0} has been raised. You can survive {1} seconds before suffocating",
            "Your {0} has been raised. You're now able to last {1} seconds before passing out",
            "Your {0} has been raised. Currently you can last {1} seconds before passing out",
            "Your {0} has been raised. You can last {1} seconds before passing out",
            "Your {0} has been raised. You can survive {1} seconds before passing out",
            "Your {0} has been raised. You're now able to last {1} seconds before drowning",
            "Your {0} has been raised. Currently you can last {1} seconds before drowning",
            "Your {0} has been raised. You can last {1} seconds before drowning",
            "Your {0} has been raised. You can survive {1} seconds before drowning",

            "Your {0} has increased. You're now able to last {1} seconds before suffocating",
            "Your {0} has increased. Currently you can last {1} seconds before suffocating",
            "Your {0} has increased. You can last {1} seconds before suffocating",
            "Your {0} has increased. You can survive {1} seconds before suffocating",
            "Your {0} has increased. You're now able to last {1} seconds before passing out",
            "Your {0} has increased. Currently you can last {1} seconds before passing out",
            "Your {0} has increased. You can last {1} seconds before passing out",
            "Your {0} has increased. You can survive {1} seconds before passing out",
            "Your {0} has increased. You're now able to last {1} seconds before drowning",
            "Your {0} has increased. Currently you can last {1} seconds before drowning",
            "Your {0} has increased. You can last {1} seconds before drowning",
            "Your {0} has increased. You can survive {1} seconds before drowning"
        };
    }
}
