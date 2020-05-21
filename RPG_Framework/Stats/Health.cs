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
            StatObject stat = new StatObject()
            {
                Name = "Max Health",
                Level = saveData.HealthBonusLevel,
                MaxLevel = cfg.MaxHealthBoost,
                XP = saveData.Health_XP,
                XPToNextLevel = saveData.Health_XPToNextLevel,
                Modifier = cfg.HealthXP_Modifier
            };

            if (!StatMgr.CanLevelUp(stat))
            {
                __instance.liveMixin.data.maxHealth = 100 + saveData.HealthBonusLevel;
                SaveData.Save_SaveFile();
                return;
            }

            int gainedLevels = StatMgr.DoWhileLevelUp(stat);
            StatMgr.NotifyLevelUp(stat, gainedLevels);

            saveData.HealthBonusLevel = stat.Level;
            saveData.Health_XP = stat.XP;
            saveData.Health_XPToNextLevel = stat.XPToNextLevel;
            SaveData.Save_SaveFile();

            __instance.liveMixin.data.maxHealth = 100 + saveData.HealthBonusLevel;
        }
    }
}