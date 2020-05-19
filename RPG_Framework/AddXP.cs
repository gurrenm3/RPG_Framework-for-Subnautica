using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using RPG_Framework.LevelUp;

namespace RPG_Framework
{
    public class AddXP
    {
        public static void OnCreatureKilled(CreatureDeath __instance)
        {
            AddExperience(__instance.liveMixin.maxHealth * Config.GetConfig().OnKillcreatureKillXP_Modifier);
            Player.main.playerController.SetMotorMode(Player.main.motorMode);
        }

        public static void AddExperience(float amount)
        {
            SaveData.GetSaveData().PlayerXP += Config.GetConfig().XP_Multiplier;
            SaveData.Save_SaveFile();
        }
    }
}
