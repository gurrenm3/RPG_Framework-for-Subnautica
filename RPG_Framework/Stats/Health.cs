using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPG_Framework.Stats
{
    class Health
    {
        private static SaveData saveData;
        private static Config cfg = Config.GetConfig();

        //float defaultMaxHealth = 100f;
        
        public static void UpdateHealth(Player __instance)
        {
            saveData = SaveData.GetSaveData();
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
                return;
            }

            int gainedLevels = StatMgr.DoWhileLevelUp(stat);
            StatMgr.NotifyLevelUp(stat, gainedLevels);

            saveData.HealthBonusLevel = stat.Level;
            saveData.Health_XP = stat.XP;
            saveData.Health_XPToNextLevel = stat.XPToNextLevel;

            __instance.liveMixin.data.maxHealth = 100 + saveData.HealthBonusLevel;
        }
    }
}