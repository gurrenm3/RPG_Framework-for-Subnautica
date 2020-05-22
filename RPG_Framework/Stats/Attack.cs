using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RPG_Framework.Stats
{
    
    class Attack
    {
        //Will try again later when I figure out how to get damage dealer
    }

    [HarmonyPatch(typeof(GUIHand))]
    [HarmonyPatch("GetActiveTarget")]
    class Creature_OnTakeDamage_Patch
    {
        //private static SaveData saveData;
        [HarmonyPostfix]
        public static void Postfix(GameObject __result)
        {
            //saveData = SaveData.GetSaveData();
            //dealer is null for some reason. Will try again later

        }
    }



    [HarmonyPatch(typeof(CreatureDeath))]
    [HarmonyPatch("OnTakeDamage")]
    class CreatureDeath_OnTakeDamage_Patch
    {
        //private static SaveData saveData;
        [HarmonyPostfix]
        public static void Postfix(DamageInfo damageInfo)
        {
            /*if (giveXP)
                Log.InGameMSG("Can give XP");*/
            //saveData = SaveData.GetSaveData();
            //dealer is null for some reason. Will try again later

        }
    }



    [HarmonyPatch(typeof(CreatureDeath))]
    [HarmonyPatch("OnAttackByCreature")]
    class CreatureDeath_OnAttackByCreature_Patch
    {
        //private static SaveData saveData;
        [HarmonyPostfix]
        public static void Postfix(CreatureDeath __instance)
        {
            //saveData = SaveData.GetSaveData();
            //dealer is null for some reason. Will try again later

        }
    }
}
