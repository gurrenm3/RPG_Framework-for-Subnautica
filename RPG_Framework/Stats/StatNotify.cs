using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPG_Framework.Stats
{
    class StatNotify
    {
        public static bool HasStatIncrease(float current, float nextWholeLevel)
        {
            if (Math.Truncate(current) >= nextWholeLevel)
                return true;

            return false;
        }

        public static void NotifyStatIncrease(string stat, float amount, float currentBoost)
        {
            Log.InGameMSG(stat + " has increased by " + amount + ". Current boost: " + Math.Truncate(currentBoost));
        }
    }
}
