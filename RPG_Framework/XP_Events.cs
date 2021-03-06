﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarmonyLib;
using UnityEngine;
using Random = System.Random;

namespace RPG_Framework
{
    class XP_Events
    {
        const float eventDuration = 5f;
        static Config cfg;
        internal static void AddExperience(float amount)
        {
            SaveData.GetSaveData().PlayerXP += amount;
        }

        private static bool hasAppliedBonusXP;
        internal static void DoubleXPEvent()
        {
            cfg = Config.GetConfig();
            XP_Events events = new XP_Events();
            if (!events.IsDoubleXP())
            {
                if (cfg.DoubleXPApplied || eventInProgress)
                    events.StopEvent();
                return;
            }

            events.StartXPEvent();
        }

        static bool eventInProgress = false;
        private void StartXPEvent()
        {
            if(!eventInProgress)
            {
                numDoubleXPChecks = 0;
                eventInProgress = true;
                cfg.DoubleXPApplied = true;

                cfg.XP_Multiplier *= 2;
                Config.SaveConfig();
                eventTime = time + (eventDuration * 60);
                Logger.Log("Double XP event has started! You get twice as much XP for the next " + eventDuration + " minutes");
            }
        }

        static float eventTime;
        private bool IsEventOver()
        {
            return Time.time > eventTime;
        }

        private void StopEvent()
        {
            eventInProgress = false;
            cfg.XP_Multiplier /= 2;
            cfg.DoubleXPApplied = false;
            Config.SaveConfig();
        }

        private static float time;
        private static int numDoubleXPChecks = 0;
        private bool IsDoubleXP()
        {
            if (!cfg.EnableDoubleXPEvents)
                return false;

            if (eventInProgress)
            {
                if (!IsEventOver())
                    return true;

                return false;
            }

            float waitTime = 600;
            if (Time.time < time)
                return false;

            time = Time.time + waitTime;


            numDoubleXPChecks++;
            if(numDoubleXPChecks > 20)
                return true;

            
            Random rand = new Random();
            bool result = rand.Next(0, 5) == 1;
            if (!result || (result && numDoubleXPChecks < 4))
                return false;

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

                /*Logger.Log(CreatureClass.GetName(target) + " is dead!");
                CreatureClass.GetMeatDropCount(CreatureClass.GetName(target));*/
            }
        }
        #endregion
    }
}
