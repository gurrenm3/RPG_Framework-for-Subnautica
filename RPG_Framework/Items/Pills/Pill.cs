using SMLHelper.V2;
using SMLHelper.V2.Assets;
using SMLHelper.V2.Crafting;
using SMLHelper.V2.Handlers;
using SMLHelper.V2.Utility;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace RPG_Framework.Items.Pills
{
    internal class Pill : Craftable
    {
        private static Atlas.Sprite customSprite;
        public string ClassID { get; set; }
        public string FriendlyName { get; set; }
        public string Description { get; set; }
        public string SpriteName { get; set; }
        public TechGroup PDAGroup { get; set; }
        public TechCategory PDACategory { get; set; }
        public TechType RequiredToUnlock { get; set; }
        public TechType BasePrefab { get; set; }

        //public Pill() : base("RedPill", "Pill", "A red pill")
        public Pill(string classID, string friendlyName, string description, string spriteName) : this(classID, friendlyName, description, spriteName, TechGroup.Personal, TechCategory.Equipment, TechType.FirstAidKit, TechType.None)
        {

        }

        public Pill(string classID, string friendlyName, string description, string spriteName, TechGroup group, TechCategory category, TechType basePrefab, TechType requireForUnlock) : base(classID, friendlyName, description)
        {
            ClassID = classID;
            FriendlyName = friendlyName;
            Description = description;
            SpriteName = spriteName;
            PDAGroup = group;
            PDACategory = category;
            BasePrefab = basePrefab;
            RequiredToUnlock = requireForUnlock;
        }



        public override TechGroup GroupForPDA => PDAGroup;
        public override TechCategory CategoryForPDA => PDACategory;
        public override TechType RequiredForUnlock => RequiredToUnlock;
        public override string[] StepsToFabricatorTab => new string[] { "RPGItems", "RPGConsumables" };
        public override GameObject GetGameObject()
        {
            GameObject prefab = CraftData.GetPrefabForTechType(BasePrefab);
            return GameObject.Instantiate(prefab);
        }

        public override CraftTree.Type FabricatorType => CraftTree.Type.Fabricator;
        

        protected override TechData GetBlueprintRecipe()
        {
            var type = Handler.TechTypeHandler.TryGetModdedTechType(ClassID, out TechType moddedTechType);

            return new TechData()
            {
                craftAmount = 1,
                Ingredients =
                    {
                        new Ingredient(moddedTechType, 1)
                    }
            };
        }

        protected override Atlas.Sprite GetItemSprite()
        {
            string executingLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string folderPath = Path.Combine(executingLocation, "Assets\\Sprites\\Items");
            string spriteLocation = Path.Combine(folderPath, SpriteName);
            return ImageUtils.LoadSpriteFromFile(spriteLocation);
        }
    }
}
