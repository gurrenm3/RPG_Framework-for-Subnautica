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

        Dictionary<string, float> XPGrowths = new Dictionary<string, float>()
        {
            {"Almost nothing | 0.1", 0.1f },
            {"Super slow | 0.25", 0.25f },
            {"Slow | 0.5", 0.5f },
            {"A little slower | 0.75", 0.75f },
            {"Normal | 1.0", 1.0f },
            {"A little faster | 1.25", 1.25f },
            {"Fast | 1.5", 1.5f },
            {"Faster | 1.75", 1.75f },
            {"Super fast | 2.0", 2f },
            {"Super super fast | 3.0", 3f },
            {"Extreme | 5.0", 5f },
            {"Max | 10.0", 10f },
        };
    }
}
