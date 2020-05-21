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

            __instance.forwardMaxSpeed += currentBoost;
            __instance.backwardMaxSpeed += currentBoost;
            __instance.acceleration += currentBoost;
            __instance.strafeMaxSpeed += currentBoost;

            /*__instance.forwardMaxSpeed = baseValues[0] + currentBoost;
            __instance.backwardMaxSpeed = baseValues[1] + currentBoost;
            __instance.acceleration = baseValues[2] + currentBoost;
            __instance.strafeMaxSpeed = baseValues[3] + currentBoost;*/
        }



        #region Swim Speed stuff
        public static void UpdateSwimSpeed()
        {
            var __instance = Player.main.playerController;
            SetSpeed setSpeed = new SetSpeed();
            UpdatePlayerController(__instance.underWaterController, saveData.SwimSpeedLevel, setSpeed.swimBaseValues);

            StatObject stat = new StatObject()
            {
                Name = "Swim Speed",
                Level = saveData.SwimSpeedLevel,
                MaxLevel = cfg.MaxSwimSpeedBoost,
                XP = saveData.SwimSpeed_XP,
                XPToNextLevel = saveData.SwimSpeed_XPToNextLevel,
                Modifier = cfg.SwimXP_Modifier
            };

            if (!StatMgr.CanLevelUp(stat)) return;

            int gainedLevels = StatMgr.DoWhileLevelUp(stat);
            StatMgr.NotifyLevelUp(stat, gainedLevels);

            saveData.SwimSpeedLevel = stat.Level;
            saveData.SwimSpeed_XP = stat.XP;
            saveData.SwimSpeed_XPToNextLevel = stat.XPToNextLevel;
            SaveData.Save_SaveFile();

            UpdatePlayerController(__instance.underWaterController, saveData.SwimSpeedLevel, setSpeed.swimBaseValues);
        }
        #endregion


        #region Walk Speed stuff
        public static void UpdateWalkSpeed()
        {
            var __instance = Player.main.playerController;
            SetSpeed setSpeed = new SetSpeed();

            UpdatePlayerController(__instance.groundController, saveData.WalkSpeedLevel, setSpeed.walkBaseValues);

            StatObject stat = new StatObject()
            {
                Name = "Walk Speed",
                Level = saveData.WalkSpeedLevel,
                MaxLevel = cfg.MaxWalkSpeedBoost,
                XP = saveData.WalkSpeed_XP,
                XPToNextLevel = saveData.WalkSpeed_XPToNextLevel,
                Modifier = cfg.WalkXP_Modifier
            };

            if (!StatMgr.CanLevelUp(stat)) return;

            int gainedLevels = StatMgr.DoWhileLevelUp(stat);
            StatMgr.NotifyLevelUp(stat, gainedLevels);

            saveData.WalkSpeedLevel = stat.Level;
            saveData.WalkSpeed_XP = stat.XP;
            saveData.WalkSpeed_XPToNextLevel = stat.XPToNextLevel;
            SaveData.Save_SaveFile();

            UpdatePlayerController(__instance.groundController, saveData.WalkSpeedLevel, setSpeed.walkBaseValues);
        }

        #endregion
    }  
}
