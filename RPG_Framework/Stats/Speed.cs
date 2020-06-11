using Harmony;
using SMLHelper.V2.Utility;
using UnityEngine;

namespace RPG_Framework.Stats
{
    class Speed
    {
        private static Config cfg = Config.GetConfig();
        private static SaveData saveData;

        private static float dontAddBoostTime;
        private static float nextBoostTime;
        private static float boostCount;
        private static float stallTime = 1.5f;


        #region Swim Speed stuff
        internal void UpdateSpeed()
        {
            if (saveData == null)
                saveData = SaveData.GetSaveData();

            if (cfg == null)
                cfg = Config.GetConfig();


            if (Player.main.motorMode == Player.MotorMode.Dive)
                UpdateSwimSpeed();
            else if (Player.main.motorMode == Player.MotorMode.Run || Player.main.motorMode == Player.MotorMode.Walk)
                UpdateWalkSpeed();
            else
                ResetSpeedBoost();
        }


        private void UpdateSwimSpeed()
        {
            var __instance = Player.main.playerController;

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
                UpdatePlayerController(__instance.underWaterController, saveData.SwimSpeedLevel);
                return;
            }

            int gainedLevels = StatMgr.DoWhileLevelUp(stat);
            StatMgr.NotifyLevelUp(stat, gainedLevels);

            saveData.SwimSpeedLevel = stat.Level;
            saveData.SwimSpeed_XP = stat.XP;
            saveData.SwimSpeed_XPToNextLevel = stat.XPToNextLevel;

            UpdatePlayerController(__instance.underWaterController, saveData.SwimSpeedLevel);
        }
        #endregion


        #region Walk Speed stuff
        private void UpdateWalkSpeed()
        {
            var __instance = Player.main.playerController;
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
                UpdatePlayerController(__instance.groundController, saveData.WalkSpeedLevel);
                return;
            }

            int gainedLevels = StatMgr.DoWhileLevelUp(stat);
            StatMgr.NotifyLevelUp(stat, gainedLevels);

            saveData.WalkSpeedLevel = stat.Level;
            saveData.WalkSpeed_XP = stat.XP;
            saveData.WalkSpeed_XPToNextLevel = stat.XPToNextLevel;

            UpdatePlayerController(__instance.groundController, saveData.WalkSpeedLevel);
        }

        #endregion


        internal void ResetSpeedBoost()
        {
            boostCount = 0;
            dontAddBoostTime = Time.time + stallTime;
        }

        private void UpdatePlayerController(PlayerMotor __instance, int currentBoost)
            => UpdatePlayerController(__instance, currentBoost, -999);

        private void UpdatePlayerController(PlayerMotor __instance, int currentBoost, int max)
        {
            if (!Guard.CanUseSpeedBoost())
            {
                useMaxBoost = false;
                ResetSpeedBoost();
                return;
            }

            if (max != -999) if (currentBoost > max) currentBoost = max;

            float boost = IncrementSpeedBoost(currentBoost);
            if (DoUseMaxBoost())
                boost = currentBoost;

            __instance.forwardMaxSpeed += boost;
            __instance.backwardMaxSpeed += boost;
            __instance.acceleration += boost;
            __instance.strafeMaxSpeed += boost;
        }


        private static bool useMaxBoost = false;
        private bool DoUseMaxBoost()
        {
            if (GameInput.GetButtonDown(GameInput.Button.Sprint))
                useMaxBoost = true;

            if (GameInput.GetButtonUp(GameInput.Button.Sprint))
                useMaxBoost = false;

            return useMaxBoost;
        }
        private float IncrementSpeedBoost(float currentBoost)
        {
            float tempBoost = 0f;
            float timeBetweenBoosts = 0.1f;

            if (Player.main.movementSpeed < 1)
            {
                ResetSpeedBoost();
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

    }
}
