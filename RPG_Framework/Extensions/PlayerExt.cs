using RPG_Framework.Stats;
using System.Collections.Generic;
using System.Linq;

namespace RPG_Framework.Extensions
{
    public static class PlayerExt
    {
        public static List<RPGStat> GetAllRPGStats(this Player player) => RPGStatManager.AllPlayerRPGStats;
        public static void AddRPGStat(this Player player, RPGStat statToAdd) => RPGStatManager.AllPlayerRPGStats.Add(statToAdd);
        public static RPGStat GetRPGStat(this Player player, string statToGet)
        {
            if (string.IsNullOrEmpty(statToGet))
                return null;

            var stats = player.GetAllRPGStats();
            var aquiredStat = from stat in stats
                         where stat.Name == stat.Name
                         select stat;

            return aquiredStat.First();
        }
    }
}
