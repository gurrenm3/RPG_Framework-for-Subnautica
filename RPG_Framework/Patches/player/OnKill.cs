using HarmonyLib;

namespace RPG_Framework.Patches.player
{
    [HarmonyPatch(typeof(Player), nameof(Player.OnKill))]
    class Player_OnKill
    {
        [HarmonyPostfix]
        internal static void Postfix()
        {
            if (Guard.IsGamePaused())
                return;


            Logger.Log("All the XP you have gained since your last save is lost", Lib.LogType.Both);
            SaveData.GetSaveData(true);
        }
    }
}
