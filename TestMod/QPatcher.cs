using HarmonyLib;
using QModManager.API.ModLoading;
using System.Reflection;

namespace TestMod
{
    [QModCore]
    public static class QModPatcher
    {
        [QModPatch]
        public static void Patch()
        {
            var assembly = Assembly.GetExecutingAssembly();
            new Harmony($"GurrenM4_{assembly.GetName().Name}").PatchAll(assembly);

            Stats_Core.Events.Player_Events.PlayerUpdated += Player_Events_PlayerUpdated;
        }

        private static void Player_Events_PlayerUpdated(object sender, Stats_Core.Events.Player_Events.PlayerEventArgs e)
        {
            ErrorMessage.AddMessage("TestMod Player Update");
        }
    }
}
