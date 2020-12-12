using System.Collections.Generic;

namespace RPG_Framework.Stats
{
    internal class RPGStatManager
    {
        private static List<RPGStat> allPlayerRPGStats;
        public static List<RPGStat> AllPlayerRPGStats
        {
            get { return allPlayerRPGStats; }
            set { allPlayerRPGStats = value; }
        }
    }
}
