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




        public static float CalcStatBoost(float total, float modifier)
        {
            float boost = total * modifier;
            float baseLevel = (float)Math.Truncate(total * modifier);
            float newModifier = 1 / ((1 / modifier / 4 * baseLevel * 2) + (1 / modifier));


            return boost;
        }
    }
}
