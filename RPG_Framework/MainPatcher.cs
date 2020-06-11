using Harmony;
using System.Reflection;
using SMLHelper.V2.Handlers;

namespace RPG_Framework
{
    class MainPatcher
    {
        public static void Patch()
        {
            var harmony = HarmonyInstance.Create("gurrenm4.RPG_Framework");   // Change this line to match your mod. 
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            OptionsPanelHandler.RegisterModOptions(new Options());

            //Items.Items.CreateFabricatorTabs();
            //Items.Items.CreateItems();
        }
    }
}

