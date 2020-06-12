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
        Config cfg;
        Dictionary<string, float> XPGrowths = XP_Handler.XPGrowths;
        Dictionary<string, float> XPGrowthsNoCustom = XP_Handler.XPGrowthsNoCustom;

        public Options() : base("RPG Options")
        {
            cfg = Config.GetConfig();
            KeybindChanged += Options_KeybindChanged;
            ChoiceChanged += Options_ChoiceChanged;
            SliderChanged += Options_SliderChanged;
            ToggleChanged += Options_ToggleChanged;
        }

        public override void BuildModOptions()
        {
            AddKeybindOption("boostKey", "Speed Boost Toggle", GameInput.Device.Keyboard, Config.GetConfig().SpeedBoostToggle);
            AddToggleOption("enableDoubleXP", "Enable DoubleXP Events?", cfg.EnableDoubleXPEvents);

            if (GetXPGrowth().ToLower().Contains("custom"))
                AddChoiceOption("xpGrowth", "XP Growth", XPGrowths.Keys.ToArray(), GetXPGrowth());
            else
                AddChoiceOption("xpGrowth", "XP Growth", XPGrowthsNoCustom.Keys.ToArray(), GetXPGrowth());

            //AddSliderOption("customXpGrowth", "Custom XP Growth", 0, 100, Config.GetConfig().XP_Multiplier);
        }

        private void Options_ChoiceChanged(object sender, ChoiceChangedEventArgs e)
        {
            if (e.Id == "xpGrowth")
            {
                cfg.XP_Multiplier = XPGrowths[e.Value];                
                if(cfg.DoubleXPApplied)
                    cfg.XP_Multiplier *= 2;

                Config.defaultXPMult = XPGrowths[e.Value];
            }
            Config.SaveConfig();
        }

        private void Options_SliderChanged(object sender, SliderChangedEventArgs e)
        {
            
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
            string custom = "";
            foreach (var growths in XPGrowths)
            {
                if (growths.Key.ToLower().Contains("custom"))
                    custom = growths.Key;

                if (growths.Value == Config.GetConfig().XP_Multiplier)
                    return growths.Key;
            }

            return custom;
        }

        
    }
}
