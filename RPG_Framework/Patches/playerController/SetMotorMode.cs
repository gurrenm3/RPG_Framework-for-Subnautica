using HarmonyLib;
using RPG_Framework.Stats.player;

namespace RPG_Framework.Patches.playerController
{
    [HarmonyPatch(typeof(PlayerController), nameof(PlayerController.SetMotorMode))]
    class PlayerController_SetMotorMode_Patch
    {
        [HarmonyPostfix]
        internal static void PostFix()
        {
            if (Guard.IsGamePaused())
                return;

            Speed speedInst = new Speed();
            speedInst.UpdateSpeed();
        }
    }
}
