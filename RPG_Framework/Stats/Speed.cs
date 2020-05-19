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

        //order is forward, back, strafe, accel
        List<float> swimBaseValues = new List<float> { 5f, 5f, 5f, 5f };

        //order is forward, back, strafe, accel
        List<float> walkBaseValues = new List<float> { 3.5f, 5f, 5f, 5f };

        [HarmonyPostfix]
        public static void PostFix(PlayerController __instance)
        {
            if (Guard.IsGamePaused())
                return;

            if(Player.main.motorMode == Player.MotorMode.Dive)
            {
                UpdateSwimSpeed();
            }

            else if (Player.main.motorMode == Player.MotorMode.Run || Player.main.motorMode == Player.MotorMode.Walk)
            {
                //setSpeed.UpdateLandSpeed(__instance);
            }

        }
        public static void UpdatePlayerController(PlayerMotor __instance, int currentBoost, List<float> baseValues)
        {
            __instance.forwardMaxSpeed = baseValues[0] + currentBoost;
            __instance.backwardMaxSpeed = baseValues[1] + currentBoost;
            __instance.acceleration = baseValues[2] + currentBoost;
            __instance.strafeMaxSpeed = baseValues[3] + currentBoost;
        }



        #region Swim Speed stuff
        public static void UpdateSwimSpeed()
        {
            var __instance = Player.main.playerController;
            SetSpeed setSpeed = new SetSpeed();

            /*Log.InGameMSG("Current magnitude is: " + __instance.velocity.magnitude);
            Log.InGameMSG("Current SPEED/5 is: " + __instance.underWaterController.);*/
            UpdatePlayerController(__instance.underWaterController, saveData.SwimSpeedLevel, setSpeed.swimBaseValues);

            if (saveData.SwimSpeedLevel >= cfg.MaxSwimSpeedBoost) return;
            if (!XP_Handler.DoesHaveLevelUp(saveData.SwimSpeed_XP, saveData.SwimSpeed_XPToNextLevel))
                return;

            int gainedLevels = 0;
            while (saveData.SwimSpeed_XP >= saveData.SwimSpeed_XPToNextLevel)
            {
                if (saveData.SwimSpeedLevel >= cfg.MaxSwimSpeedBoost) break;
                gainedLevels++;
                saveData.SwimSpeedLevel++;
                saveData.SwimSpeed_XP -= saveData.SwimSpeed_XPToNextLevel;
                if (saveData.SwimSpeed_XP < 0) saveData.SwimSpeed_XP = 0;

                saveData.SwimSpeed_XPToNextLevel = XP_Handler.CalcXPToNextLevel(saveData.SwimSpeed_XPToNextLevel, cfg.SwimXP_Modifier);
            }
            SaveData.Save_SaveFile();
            XP_Events.NotifyStatIncrease("Swim Speed", gainedLevels, saveData.SwimSpeedLevel);
            if (saveData.SwimSpeedLevel >= cfg.MaxSwimSpeedBoost)
                Log.InGameMSG("Swim Speed has reached max level");
        }

        
        #endregion

        /*#region Swim Speed stuff
        public void UpdateSwimSpeedaaa(PlayerController __instance)
        {
            float boost = XP_Handler.CalcStatBoost(saveData.SwimDistanceTravelled, cfg.SwimSpeedBoost_Modifier);
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
        
        #endregion*/
    }



    [HarmonyPatch(typeof(Player))]
    [HarmonyPatch("Update")]
    class IncreaseSavedSpeed
    {
        private static SaveData saveData = SaveData.GetSaveData();
        private static Config cfg = Config.GetConfig();
        [HarmonyPostfix]
        public static void PostFix(Player __instance)
        {
            if (Guard.IsGamePaused())
                return;

            if (__instance.motorMode == Player.MotorMode.Dive)
            {
                if (saveData.SwimSpeedLevel >= cfg.MaxSwimSpeedBoost) return;

                saveData.SwimDistanceTravelled += __instance.movementSpeed;
                saveData.SwimSpeed_XP += __instance.movementSpeed;
            }
            /*else if(__instance.motorMode == Player.MotorMode.Walk || __instance.motorMode == Player.MotorMode.Run)  //do on land stuff
                saveData.LandDistanceTravelled += __instance.movementSpeed;*/

            Player.main.playerController.SetMotorMode(__instance.motorMode);
        }
    }

    
}
