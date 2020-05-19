﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;

namespace RPG_Framework
{
    class XP_Events
    {
        public static void LevelUpStats(int currentLevel, float currentXPtoNextxLevel)
        {
            //float newXPtoNextLevel = XP_Handler.CalcXPToNextLevel(currentLevel, currentXPtoNextxLevel);
            
        }

        public static void AddExperience(float amount)
        {
            SaveData.GetSaveData().PlayerXP += Config.GetConfig().XP_Multiplier;
            SaveData.Save_SaveFile();
        }

        public static void NotifyStatIncrease(string stat, float amount, int currentLevel)
        {
            Log.InGameMSG(stat + " has increased by " + amount + ". Current level: " + currentLevel);
        }


        #region Harmony Events
        [HarmonyPatch(typeof(CreatureDeath))]
        [HarmonyPatch("OnKill")]
        class CreatureDeath_OnKill
        {
            [HarmonyPostfix]
            public static void Postfix(CreatureDeath __instance)
            {
                AddExperience(__instance.liveMixin.maxHealth * Config.GetConfig().OnKillcreatureKillXP_Modifier);
                Player.main.playerController.SetMotorMode(Player.main.motorMode);
            }
        }

        #endregion
    }
}