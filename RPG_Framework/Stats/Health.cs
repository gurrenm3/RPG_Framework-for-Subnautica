using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPG_Framework.Stats
{
    class Health
    {
        private static SaveData saveData = SaveData.GetSaveData();
        private static Config cfg = Config.GetConfig();

        //float defaultMaxHealth = 100f;
        
        public static void UpdateHealth(Player __instance)
        {
            Health h = new Health();
            //__instance.liveMixin.data.maxHealth = h.defaultMaxHealth + saveData.HealthBonusLevel;
            __instance.liveMixin.data.maxHealth += saveData.HealthBonusLevel;

            StatObject stat = new StatObject()
            {
                Name = "Max Health",
                Level = saveData.HealthBonusLevel,
                MaxLevel = cfg.MaxHealthBoost,
                XP = saveData.Health_XP,
                XPToNextLevel = saveData.Health_XPToNextLevel,
                Modifier = cfg.HealthXP_Modifier
            };

            if (!StatMgr.CanLevelUp(stat)) return;

            int gainedLevels = StatMgr.DoWhileLevelUp(stat);
            StatMgr.NotifyLevelUp(stat, gainedLevels);

            saveData.HealthBonusLevel = stat.Level;
            saveData.Health_XP = stat.XP;
            saveData.Health_XPToNextLevel = stat.XPToNextLevel;
            SaveData.Save_SaveFile();

            /*if (saveData.HealthBonusLevel >= cfg.MaxHealthBoost) return;
            if (!XP_Handler.DoesHaveLevelUp(saveData.Health_XP, saveData.Health_XPToNextLevel)) return;

            int gainedLevels = 0;
            while (saveData.Health_XP >= saveData.Health_XPToNextLevel)
            {
                if (saveData.HealthBonusLevel >= cfg.MaxHealthBoost) break;
                gainedLevels++;
                saveData.HealthBonusLevel++;
                saveData.Health_XP -= saveData.Health_XPToNextLevel;
                if (saveData.Health_XP < 0) saveData.Health_XP = 0;

                saveData.Health_XPToNextLevel = XP_Handler.CalcXPToNextLevel(saveData.Health_XPToNextLevel, cfg.HealthXP_Modifier);
            }
            SaveData.Save_SaveFile();
            XP_Events.NotifyStatIncrease("Max health", gainedLevels, saveData.HealthBonusLevel);
            if (saveData.HealthBonusLevel >= cfg.MaxHealthBoost)
                Log.InGameMSG("Max health is now max level");*/

            __instance.liveMixin.data.maxHealth += saveData.HealthBonusLevel;
            //__instance.liveMixin.data.maxHealth = h.defaultMaxHealth + saveData.HealthBonusLevel;
        }
    }
}