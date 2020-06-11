using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using UnityEngine;

namespace RPG_Framework
{
    class CreatureClass
    {
        private static CreatureClass creatureClass;
        public Dictionary<string, int[]> MeatCount = new Dictionary<string, int[]>()
        {
            {"shocker", new int[] { 7, 14 } },
            {"biter", new int[] { 0, 2 } },
            {"blighter", new int[] { 0, 2 } },
            {"boneshark", new int[] { 7, 12 } },
            {"crabsnake", new int[] { 6, 10 } },
            {"crabsquid", new int[] { 8, 14 } },
            {"lavalizard", new int[] { 6, 12 } },
            {"mesmer", new int[] { 1, 2 } },
            {"spineeel", new int[] { 7, 16 } },
            {"sandshark", new int[] { 3, 5 } },
            {"stalker", new int[] { 3, 5 } },
            {"warper", new int[] { 6, 9 } },


            {"ghostrayred", new int[] { 6, 13 } },
            {"cutefish", new int[] { 1, 4 } },
            {"gasopod", new int[] { 3, 6 } },
            {"ghostrayblue", new int[] { 4, 7 } },
            {"jellyray", new int[] { 3, 6 } },
            {"rabbitray", new int[2] { 2, 4 } },
            {"bleeder", new int[] { 0, 1 } },
            {"floater", new int[] { 0, 1 } },
            {"lavalarva", new int[] { 1, 3 } },
            {"jumper", new int[] { 0, 1 } },
            
            
            {"ghostleviathan", new int[] { 25, 40 } },
            {"ghostleviathanjuvenile", new int[] { 14, 21 } },
            {"reaperleviathan", new int[] { 18, 30 } },
            {"reefback", new int[] { 120, 160 } },
            {"seadragon", new int[] { 100, 150 } },
            {"seatreader", new int[] { 50, 80 } }
        };

        public static int[] GetMeatDropCount(string creatureName)
        {
            if(creatureClass == null) creatureClass = new CreatureClass();
            return creatureClass.MeatCount[creatureName.ToLower()];
        }


        /// <summary>
        /// Removes all extra charectes but the actual creatures name
        /// </summary>
        /// <param name="__instance">Creature __instance</param>
        /// <returns>Cleaned creature name</returns>
        public static string GetName(GameObject __instance)
        {
            return __instance.name.Split('(')[0];
        }


        /// <summary>
        /// Removes all extra charectes but the actual creatures name
        /// </summary>
        /// <param name="__instance">Creature __instance</param>
        /// <returns>Cleaned creature name</returns>
        public static string GetName(Creature __instance)
        {
            return __instance.gameObject.name.Split('(')[0];
        }
        

        /// <summary>
        /// Removes all extra charectes but the actual creatures name
        /// </summary>
        /// <param name="__instance">CreatureDeath __instance</param>
        /// <returns>Cleaned creature name</returns>
        public static string GetName(CreatureDeath __instance)
        {
            return __instance.gameObject.name.Split('(')[0];
        }
    }
}
