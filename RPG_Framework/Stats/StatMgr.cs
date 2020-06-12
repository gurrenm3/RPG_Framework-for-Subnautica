using RPG_Framework.LevelUp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;

namespace RPG_Framework.Stats
{
    class StatMgr
    {
        private static Config cfg = Config.GetConfig();

        public static float AddXP(float current, float max)
        {
            if (current == max) return 0f;
            cfg = Config.GetConfig();

            float percentEmpty = (max - current) / max;

            float multiplier = percentEmpty * 2;
            float addXP = percentEmpty * multiplier / 10 * cfg.XP_Multiplier;
            return addXP;
        }

        public static bool CanLevelUp(StatObject stat)
        {
            if (stat.Level >= stat.MaxLevel) return false;

            if (!XP_Handler.DoesHaveLevelUp(stat.XP, stat.XPToNextLevel)) return false;

            LevelingSystem.PlayLevelUpSound();
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
            cfg = Config.GetConfig();
            float resistance = level * cfg.PercentResistancePerLevel;
            
            if (resistance >= 100)
                resistance = 100;

            return resistance;
        }

        List<string> singleLevelNotifs = new List<string>
        {
            "{0} has gone up. Current level: {1}",
            "{0} has gone up. Current level is {1}",
            "{0} has gone up. Currently at level {1}",
            "{0} has gained a level. Current level: {1}",
            "{0} has gained a level. Current level is {1}",
            "{0} has gained a level. Currently at level {1}",
            "{0} has been raised. Current level: {1}",
            "{0} has been raised. Current level is {1}",
            "{0} has been raised. Currently at level {1}",
            "{0} has increased. Current level: {1}",
            "{0} has increased. Current level is {1}",
            "{0} has increased. Currently at level {1}",

            "{0} has gone up. Your current level: {1}",
            "{0} has gone up. Your current level is {1}",
            "{0} has gone up. You are currently at level {1}",
            "{0} has gained a level. Your current level: {1}",
            "{0} has gained a level. Your current level is {1}",
            "{0} has gained a level. You are currently at level {1}",
            "{0} has been raised. Your current level: {1}",
            "{0} has been raised. Your current level is {1}",
            "{0} has been raised. You are currently at level {1}",
            "{0} has increased. Your current level: {1}",
            "{0} has increased. Your current level is {1}",
            "{0} has increased. You are currently at level {1}",

            "Your {0} has gone up. Current level: {1}",
            "Your {0} has gone up. Current level is {1}",
            "Your {0} has gone up. Currently at level {1}",
            "Your {0} has gained a level. Current level is {1}",
            "Your {0} has gained a level. Currently at level {1}",
            "Your {0} has been raised. Current level: {1}",
            "Your {0} has been raised. Current level is {1}",
            "Your {0} has been raised. Currently at level {1}",
            "Your {0} has increased. Current level: {1}",
            "Your {0} has increased. Current level is {1}",
            "Your {0} has increased. Currently at level {1}"
        };

        List<string> multiLevelNotifs = new List<string>
        {
            "{0} has gone up by {1}. Current level: {2}",
            "{0} has gone up by {1}. Current level is {2}",
            "{0} has gone up by {1}. Currently at level {2}",
            "{0} has gained {1} levels. Current level: {2}",
            "{0} has gained {1} levels. Current level is {2}",
            "{0} has gained {1} levels. Currently at level {2}",
            "{0} has been raised by {1}. Current level: {2}",
            "{0} has been raised by {1}. Current level is {2}",
            "{0} has been raised by {1}. Currently at level {2}",
            "{0} has increased by {1}. Current level: {2}",
            "{0} has increased by {1}. Current level is {2}",
            "{0} has increased by {1}. Currently at level {2}",

            "{0} has gone up by {1}. Your current level: {2}",
            "{0} has gone up by {1}. Your current level is {2}",
            "{0} has gone up by {1}.  You are currently at level {2}",
            "{0} has gained {1} levels. Your current level: {2}",
            "{0} has gained {1} levels. Your current level is {2}",
            "{0} has gained {1} levels. You are currently at level {2}",
            "{0} has been raised by {1}. Your current level: {2}",
            "{0} has been raised by {1}. Your current level is {2}",
            "{0} has been raised by {1}. You are currently at level {2}",
            "{0} has increased by {1}. Your current level: {2}",
            "{0} has increased by {1}. Your current level is {2}",
            "{0} has increased by {1}. You are currently at level {2}",

            "Your {0} has gone up by {1}. Current level: {2}",
            "Your {0} has gone up by {1}. Current level is {2}",
            "Your {0} has gone up by {1}. Currently at level {2}",
            "Your {0} has gained {1} levels. Current level: {2}",
            "Your {0} has gained {1} levels. Current level is {2}",
            "Your {0} has gained {1} levels. Currently at level {2}",
            "Your {0} has been raised by {1}. Current level: {2}",
            "Your {0} has been raised by {1}. Current level is {2}",
            "Your {0} has been raised by {1}. Currently at level {2}",
            "Your {0} has increased by {1}. Current level: {2}",
            "Your {0} has increased by {1}. Current level is {2}",
            "Your {0} has increased by {1}. Currently at level {2}"
        };


        public List<string> maxLevelNotifs = new List<string>
        {
            "{0} has reached max level",
            "{0} has reached the max level",
            "{0} is now max level",
            "{0} has reached the level cap",
            "{0} is maxed out",
            "{0} is as high as it can go",
            "{0} is max level and can't go any higher",
            "{0} is max level",

            "Your {0} has reached max level",
            "Your {0} has reached the max level",
            "Your {0} is now max level",
            "Your {0} has reached the level cap",
            "Your {0} is maxed out",
            "Your {0} is as high as it can go",
            "Your {0} is max level and can't go any higher",
            "Your {0} is max level",

            "You've maxed out {0}",
            "You've maxed out your {0}",
            "You've maxed out your {0} stat",
            "You've reached max level for {0}",
            "You've reached max level for your {0}",
            "You've reached max level for your {0} stat",
        };

        public static void NotifyLevelUp(StatObject stat, int gainedLevels)
        {
            Random random = new Random();
            StatMgr mgr = new StatMgr();
            int responseNum = random.Next(0, mgr.singleLevelNotifs.Count -1);

            
            if (gainedLevels == 1)
                Log.InGameMSG(String.Format(mgr.singleLevelNotifs[responseNum], stat.Name, stat.Level));
            else
                Log.InGameMSG(String.Format(mgr.multiLevelNotifs[responseNum], stat.Name, gainedLevels, stat.Level));

            if (stat.Level >= stat.MaxLevel)
            {
                int maxRandom = random.Next(0, mgr.maxLevelNotifs.Count - 1);
                Log.InGameMSG(String.Format(mgr.maxLevelNotifs[maxRandom], stat.Name));
            }
        }
    }

    public class StatObject
    {
        public string Name { get; set; }

        public int Level { get; set; }
        public int MaxLevel { get; set; }
        public float  XP { get; set; }
        public float XPToNextLevel { get; set; }
        public float Modifier { get; set; }
    }
}
