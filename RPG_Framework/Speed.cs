using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using System.Text;

namespace RPG_Framework.Stats
{
    [HarmonyPatch(typeof(PlayerController))]
    [HarmonyPatch("SetMotorMode")]
    class SetSwimSpeed
    {
        [HarmonyPostfix]
        public static void PostFix(PlayerController __instance)
        {
            __instance.underWaterController.forwardMaxSpeed += SaveData.GetSaveData().MaxForwardSwimSpeed + 20;
            __instance.underWaterController.backwardMaxSpeed += SaveData.GetSaveData().MaxForwardSwimSpeed + 20;
            __instance.underWaterController.acceleration += SaveData.GetSaveData().MaxForwardSwimSpeed + 20;
        }
    }

    [HarmonyPatch(typeof(Player))]
    [HarmonyPatch("Update")]
    class IncreaseSavedSpeed
    {
        [HarmonyPostfix]
        public static void PostFix(Player __instance)
        {
            if (__instance.motorMode == Player.MotorMode.Dive)  //do underwater stuff
            {
                SaveData.GetSaveData().SwimDistance += __instance.movementSpeed;
            }
            else if(__instance.motorMode == Player.MotorMode.Walk || __instance.motorMode == Player.MotorMode.Run)  //do on land stuff
            {
                
            }
        }
    }
}
