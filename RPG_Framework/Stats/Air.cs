using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPG_Framework.Stats
{
    class Air
    {
        private static SaveData saveData = SaveData.GetSaveData();
        private static Config cfg = Config.GetConfig();

        //float defaultMaxHealth = 100f;

        /*public static float AddXP(Player __instance)
        {
            float addXP = 0f;
            float percentEmpty = (__instance.oxygenMgr. - __instance.liveMixin.health) / 100;

            float multiplier = 1;
            if (percentEmpty >= 33 && percentEmpty < 66) multiplier = 1.35f;
            else if (percentEmpty >= 66 && percentEmpty < 90) multiplier = 1.75f;
            else if (percentEmpty >= 90 && percentEmpty < 95) multiplier = 2.25f;
            else if (percentEmpty >= 95 && percentEmpty < 100) multiplier = 3;

            addXP = percentEmpty * multiplier / 10;
            return addXP;
        }*/

        public static void UpdateOxygen(Player __instance)
        {
            /*//float baseMaxO2 = __instance.oxygenMgr.GetOxygenCapacity();
            //__instance.liveMixin.data.maxHealth = h.defaultMaxHealth + saveData.HealthBonusLevel;

            Log.InGameMSG("Checking sources...");


            //Oxygen a = new Oxygen();

            //a.name = "StatO2";
            //a.oxygenAvailable = 500;
            //a.oxygenCapacity = 500;

            //__instance.oxygenMgr.RegisterSource(a);
            //var sources = (List<Oxygen>)typeof(OxygenManager).GetField("sources").GetValue(__instance.oxygenMgr);
            
            
            
            
            
            
            //var sources2 = (List<Oxygen>)typeof(OxygenManager).GetField("sources").GetValue(__instance);
            *//*if (sources == null)
            {
                Log.InGameMSG("Sources is null");
                
                if(sources2 == null)
                    Log.InGameMSG("Sources2 == null");
                else
                    Log.InGameMSG("Sources2 is not null");
                return;
            }

            Log.InGameMSG("Count1: " + sources.Count());
            Log.InGameMSG("Count2: " + sources2.Count());

            Log.InGameMSG("Now on souces");
            foreach (var a in sources)
            {
                Log.InGameMSG("Capacity1: " +a.oxygenCapacity.ToString());
            }*//*

            Log.InGameMSG("Now on souces2 ");
            *//*foreach (var a in sources2)
            {
                Log.InGameMSG("Capacity2: " + a.oxygenCapacity.ToString());
            }*//*
            return;
            StatObject stat = new StatObject()
            {
                Name = "Max Air",
                Level = saveData.AirBonusLevel,
                MaxLevel = cfg.MaxAirBoost,
                XP = saveData.Air_XP,
                XPToNextLevel = saveData.Air_XPToNextLevel,
                Modifier = cfg.AirXP_Modifier
            };

            if (!StatMgr.CanLevelUp(stat)) return;

            int gainedLevels = StatMgr.DoWhileLevelUp(stat);
            StatMgr.NotifyLevelUp(stat, gainedLevels);

            saveData.AirBonusLevel = stat.Level;
            saveData.Air_XP = stat.XP;
            saveData.Air_XPToNextLevel = stat.XPToNextLevel;
            SaveData.Save_SaveFile();*/
        }
    }
}
