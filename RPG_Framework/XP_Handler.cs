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
    }
}
