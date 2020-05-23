using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;

namespace RPG_Framework
{
    class XP_Events
    {
        public static void AddExperience(float amount)
        {
            SaveData.GetSaveData().PlayerXP += amount;
        }


        //
        // Harmony Events
        //
        #region Harmony Events
        [HarmonyPatch(typeof(CreatureDeath))]
        [HarmonyPatch("OnKill")]
        class CreatureDeath_OnKill
        {
            [HarmonyPostfix]
            public static void Postfix(CreatureDeath __instance)
            {
                AddExperience(__instance.liveMixin.maxHealth * Config.GetConfig().OnKillcreatureKillXP_Modifier * Config.GetConfig().XP_Multiplier);
            }
        }

        #endregion
    }
}
