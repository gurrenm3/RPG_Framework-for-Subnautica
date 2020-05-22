using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPG_Framework.Stats
{
    [HarmonyPatch(typeof(Knife))]
    [HarmonyPatch("IsValidTarget")]
    class Knife_IsValidTarget_Patch
    {
        [HarmonyPostfix]
        public static void Postfix(LiveMixin liveMixin, bool __result)
        {
            if (Guard.IsGamePaused()) return;

            /*Log.InGameMSG("Current Health: " +liveMixin.health.ToString());
            Log.InGameMSG("Max Health: " +liveMixin.maxHealth.ToString());*/
        }
    }
}
