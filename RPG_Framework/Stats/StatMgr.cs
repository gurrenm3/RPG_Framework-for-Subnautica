using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPG_Framework.Stats
{
    class StatMgr
    {
        private static Config cfg = Config.GetConfig();

        public static float AddXP(float current, float max)
        {
            if (current == max) return 0f;

            float addXP = 0f;
            float percentEmpty = (max - current) / max;

            float multiplier = percentEmpty * 2;
            addXP = percentEmpty * multiplier / 10;
            return addXP;
        }

        public static bool CanLevelUp(StatObject stat)
        {
            if (stat.Level >= stat.MaxLevel) return false;
            if (!XP_Handler.DoesHaveLevelUp(stat.XP, stat.XPToNextLevel)) return false;

            return true;
        }

        public static int DoWhileLevelUp(StatObject stat)
        {
            int gainedLevels = 0;

            do
            {
                if (stat.Level >= stat.MaxLevel) break;

                gainedLevels++;
                stat.Level++;
                stat.XP -= stat.XPToNextLevel;

                if (stat.XP < 0)
                    stat.XP = 0;

                stat.XPToNextLevel = XP_Handler.CalcXPToNextLevel(stat.XPToNextLevel, stat.Modifier);
            }
            while (stat.XP >= stat.XPToNextLevel);
            
            return gainedLevels;
        }


        public static float CalcResistance(int level)
        {
            float resistance = level * cfg.PercentResistancePerLevel;
            
            if (resistance >= 100)
                resistance = 100;

            return resistance;
        }
        
        public static void NotifyLevelUp(StatObject stat, int gainedLevels)
        {
            XP_Events.NotifyStatIncrease(stat.Name, gainedLevels, stat.Level);
            if (stat.Level >= stat.MaxLevel)
                Log.InGameMSG(stat.Name + " is now max level");
        }


        public static void Notify_ResistanceLevelUp(StatObject stat, int gainedLevels, float currentResistance)
        {
            if (gainedLevels == 1)
                Log.InGameMSG(stat.Name + " has gained a level. Current resistance is " + currentResistance + "%");
            else
                Log.InGameMSG(stat.Name + " has gained " + gainedLevels + " levels. Current resistance is " + currentResistance + "%");


            if (stat.Level >= stat.MaxLevel)
                Log.InGameMSG(stat.Name + " is now max level");
        }
    }

    public class StatObject
    {
        public string Name { get; set; }

        public int Level { get; set; }
        public int MaxLevel { get; set; }
        public float XP { get; set; }
        public float XPToNextLevel { get; set; }
        public float Modifier { get; set; }
    }
}
