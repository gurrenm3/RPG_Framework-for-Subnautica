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
        private static Config cfg = Config.GetConfig();
        private static SaveData saveData = SaveData.GetSaveData();

        const float BaseForwardSwimSpeed = 5f;
        const float BaseBackSwimSpeed = 5f;
        const float BaseStrafeSwimSpeed = 5f;
        const float BaseAccelSwimSpeed = 5f;

        const float BaseForwardLandSpeed = 3.5f;
        const float BaseBackLandSpeed = 5f;
        const float BaseStrafeLandSpeed = 5f;
        const float BaseAccelLandSpeed = 5f;

        [HarmonyPostfix]
        public static void PostFix(PlayerController __instance)
        {
            if (Guard.IsGamePaused())
                return;

            SetSpeed setSpeed = new SetSpeed();
            if(Player.main.motorMode == Player.MotorMode.Dive)
            {
                setSpeed.UpdateSwimSpeed(__instance);
            }

            else if (Player.main.motorMode == Player.MotorMode.Run || Player.main.motorMode == Player.MotorMode.Walk)
            {
                setSpeed.UpdateLandSpeed(__instance);
            }

        }
        public bool NotifyStatIncrease(string statName, float baseSpeed, float boost, float currentLevel, float nextLevel)
        {
            float increase = (float)Math.Truncate((boost + 1) - nextLevel);
            StatNotify.NotifyStatIncrease(statName, increase, currentLevel);

            if (nextLevel + baseSpeed > cfg.MaxSwimSpeed)
            {
                Log.InGameMSG("Your " + statName + " is max level");
                return false;
            }

            return true;
        }

        #region Swim Speed stuff
        public void UpdateSwimSpeed(PlayerController __instance)
        {
            float boost = AddXP.CalcStatBoost(saveData.SwimDistanceTravelled, cfg.SwimSpeedBoost_Modifier);
            //float boost = (saveData.SwimDistanceTravelled * cfg.SwimSpeedBoost_Modifier);
            float baseSpeed = BaseForwardSwimSpeed;

            if (boost + baseSpeed > cfg.MaxSwimSpeed)
                boost = cfg.MaxSwimSpeed - baseSpeed;

            __instance.underWaterController.forwardMaxSpeed = BaseForwardSwimSpeed + boost;
            __instance.underWaterController.backwardMaxSpeed = BaseBackSwimSpeed + boost;
            __instance.underWaterController.acceleration = BaseAccelSwimSpeed + boost;
            __instance.underWaterController.strafeMaxSpeed = BaseStrafeSwimSpeed + boost;


            if (baseSpeed + saveData.SwimSpeed_NextPassiveIncrease > cfg.MaxSwimSpeed)
            {
                saveData.SwimSpeed_PassiveIncrease = cfg.MaxSwimSpeed - baseSpeed;
                return;
            }
            saveData.SwimSpeed_PassiveIncrease = boost;

            if (StatNotify.HasStatIncrease(boost, saveData.SwimSpeed_NextPassiveIncrease))
            {
                if (NotifyStatIncrease("Swim Speed", BaseForwardSwimSpeed, boost,
                    saveData.SwimSpeed_PassiveIncrease, saveData.SwimSpeed_NextPassiveIncrease))
                    saveData.SwimSpeed_NextPassiveIncrease++;
            }
            //SwimSpeedNotify(boost);
        }
        #endregion


        #region Land Speed stuff
        public void UpdateLandSpeed(PlayerController __instance)
        {
            float boost = (saveData.LandDistanceTravelled * cfg.LandSpeedBoost_Modifier);
            float baseSpeed = BaseForwardLandSpeed;

            if (boost + baseSpeed > cfg.MaxLandSpeed)
                boost = cfg.MaxLandSpeed - baseSpeed;

            __instance.groundController.forwardMaxSpeed = BaseForwardLandSpeed + boost;
            __instance.groundController.backwardMaxSpeed = BaseBackLandSpeed + boost;
            __instance.groundController.acceleration = BaseAccelLandSpeed + boost;
            __instance.groundController.strafeMaxSpeed = BaseStrafeLandSpeed + boost;


            if (baseSpeed + saveData.LandSpeed_NextPassiveIncrease > cfg.MaxLandSpeed)
            {
                saveData.LandSpeed_PassiveIncrease = cfg.MaxLandSpeed - baseSpeed;
                return;
            }
            saveData.LandSpeed_PassiveIncrease = boost;

            if (StatNotify.HasStatIncrease(boost, saveData.LandSpeed_NextPassiveIncrease))
            {
                if(NotifyStatIncrease("Walk Speed", BaseForwardLandSpeed, boost, 
                    saveData.LandSpeed_PassiveIncrease, saveData.LandSpeed_NextPassiveIncrease))
                    saveData.LandSpeed_NextPassiveIncrease++;
            }
            
        }
        
        #endregion
    }



    [HarmonyPatch(typeof(Player))]
    [HarmonyPatch("Update")]
    class IncreaseSavedSpeed
    {
        private static SaveData saveData = SaveData.GetSaveData();
        [HarmonyPostfix]
        public static void PostFix(Player __instance)
        {
            if (Guard.IsGamePaused())
                return;

            if (__instance.motorMode == Player.MotorMode.Dive)  //do underwater stuff
            {
                saveData.SwimDistanceTravelled += __instance.movementSpeed;
                //Log.InGameMSG("dist: " + SaveData.GetSaveData().SwimDistance);    //Temporary. Being used to test if paused
            }
            else if(__instance.motorMode == Player.MotorMode.Walk || __instance.motorMode == Player.MotorMode.Run)  //do on land stuff
                saveData.LandDistanceTravelled += __instance.movementSpeed;

            Player.main.playerController.SetMotorMode(__instance.motorMode);
        }
    }

    
}
