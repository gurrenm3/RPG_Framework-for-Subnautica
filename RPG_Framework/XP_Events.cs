using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using RPG_Framework.Items.Pills;
using RPG_Framework.UI;
using UnityEngine;
using Random = System.Random;

namespace RPG_Framework
{
    class XP_Events
    {
        internal static void AddExperience(float amount)
        {
            SaveData.GetSaveData().PlayerXP += amount;
        }

        private static bool hasAppliedBonusXP;
        internal static void DoubleXPEvent()
        {
            XP_Events events = new XP_Events();
            if (!events.IsDoubleXP())
            {
                if(hasAppliedBonusXP)
                {
                    var cfg = Config.GetConfig();
                    cfg.XP_Multiplier -= 1;
                }    
                return;
            }

            Log.InGameMSG("Double XP event has started! You get twice as much XP for the next 5 minutes");
            hasAppliedBonusXP = true;
        }

        private static float time;
        private static int numDoubleXPChecks = 0;
        private bool IsDoubleXP()
        {
            float waitTime = 600;
            if (Time.time < time)
                return false;
            time = Time.time + waitTime;


            numDoubleXPChecks++;
            if(numDoubleXPChecks > 20)
            {
                numDoubleXPChecks = 0;
                return true;
            }

            
            Random rand = new Random();
            bool result = rand.Next(0, 5) == 1;
            if (!result || (result && numDoubleXPChecks < 4))
                return false;


            numDoubleXPChecks = 0;
            return true;
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




        [HarmonyPatch(typeof(Knife))]
        [HarmonyPatch("GiveResourceOnDamage")]
        class KnifePatch
        {
            [HarmonyPostfix]
            public static void Postfix(GameObject target, bool isAlive)
            {
                //Test.Start();
                /*if (isAlive) return;

                Random rand = new System.Random();

                var a = Items.Items.GetModdedTechType("BluePill");
                var count = CreatureClass.GetMeatDropCount(CreatureClass.GetName(target));
                if (count == null) return;*/


                //CraftData.AddToInventory(a, rand.Next(count[0], count[1]), false, true);

                /*Log.InGameMSG(CreatureClass.GetName(target) + " is dead!");
                CreatureClass.GetMeatDropCount(CreatureClass.GetName(target));*/
            }
        }
        #endregion
    }
}
