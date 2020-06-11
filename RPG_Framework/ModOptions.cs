using SMLHelper.V2.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RPG_Framework
{
    public class Options : ModOptions
    {
        private static int test;
        public Options() : base("RPG Options")
        {
            KeybindChanged += Options_KeybindChanged;
        }

        private void Options_KeybindChanged(object sender, KeybindChangedEventArgs e)
        {
            if (e.Id == "boostKey")
                Config.GetConfig().SpeedBoostToggle = e.Key;
        }

        public override void BuildModOptions()
        {
            AddKeybindOption("boostKey", "Speed Boost Toggle", GameInput.Device.Keyboard, Config.GetConfig().SpeedBoostToggle);
            //AddChoiceOption("xpGrowth", "XP Growth", new string[] { "Slow", "Normal", "Fast" }, test);
        }
    }
}
