using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;

namespace RPG_Framework
{
    class CreatureClass
    {
        /// <summary>
        /// Removes all extra charectes but the actual creatures name
        /// </summary>
        /// <param name="__instance">Creature __instance</param>
        /// <returns>Cleaned creature name</returns>
        public static string GetName(Creature __instance)
        {
            return __instance.gameObject.name.Split('(')[0];
        }
        
        /// <summary>
        /// Removes all extra charectes but the actual creatures name
        /// </summary>
        /// <param name="__instance">CreatureDeath __instance</param>
        /// <returns>Cleaned creature name</returns>
        public static string GetName(CreatureDeath __instance)
        {
            return __instance.gameObject.name.Split('(')[0];
        }
    }
}
