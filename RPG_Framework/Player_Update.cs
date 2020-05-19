using Harmony;
using RPG_Framework.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPG_Framework
{
    [HarmonyPatch(typeof(Player))]
    [HarmonyPatch("Update")]
    class Player_Update
    {
        private static SaveData saveData = SaveData.GetSaveData();
        private static Config cfg = Config.GetConfig();
        private static Player_Update pUpdate = new Player_Update();
        [HarmonyPostfix]
        public static void PostFix(Player __instance)
        {
            if (Guard.IsGamePaused())
                return;

            pUpdate.UpdateMovement(__instance);

            pUpdate.UpdateHealth(__instance);
        }

        public void UpdateMovement(Player __instance)
        {
            if (__instance.motorMode == Player.MotorMode.Dive)  //add xp to swim speed
            {
                if (saveData.SwimSpeedLevel >= cfg.MaxSwimSpeedBoost) return;

                saveData.SwimDistanceTravelled += __instance.movementSpeed;
                saveData.SwimSpeed_XP += __instance.movementSpeed;
            }
            else if (__instance.motorMode == Player.MotorMode.Walk || __instance.motorMode == Player.MotorMode.Run)  //add xp to walk speed
            {
                if (saveData.WalkSpeedLevel >= cfg.MaxWalkSpeedBoost) return;

                saveData.WalkDistanceTravelled += __instance.movementSpeed;
                saveData.WalkSpeed_XP += __instance.movementSpeed;
            }

            Player.main.playerController.SetMotorMode(__instance.motorMode);
        }

        public void UpdateHealth(Player __instance)
        {
            if (__instance.liveMixin.IsFullHealth()) return;

            saveData.Health_XP += Health.AddXP(__instance);
            Health.UpdateHealth(__instance);
        }
    }
}
