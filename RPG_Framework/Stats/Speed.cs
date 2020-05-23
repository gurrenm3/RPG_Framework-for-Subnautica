using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using UnityEngine;

namespace RPG_Framework.Stats
{
    [HarmonyPatch(typeof(PlayerController))]
    [HarmonyPatch("SetMotorMode")]
    class SetSpeed
    {
        private static Config cfg = Config.GetConfig();
        private static SaveData saveData;

        //order is: forward, back, strafe, accel
        List<float> swimBaseValues = new List<float> { 5f, 5f, 5f, 5f };

        //order is: forward, back, strafe, accel
        List<float> walkBaseValues = new List<float> { 3.5f, 5f, 5f, 5f };

        [HarmonyPostfix]
        public static void PostFix(PlayerController __instance)
        {
            if (Guard.IsGamePaused())
                return;

            saveData = SaveData.GetSaveData();

            if (Player.main.motorMode == Player.MotorMode.Dive)
                UpdateSwimSpeed();

            else if (Player.main.motorMode == Player.MotorMode.Run || Player.main.motorMode == Player.MotorMode.Walk)
                UpdateWalkSpeed();

        }
        public static void UpdatePlayerController(PlayerMotor __instance, int currentBoost, List<float> baseValues)
            => UpdatePlayerController(__instance, currentBoost, baseValues, -999);

        public static void UpdatePlayerController(PlayerMotor __instance, int currentBoost, List<float> baseValues, int max)
        {
            if (max != -999) if(currentBoost > max) currentBoost = max;

            float boost = IncrementSpeedBoost(__instance, currentBoost);

            __instance.forwardMaxSpeed += boost;
            __instance.backwardMaxSpeed += boost;
            __instance.acceleration += boost;
            __instance.strafeMaxSpeed += boost;
        }

        static float dontAddBoostTime;
        static float nextBoostTime;
        static float boostCount;
        public static float IncrementSpeedBoost(PlayerMotor __instance, float currentBoost)
        {
            float tempBoost = 0f;
            float stallTime = 0.9f;
            float timeBetweenBoosts = 0.06f;

            if (currentBoost >= 1) tempBoost = 1f;

            if (Player.main.movementSpeed < 1)
            {
                boostCount = tempBoost;
                dontAddBoostTime = Time.time + stallTime;
                return tempBoost;
            }

            if (Time.time < dontAddBoostTime)
            {
                boostCount = tempBoost;
                return tempBoost;
            }

            if (boostCount < currentBoost)
            {
                if (Time.time > nextBoostTime)
                {
                    boostCount++;
                    nextBoostTime = Time.time + timeBetweenBoosts;
                    return boostCount;
                }
                return boostCount;
            }
            else
                return currentBoost;
        }



        #region Swim Speed stuff
        public static void UpdateSwimSpeed()
        {
            var __instance = Player.main.playerController;
            SetSpeed setSpeed = new SetSpeed();

            StatObject stat = new StatObject()
            {
                Name = "Swim Speed",
                Level = saveData.SwimSpeedLevel,
                MaxLevel = cfg.MaxSwimSpeedLevel,
                XP = saveData.SwimSpeed_XP,
                XPToNextLevel = saveData.SwimSpeed_XPToNextLevel,
                Modifier = cfg.Swim_XPNextLevel_Multiplier
            };

            if (!StatMgr.CanLevelUp(stat))
            {
                UpdatePlayerController(__instance.underWaterController, saveData.SwimSpeedLevel, setSpeed.swimBaseValues);
                return;
            }

            int gainedLevels = StatMgr.DoWhileLevelUp(stat);
            StatMgr.NotifyLevelUp(stat, gainedLevels);

            saveData.SwimSpeedLevel = stat.Level;
            saveData.SwimSpeed_XP = stat.XP;
            saveData.SwimSpeed_XPToNextLevel = stat.XPToNextLevel;

            UpdatePlayerController(__instance.underWaterController, saveData.SwimSpeedLevel, setSpeed.swimBaseValues);
        }
        #endregion


        #region Walk Speed stuff
        public static void UpdateWalkSpeed()
        {
            var __instance = Player.main.playerController;
            SetSpeed setSpeed = new SetSpeed();

            StatObject stat = new StatObject()
            {
                Name = "Walk Speed",
                Level = saveData.WalkSpeedLevel,
                MaxLevel = cfg.MaxWalkSpeedLevel,
                XP = saveData.WalkSpeed_XP,
                XPToNextLevel = saveData.WalkSpeed_XPToNextLevel,
                Modifier = cfg.Walk_XPNextLevel_Multiplier
            };

            if (!StatMgr.CanLevelUp(stat))
            {
                UpdatePlayerController(__instance.groundController, saveData.WalkSpeedLevel, setSpeed.walkBaseValues);
                return;
            }

            int gainedLevels = StatMgr.DoWhileLevelUp(stat);
            StatMgr.NotifyLevelUp(stat, gainedLevels);

            saveData.WalkSpeedLevel = stat.Level;
            saveData.WalkSpeed_XP = stat.XP;
            saveData.WalkSpeed_XPToNextLevel = stat.XPToNextLevel;

            UpdatePlayerController(__instance.groundController, saveData.WalkSpeedLevel, setSpeed.walkBaseValues);
        }

        #endregion
    }  
}
