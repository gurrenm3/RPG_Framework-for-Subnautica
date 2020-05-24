using Harmony;
using RPG_Framework.Stats;
using SMLHelper.V2.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RPG_Framework
{
    [HarmonyPatch(typeof(Player))]
    [HarmonyPatch("OnTakeDamage")]
    class Player_OnTakeDamage_Patch
    {
        [HarmonyPrefix]
        public static bool Prefix(Player __instance, DamageInfo damageInfo)
        {
            if (Guard.IsGamePaused())
                return true;

            UpdateResistance(__instance, damageInfo);
            return true;
        }
        public static void UpdateResistance(Player __instance, DamageInfo damageInfo)
        {
            DamageResistance.ApplyResistance(__instance, damageInfo);
        }
    }


    [HarmonyPatch(typeof(Player))]
    [HarmonyPatch("Update")]
    class Player_Update
    {
        private static SaveData saveData;
        private static Config cfg = Config.GetConfig();
        private static Player_Update pUpdate = new Player_Update();

        [HarmonyPostfix]
        public static void PostFix(Player __instance)
        {
            if (Guard.IsGamePaused())
                return;

            saveData = SaveData.GetSaveData();
            pUpdate.UpdateMovement(__instance);

            pUpdate.UpdateHealth(__instance);

            pUpdate.UpdateSuffocation(__instance);
        }

        public void UpdateMovement(Player __instance)
        {
            if (__instance.IsUnderwaterForSwimming() || __instance.motorMode == Player.MotorMode.Dive)  //add xp to swim speed
            {
                if (saveData.SwimSpeedLevel <= cfg.MaxSwimSpeedLevel)
                    saveData.SwimSpeed_XP += __instance.movementSpeed * cfg.XP_Multiplier;
            }
            else if (__instance.motorMode == Player.MotorMode.Walk || __instance.motorMode == Player.MotorMode.Run)  //add xp to walk speed
            {
                if (saveData.WalkSpeedLevel <= cfg.MaxWalkSpeedLevel)
                    saveData.WalkSpeed_XP += __instance.movementSpeed * cfg.XP_Multiplier;
            }

            Player.main.playerController.SetMotorMode(__instance.motorMode);
        }


        static float nextHealth;// = 0f;
        public void UpdateHealth(Player __instance)
        {
            if (__instance.liveMixin.IsFullHealth()) return;

            if (Time.time > nextHealth)
            {
                nextHealth = Time.time + 0.5f;//1f;
                saveData.Health_XP += StatMgr.AddXP(__instance.liveMixin.health, __instance.liveMixin.maxHealth);
            }
            
            Health.UpdateHealth(__instance);
        }


        static float nextSuffocate;// = 0f;
        public void UpdateSuffocation(Player __instance)
        {
            if(__instance.GetOxygenAvailable() > 3) return;

            
            if (Time.time > nextSuffocate)
            {
                nextSuffocate = Time.time + 1f;
                saveData.SuffocateResist_XP += (1f * cfg.XP_Multiplier);
            }
            
            Suffocation.UpdateSuffocation(__instance);
        }

    }

    [HarmonyPatch(typeof(Player))]
    [HarmonyPatch("OnKill")]
    class Player_OnKill_Patch
    {
        [HarmonyPostfix]
        public static void Postfix()
        {
            if (Guard.IsGamePaused())
                return;

            
            Log.InGameMSG("All the XP you have gained since your last save is lost");
            SaveData.GetSaveData(true);
        }
    }


    [HarmonyPatch(typeof(Player))]
    [HarmonyPatch("GetBreathPeriod")]
    class Player_GetBreathPeriod_Patch
    {
        static float nextXP;

        [HarmonyPostfix]
        public static void Postfix(Player __instance, ref float __result)
        {
            if (Guard.IsGamePaused() || __instance.CanBreathe() || __result == 99999) return;

            if (Time.time > nextXP)
            {
                SaveData.GetSaveData().BreathPeriod_XP += Air.AddXP(__instance.oxygenMgr.GetOxygenAvailable(), __instance.oxygenMgr.GetOxygenCapacity());
                nextXP = Time.time + 1f;
            }

            __result = Air.UpdateBreathPeriod(__instance, ref __result);
            return;
        }
    }




    [HarmonyPatch(typeof(Player))]
    [HarmonyPatch("GetOxygenPerBreath")]
    class Player_GetOxygenPerBreath_Patch
    {
        static float lastBreathInterval;
        [HarmonyPrefix]
        public static bool Prefix(float breathingInterval)
        {
            if (Guard.IsGamePaused()) return true;
            lastBreathInterval = breathingInterval;

            return true;
        }

        
        static float nextXP;
        [HarmonyPostfix]
        public static void Postfix(Player __instance, ref float __result)//, float __state)
        {
            if (Guard.IsGamePaused() || __instance.CanBreathe() || __result == 0) return;

            __result = Air.UpdateOxygenPerBreath(__instance, ref __result, lastBreathInterval);
            return;
        }
    }
}
