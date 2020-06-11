using Harmony;
using PriorityQueueInternal;
using RPG_Framework.Items.Pills;
using SMLHelper.V2;
using SMLHelper.V2.Handlers;
using SMLHelper.V2.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace RPG_Framework.Items
{
    class Items
    {
        private static Items items;
        public List<Pill> Pills = new List<Pill>();


        public static void CreateItems()
        {
            if (items == null) items = new Items();
            items.GetPills();
            items.PatchPills(items.Pills);
        }


        public static void CreateFabricatorTabs()
        {
            string executingLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string folderPath = Path.Combine(executingLocation, "Assets\\Sprites\\UI");
            string RPGTab = Path.Combine(folderPath, "RPG Fabricator Icon.png");
            string consumablesTab = Path.Combine(folderPath, "Red pill bottle_Smaller2.png");

            CraftTreeHandler.Main.AddTabNode(CraftTree.Type.Fabricator, "RPGItems", "RPG Items", ImageUtils.LoadSpriteFromFile(RPGTab));
            CraftTreeHandler.Main.AddTabNode(CraftTree.Type.Fabricator, "RPGConsumables", "Consumables", ImageUtils.LoadSpriteFromFile(consumablesTab), new string[] { "RPGItems" });
        }

        public List<Pill> GetPills()
        {
            Pills.Add(new Pill("RedPill", "Health pill", "A red pill", "Red Pill_Smaller.png"));
            Pills.Add(new Pill("BluePill", "Oxygen pill", "A blue pill", "Blue Pill_Smaller.png"));
            Pills.Add(new Pill("GreenPill", "Speed pill", "A green pill", "Green Pill_Smaller.png"));

            return Pills;
        }


        public static TechType GetModdedTechType(string classID)
        {
            Handler.TechTypeHandler.TryGetModdedTechType(classID, out TechType moddedTechType);
            return moddedTechType;
        }

        public void PatchPills(List<Pill> Pills)
        {
            foreach (Pill pill in Pills)
                pill.Patch();
        }
    }
}
