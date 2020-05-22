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
        //private static float lastBreathInterval = 0f;
        public static float UpdateBreathPeriod(Player __instance, ref float __result)
        {
            saveData = SaveData.GetSaveData();
            StatObject stat = new StatObject()
            {
                Name = "Breath Period",
                Level = saveData.BreathPeriodLevel,
                MaxLevel = cfg.MaxBreathPeriodBoost,
                XP = saveData.BreathPeriod_XP,
                XPToNextLevel = saveData.BreathPeriod_XPToNextLevel,
                Modifier = cfg.BreathPeriodXP_Modifier
            };

            if (!StatMgr.CanLevelUp(stat))
            {
                SaveData.Save_SaveFile();
                __result += CalcBreathPeriodPercent(saveData.BreathPeriodLevel);
                //lastBreathInterval = __result;
                return __result;
            }

            int gainedLevels = StatMgr.DoWhileLevelUp(stat);
            StatMgr.NotifyLevelUp(stat, gainedLevels);

            saveData.BreathPeriodLevel = stat.Level;
            saveData.BreathPeriod_XP = stat.XP;
            saveData.BreathPeriod_XPToNextLevel = stat.XPToNextLevel;
            SaveData.Save_SaveFile();

            __result += CalcBreathPeriodPercent(saveData.BreathPeriodLevel);
            //lastBreathInterval = __result;
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
    }
}
