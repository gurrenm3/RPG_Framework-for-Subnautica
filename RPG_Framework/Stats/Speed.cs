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

        //order is: forward, back, strafe, accel
        List<float> swimBaseValues = new List<float> { 5f, 5f, 5f, 5f };

        //order is: forward, back, strafe, accel
        List<float> walkBaseValues = new List<float> { 3.5f, 5f, 5f, 5f };

        [HarmonyPostfix]
        public static void PostFix(PlayerController __instance)
        {
            if (Guard.IsGamePaused())
                return;

            if(Player.main.motorMode == Player.MotorMode.Dive)
                UpdateSwimSpeed();

            else if (Player.main.motorMode == Player.MotorMode.Run || Player.main.motorMode == Player.MotorMode.Walk)
                UpdateWalkSpeed();

        }
        public static void UpdatePlayerController(PlayerMotor __instance, int currentBoost, List<float> baseValues)
            => UpdatePlayerController(__instance, currentBoost, baseValues, -999);

        public static void UpdatePlayerController(PlayerMotor __instance, int currentBoost, List<float> baseValues, int max)
        {
            if (max != -999) if(currentBoost > max) currentBoost = max;

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
                Log.InGameMSG("Swim Speed is now max level");

            UpdatePlayerController(__instance.underWaterController, saveData.SwimSpeedLevel, setSpeed.swimBaseValues);
        }
        #endregion


        #region Walk Speed stuff
        public static void UpdateWalkSpeed()
        {
            var __instance = Player.main.playerController;
            SetSpeed setSpeed = new SetSpeed();

            UpdatePlayerController(__instance.groundController, saveData.WalkSpeedLevel, setSpeed.walkBaseValues);

            if (saveData.WalkSpeedLevel >= cfg.MaxWalkSpeedBoost) return;
            if (!XP_Handler.DoesHaveLevelUp(saveData.WalkSpeed_XP, saveData.WalkSpeed_XPToNextLevel))
                return;

            int gainedLevels = 0;
            while (saveData.WalkSpeed_XP >= saveData.WalkSpeed_XPToNextLevel)
            {
                if (saveData.WalkSpeedLevel >= cfg.MaxWalkSpeedBoost) break;
                gainedLevels++;
                saveData.WalkSpeedLevel++;
                saveData.WalkSpeed_XP -= saveData.WalkSpeed_XPToNextLevel;
                if (saveData.WalkSpeed_XP < 0) saveData.WalkSpeed_XP = 0;

                saveData.WalkSpeed_XPToNextLevel = XP_Handler.CalcXPToNextLevel(saveData.WalkSpeed_XPToNextLevel, cfg.WalkXP_Modifier);
            }
            SaveData.Save_SaveFile();
            XP_Events.NotifyStatIncrease("Walk Speed", gainedLevels, saveData.WalkSpeedLevel);
            if (saveData.WalkSpeedLevel >= cfg.MaxWalkSpeedBoost)
                Log.InGameMSG("Walk Speed is now max level");

            UpdatePlayerController(__instance.groundController, saveData.WalkSpeedLevel, setSpeed.walkBaseValues);
        }

        #endregion
    }  
}
