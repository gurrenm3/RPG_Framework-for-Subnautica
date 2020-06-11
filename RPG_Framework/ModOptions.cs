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
        Config cfg = Config.GetConfig();
        Dictionary<string, float> XPGrowths = XP_Handler.XPGrowths;
        public Options() : base("RPG Options")
        {
            KeybindChanged += Options_KeybindChanged;
            ChoiceChanged += Options_ChoiceChanged;
            ToggleChanged += Options_ToggleChanged;
        }

        public override void BuildModOptions()
        {
            AddKeybindOption("boostKey", "Speed Boost Toggle", GameInput.Device.Keyboard, Config.GetConfig().SpeedBoostToggle);
            AddChoiceOption("xpGrowth", "XP Growth", XPGrowths.Keys.ToArray(), GetXPGrowth());
            AddToggleOption("enableDoubleXP", "Enable DoubleXP Events?", cfg.EnableDoubleXPEvents);
        }

        private void Options_ChoiceChanged(object sender, ChoiceChangedEventArgs e)
        {
            if (e.Id == "xpGrowth")
                cfg.XP_Multiplier = XPGrowths[e.Value];

            Config.SaveConfig();
        }

        private void Options_KeybindChanged(object sender, KeybindChangedEventArgs e)
        {
            if (e.Id == "boostKey")
                Config.GetConfig().SpeedBoostToggle = e.Key;

            Config.SaveConfig();
        }

        private void Options_ToggleChanged(object sender, ToggleChangedEventArgs e)
        {
            if(e.Id == "enableDoubleXP")
                cfg.EnableDoubleXPEvents = e.Value;

            Config.SaveConfig();
        }


        private string GetXPGrowth()
        {
            foreach (var growths in XPGrowths)
                if (growths.Value == Config.GetConfig().XP_Multiplier)
                    return growths.Key;

            return "";
        }

        
    }
}
