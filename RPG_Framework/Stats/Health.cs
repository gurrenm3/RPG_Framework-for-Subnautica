using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPG_Framework.Stats
{
    class Health
    {
        private static SaveData saveData;
        private static Config cfg;

        //float defaultMaxHealth = 100f;
        
        public static void UpdateHealth(Player __instance)
        {
            saveData = SaveData.GetSaveData();
            cfg = Config.GetConfig();

            StatObject stat = new StatObject()
            {
                Name = "Max Health",
                Level = saveData.HealthLevel,
                MaxLevel = cfg.MaxHealthLevel,
                XP = saveData.Health_XP,
                XPToNextLevel = saveData.Health_XPToNextLevel,
                Modifier = cfg.Health_XPNextLevel_Multiplier
            };

            if (!StatMgr.CanLevelUp(stat))
            {
                __instance.liveMixin.data.maxHealth = 100 + saveData.HealthLevel;
                return;
            }

            int gainedLevels = StatMgr.DoWhileLevelUp(stat);
            StatMgr.NotifyLevelUp(stat, gainedLevels);

            saveData.HealthLevel = stat.Level;
            saveData.Health_XP = stat.XP;
            saveData.Health_XPToNextLevel = stat.XPToNextLevel;

            __instance.liveMixin.data.maxHealth = 100 + saveData.HealthLevel;
        }
    }
}