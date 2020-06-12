using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPG_Framework.Stats
{
    class Food
    {
        private static Config cfg;
        private static SaveData saveData;

        public static void UpdateFood(Player __instance)
        {
            /*saveData = SaveData.GetSaveData();
            cfg = Config.GetConfig();
            StatObject stat = new StatObject()
            {
                Name = "Max Food",
                Level = saveData.FoodBonusLevel,
                MaxLevel = cfg.MaxFoodBoost,
                XP = saveData.Food_XP,
                XPToNextLevel = saveData.Food_XPToNextLevel,
                Modifier = cfg.FoodXP_Modifier
            };

            if (!StatMgr.CanLevelUp(stat)) 
            {
                return;
            }

            int gainedLevels = StatMgr.DoWhileLevelUp(stat);
            StatMgr.NotifyLevelUp(stat, gainedLevels);

            saveData.FoodBonusLevel = stat.Level;
            saveData.Food_XP = stat.XP;
            saveData.Food_XPToNextLevel = stat.XPToNextLevel;*/
        }
    }
}
