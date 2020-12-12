using HarmonyLib;
using RPG_Framework.Stats.player;

namespace RPG_Framework.Patches.player
{
    [HarmonyPatch(typeof(Player), nameof(Player.GetOxygenPerBreath))]
    class Player_GetOxygenPerBreath_Patch
    {
        static float lastBreathInterval;
        [HarmonyPrefix]
        internal static bool Prefix(float breathingInterval)
        {
            if (Guard.IsGamePaused()) return true;
            lastBreathInterval = breathingInterval;

            return true;
        }


        static float nextXP;
        [HarmonyPostfix]
        internal static void Postfix(Player __instance, ref float __result)
        {
            if (Guard.IsGamePaused() || __instance.CanBreathe() || __result == 0) return;

            __result = Air.UpdateOxygenPerBreath(__instance, ref __result, lastBreathInterval);
            return;
        }
    }
}
