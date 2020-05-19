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

        float defaultMaxHealth = 100f;

        public static float AddXP(Player __instance)
        {
            float addXP = 0f;
            float percentEmpty = (__instance.liveMixin.maxHealth - __instance.liveMixin.health) / 100;

            float multiplier = 1;
            if (percentEmpty >= 33 && percentEmpty < 66) multiplier = 1.35f;
            else if (percentEmpty >= 66 && percentEmpty < 90) multiplier = 1.75f;
            else if (percentEmpty >= 90 && percentEmpty < 95) multiplier = 2.25f;
            else if (percentEmpty >= 95 && percentEmpty < 100) multiplier = 3;

            addXP = percentEmpty * multiplier/10;
            return addXP;
        }
        
        public static void UpdateHealth(Player __instance)
        {
            Health h = new Health();
            __instance.liveMixin.data.maxHealth = h.defaultMaxHealth + saveData.HealthBonusLevel;

            if (saveData.HealthBonusLevel >= cfg.MaxHealthBoost) return;
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
                Log.InGameMSG("Max health is now max level");

            __instance.liveMixin.data.maxHealth = h.defaultMaxHealth + saveData.HealthBonusLevel;
        }
    }
}
