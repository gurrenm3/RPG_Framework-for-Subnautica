using HarmonyLib;
using RPG_Framework.Stats.player;
using UnityEngine;
using RPG_Framework.Extensions;
using RPG_Framework.Stats;

namespace RPG_Framework.Patches.player
{
    [HarmonyPatch(typeof(Player), nameof(Player.Update))]
    class Player_Update
    {
        private static SaveData saveData;
        private static Config cfg = Config.GetConfig();
        private static Player_Update pUpdate = new Player_Update();

        [HarmonyPostfix]
        internal static void PostFix(Player __instance)
        {
            if (Guard.IsGamePaused())
                return;

            cfg = Config.GetConfig();
            saveData = SaveData.GetSaveData();

            RPGKeyPress.ProcessKeys();

            XP_Events.DoubleXPEvent();

            pUpdate.UpdateMovement(__instance);
            pUpdate.UpdateHealth(__instance);
            pUpdate.UpdateSuffocation(__instance);

            Hints.CheckMSGs();
        }

        internal void UpdateMovement(Player __instance)
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



        private static float nextHealth;// = 0f;
        internal void UpdateHealth(Player __instance)
        {
            if (__instance.liveMixin.IsFullHealth()) return;

            if (Time.time > nextHealth)
            {
                nextHealth = Time.time + 0.5f;//1f;
                saveData.Health_XP += StatMgr.AddXP(__instance.liveMixin.health, __instance.liveMixin.maxHealth);
            }

            Health.UpdateHealth(__instance);
        }


        private static float nextSuffocate;// = 0f;
        internal void UpdateSuffocation(Player __instance)
        {
            if (__instance.GetOxygenAvailable() > 3) return;


            if (Time.time > nextSuffocate)
            {
                nextSuffocate = Time.time + 1f;
                saveData.SuffocateResist_XP += (1f * cfg.XP_Multiplier);
            }

            Suffocation.UpdateSuffocation(__instance);
        }
    }
}
