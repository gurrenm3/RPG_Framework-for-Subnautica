using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;

namespace RPG_Framework
{
    class XP_Events
    {
        public static void OnLevelUp()
        {
            
        }


        #region Harmony Events
        [HarmonyPatch(typeof(CreatureDeath))]
        [HarmonyPatch("OnKill")]
        class CreatureDeath_OnKill
        {
            [HarmonyPostfix]
            //public static void Postfix(CreatureDeath __instance) => AddXP.OnCreatureKilled(__instance);
            public static void Postfix(CreatureDeath __instance)
            {
                Log.InGameMSG("Killed " + CreatureClass.GetName(__instance));
                AddXP.OnCreatureKilled(__instance);
            }
        }

        #endregion
    }
}
