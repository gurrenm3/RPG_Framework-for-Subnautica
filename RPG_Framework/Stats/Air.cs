using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPG_Framework.Stats
{
    class Air
    {
        private static SaveData saveData;
        private static Config cfg = Config.GetConfig();
        public static float UpdateBreathPeriod(Player __instance, ref float __result)
        {
            saveData = SaveData.GetSaveData();
            cfg = Config.GetConfig();

            StatObject stat = new StatObject()
            {
                Name = "Breath Period",
                Level = saveData.BreathPeriodLevel,
                MaxLevel = cfg.MaxBreathPeriodLevel,
                XP = saveData.BreathPeriod_XP,
                XPToNextLevel = saveData.BreathPeriod_XPToNextLevel,
                Modifier = cfg.BreathPeriod_XPNextLevel_Multiplier
            };

            if (!StatMgr.CanLevelUp(stat))
            {
                __result += CalcBreathPeriodPercent(saveData.BreathPeriodLevel);
                return __result;
            }

            int gainedLevels = StatMgr.DoWhileLevelUp(stat);
            Air.NotifyLevelUp(stat);

            saveData.BreathPeriodLevel = stat.Level;
            saveData.BreathPeriod_XP = stat.XP;
            saveData.BreathPeriod_XPToNextLevel = stat.XPToNextLevel;

            __result += CalcBreathPeriodPercent(saveData.BreathPeriodLevel);
            return __result;
        }

        
        public static float UpdateOxygenPerBreath(Player __instance, ref float __result, float lastBreathInterval)
        {
            saveData = SaveData.GetSaveData();
            float num = __result / lastBreathInterval;
            float originalBreath = lastBreathInterval - CalcBreathPeriodPercent(saveData.BreathPeriodLevel);

            __result = originalBreath * num;
            return __result;
        }





        public static float CalcBreathPeriodPercent(int level)
        {
            return level * cfg.PercentBreathPeriodPerLevel;
        }

        public static float AddXP(float current, float max)
        {
            if (current == max) return 0f;

            float percentEmpty = (max - current) / max;

            float multiplier = percentEmpty * 2;
            float addXP = percentEmpty * multiplier / 10 * cfg.XP_Multiplier;
            return addXP;
        }

        public static void NotifyLevelUp(StatObject stat)
        {
            Random random = new Random();
            Air a = new Air();
            StatMgr mgr = new StatMgr();

            int responseNum = random.Next(0, a.singleLevelNotifs.Count - 1);
            Log.InGameMSG(String.Format(a.singleLevelNotifs[responseNum], stat.Name, CalcBreathPeriodPercent(stat.Level) + 3));


            if (stat.Level >= stat.MaxLevel)
            {
                int maxRandom = random.Next(0, mgr.maxLevelNotifs.Count - 1);
                Log.InGameMSG(String.Format(mgr.maxLevelNotifs[maxRandom], stat.Name) + ". Each breath now lasts " + (CalcBreathPeriodPercent(stat.Level) + 3) + " seconds");
            }
        }

        List<string> singleLevelNotifs = new List<string>
        {
            "{0} has gone up. Your breath lasts {1} seconds",
            "{0} has gone up. Your breath now lasts {1} seconds",
            "{0} has gone up. Your breath currently lasts {1} seconds",
            "{0} has gone up. Your breath takes {1} seconds",
            "{0} has been raised. Your breath lasts {1} seconds",
            "{0} has been raised. Your breath now lasts {1} seconds",
            "{0} has been raised. Your breath currently lasts {1} seconds",
            "{0} has been raised. Your breath takes {1} seconds",
            "{0} has increased. Your breath lasts {1} seconds",
            "{0} has increased. Your breath now lasts {1} seconds",
            "{0} has increased. Your breath currently lasts {1} seconds",
            "{0} has increased. Your breath takes {1} seconds",

            "{0} has gone up. Each breath lasts {1} seconds",
            "{0} has gone up. Each breath now lasts {1} seconds",
            "{0} has gone up. Each breath currently lasts {1} seconds",
            "{0} has gone up. Each breath takes {1} seconds",
            "{0} has been raised. Each breath lasts {1} seconds",
            "{0} has been raised. Each breath now lasts {1} seconds",
            "{0} has been raised. Each breath currently lasts {1} seconds",
            "{0} has been raised. Each breath takes {1} seconds",
            "{0} has increased. Each breath lasts {1} seconds",
            "{0} has increased. Each breath now lasts {1} seconds",
            "{0} has increased. Each breath currently lasts {1} seconds",
            "{0} has increased. Each breath takes {1} seconds",

            "Your {0} has gone up. Each breath lasts {1} seconds",
            "Your {0} has gone up. Each breath now lasts {1} seconds",
            "Your {0} has gone up. Each breath currently lasts {1} seconds",
            "Your {0} has gone up. Each breath takes {1} seconds",
            "Your {0} has been raised. Each breath lasts {1} seconds",
            "Your {0} has been raised. Each breath now lasts {1} seconds",
            "Your {0} has been raised. Each breath currently lasts {1} seconds",
            "Your {0} has been raised. Each breath takes {1} seconds",
            "Your {0} has increased. Each breath lasts {1} seconds",
            "Your {0} has increased. Each breath now lasts {1} seconds",
            "Your {0} has increased. Each breath currently lasts {1} seconds",
            "Your {0} has increased. Each breath takes {1} seconds"
        };
    }
}
