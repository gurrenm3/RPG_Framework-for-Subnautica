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
