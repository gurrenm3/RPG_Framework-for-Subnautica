using Harmony;
using RPG_Framework.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPG_Framework
{
    [HarmonyPatch(typeof(Player))]
    [HarmonyPatch("OnTakeDamage")]
    class Player_UpdatePrefix
    {
        [HarmonyPrefix]
        public static bool Prefix(Player __instance, DamageInfo damageInfo)
        {
            /*if (Guard.IsGamePaused())
                return true;*/

            //Log.InGameMSG("Prefix ");
            UpdateResistance(__instance, damageInfo);
            return true;
        }
        public static void UpdateResistance(Player __instance, DamageInfo damageInfo)
        {
            DamageResistance.ApplyResistance(__instance, damageInfo);
        }
    }


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
            

            //pUpdate.UpdateOxygen(__instance);
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

            saveData.Health_XP += StatMgr.AddXP(__instance.liveMixin.health, __instance.liveMixin.maxHealth);
            Health.UpdateHealth(__instance);
        }


        public void UpdateFood(Player __instance)
        {
            /*saveData.Food_XP += StatMgr.AddXP(__instance);
            Food.UpdateFood(__instance);*/
        }

        public void UpdateOxygen(Player __instance)
        {
            //if (!__instance.IsUnderwater() || __instance.oxygenMgr.GetOxygenAvailable() >= __instance.oxygenMgr.GetOxygenCapacity()) return;
            
            Air.UpdateOxygen(__instance);            
        }
    }
}
