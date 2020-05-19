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
    class SetSpeed
    {
        [HarmonyPostfix]
        public static void PostFix(PlayerController __instance)
        {
            SetSpeed setSpeed = new SetSpeed();
            if(Player.main.motorMode == Player.MotorMode.Dive)
            {
                setSpeed.UpdateSwimSpeed(__instance);
            }

            else if (Player.main.motorMode == Player.MotorMode.Run || Player.main.motorMode == Player.MotorMode.Walk)
            {
                float speed = (SaveData.GetSaveData().LandDistanceTravelled * Config.GetConfig().LandSpeedBoost_Modifier)
                + __instance.groundController.backwardMaxSpeed; //using backspeed instead because it's higher

                if (speed > Config.GetConfig().MaxLandSpeed)
                    speed = Config.GetConfig().MaxLandSpeed - __instance.groundController.backwardMaxSpeed; //subtracting for same reason as above

                __instance.groundController.forwardMaxSpeed += speed;
                __instance.groundController.backwardMaxSpeed += speed;
                __instance.groundController.acceleration += speed;
                __instance.groundController.strafeMaxSpeed += speed;
            }

        }

        public void UpdateSwimSpeed(PlayerController __instance)
        {
            float boost = (SaveData.GetSaveData().SwimDistanceTravelled * Config.GetConfig().SwimSpeedBoost_Modifier);
            float baseSpeed = __instance.underWaterController.forwardMaxSpeed;

            if (boost + baseSpeed > Config.GetConfig().MaxSwimSpeed)
                boost = Config.GetConfig().MaxSwimSpeed - baseSpeed;

            __instance.underWaterController.forwardMaxSpeed += boost;
            __instance.underWaterController.backwardMaxSpeed += boost;
            __instance.underWaterController.acceleration += boost;
            __instance.underWaterController.strafeMaxSpeed += boost;


            if (baseSpeed + SaveData.GetSaveData().SwimSpeed_NextPassiveIncrease > Config.GetConfig().MaxSwimSpeed)
            {
                SaveData.GetSaveData().SwimSpeed_PassiveIncrease = Config.GetConfig().MaxSwimSpeed - baseSpeed;
                return;
            }
            SaveData.GetSaveData().SwimSpeed_PassiveIncrease = boost;


            if (StatNotify.HasStatIncrease(boost, SaveData.GetSaveData().SwimSpeed_NextPassiveIncrease))
            {
                float increase = (float)Math.Truncate((boost + 1) - SaveData.GetSaveData().SwimSpeed_NextPassiveIncrease);

                StatNotify.NotifyStatIncrease("Swim Speed", increase, SaveData.GetSaveData().SwimSpeed_PassiveIncrease);
                SaveData.GetSaveData().SwimSpeed_NextPassiveIncrease++;

                if (SaveData.GetSaveData().SwimSpeed_NextPassiveIncrease + baseSpeed
                    > Config.GetConfig().MaxSwimSpeed)
                    Log.InGameMSG("You've reached max level for your swim speed");
            }
        }
    }



    [HarmonyPatch(typeof(Player))]
    [HarmonyPatch("Update")]
    class IncreaseSavedSpeed
    {
        [HarmonyPostfix]
        public static void PostFix(Player __instance)
        {
            if (Guard.IsGamePaused())
                return;

            if (__instance.motorMode == Player.MotorMode.Dive)  //do underwater stuff
            {
                SaveData.GetSaveData().SwimDistanceTravelled += __instance.movementSpeed;
                //Log.InGameMSG("dist: " + SaveData.GetSaveData().SwimDistance);    //Temporary. Being used to test if paused
            }
            else if(__instance.motorMode == Player.MotorMode.Walk || __instance.motorMode == Player.MotorMode.Run)  //do on land stuff
                SaveData.GetSaveData().LandDistanceTravelled += __instance.movementSpeed;

            Player.main.playerController.SetMotorMode(__instance.motorMode);
        }
    }

    
}
