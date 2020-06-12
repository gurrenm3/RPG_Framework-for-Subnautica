using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using RPG_Framework.LevelUp;

namespace RPG_Framework
{
    public class XP_Handler
    {
        public static float CalcXPToNextLevel(float currentXPtoNextLevel, float xpModifier)
        {
            float xpNextLevel = (currentXPtoNextLevel * xpModifier);
            return xpNextLevel;
        }

        public static bool DoesHaveLevelUp(float currentXP, float XPtoNextLevel)
        {
            if (currentXP >= XPtoNextLevel)
                return true;

            return false;
        }

        internal static Dictionary<string, float> XPGrowths = new Dictionary<string, float>()
        {
            {"Almost nothing | 0.1", 0.1f },
            {"Super slow | 0.25", 0.25f },
            {"Slow | 0.5", 0.5f },
            {"A little slower | 0.75", 0.75f },
            {"Normal | 1.0", 1.0f },
            {"A little faster | 1.25", 1.25f },
            {"Fast | 1.5", 1.5f },
            {"Faster | 1.75", 1.75f },
            {"Super fast | 2.0", 2f },
            {"Super super fast | 3.0", 3f },
            {"Extreme | 5.0", 5f },
            {"Max | 10.0", 10f },
            {"Reaper Leviathan | 25.0", 25f },
            {"Ghost Leviathan | 50.0", 50f },
            {"Sea Dragon Leviathan | 100.0", 100f },
            {"Custom | " + Config.GetConfig().XP_Multiplier, Config.GetConfig().XP_Multiplier }
        };

        internal static Dictionary<string, float> XPGrowthsNoCustom = new Dictionary<string, float>()
        {
            {"Almost nothing | 0.1", 0.1f },
            {"Super slow | 0.25", 0.25f },
            {"Slow | 0.5", 0.5f },
            {"A little slower | 0.75", 0.75f },
            {"Normal | 1.0", 1.0f },
            {"A little faster | 1.25", 1.25f },
            {"Fast | 1.5", 1.5f },
            {"Faster | 1.75", 1.75f },
            {"Super fast | 2.0", 2f },
            {"Super super fast | 3.0", 3f },
            {"Extreme | 5.0", 5f },
            {"Max | 10.0", 10f },
            {"Reaper Leviathan | 25.0", 25f },
            {"Ghost Leviathan | 50.0", 50f },
            {"Sea Dragon Leviathan | 100.0", 100f }
        };
    }
}
