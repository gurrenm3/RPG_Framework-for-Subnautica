using RPG_Framework.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPG_Framework
{
    class RPGKeyPress
    {
        internal static void ProcessKeys()
        {
            SetSpeedToggled();
            //ShowHideStatsUI();
        }

        private static bool statsUIShown;
        internal static void ShowHideStatsUI()
        {
            var key = Config.GetConfig().StatsUIToggle;
            if (!SMLHelper.V2.Utility.KeyCodeUtils.GetKeyDown(key))
                return;

            if (StatsPanel.GetPanel() == null || !StatsPanel.GetPanel().Visible)
                StatsPanel.Show();
            else
                StatsPanel.GetPanel().Visible = false;
        }


        private static bool speedToggled;
        internal static void SetSpeedToggled()
        {
            var speedKey = Config.GetConfig().SpeedBoostToggle;
            if (SMLHelper.V2.Utility.KeyCodeUtils.GetKeyDown(speedKey))
                speedToggled = !speedToggled;
        }

        internal static bool IsSpeedToggled() => speedToggled;
    }
}
