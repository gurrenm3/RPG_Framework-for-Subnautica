using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPG_Framework.Stats
{
    class Attack
    {
        //Will try again later when I figure out how to get damage dealer
    }

    [HarmonyPatch(typeof(CreatureDeath))]
    [HarmonyPatch("OnTakeDamage")]
    class IncreaseSavedAttack
    {
        [HarmonyPostfix]
        public static void Postfix(DamageInfo damageInfo)
        {
            //dealer is null for some reason. Will try again later
        }
    }
}
