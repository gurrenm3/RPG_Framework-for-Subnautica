using HarmonyLib;
using RPG_Framework.Stats.player;
using UnityEngine;

namespace RPG_Framework.Patches.player
{
    [HarmonyPatch(typeof(Player), nameof(Player.GetBreathPeriod))]
    class Player_GetBreathPeriod_Patch
    {
        static float nextXP;

        [HarmonyPostfix]
        internal static void Postfix(Player __instance, ref float __result)
        {
            if (Guard.IsGamePaused() || __instance.CanBreathe() || __result == 99999) return;

            if (Time.time > nextXP)
            {
                if (SaveData.GetSaveData().BreathPeriod_XP < 0)
                    SaveData.GetSaveData().BreathPeriod_XP = SaveData.GetSaveData().BreathPeriod_XPToNextLevel / 2;

                SaveData.GetSaveData().BreathPeriod_XP += Air.AddXP(__instance.oxygenMgr.GetOxygenAvailable(), __instance.oxygenMgr.GetOxygenCapacity());
                nextXP = Time.time + 1f;
            }

            __result = Air.UpdateBreathPeriod(__instance, ref __result);
            return;
        }
    }
}
