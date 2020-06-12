using Oculus.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPG_Framework
{
    class Hints
    {
        static Config cfg;
        static SaveData saveData;
        public static void CheckMSGs()
        {
            Hints hints = new Hints();
            cfg = Config.GetConfig();
            saveData = SaveData.GetSaveData();

            if (!hints.CanShowMSG())
                return;

            if (!cfg.XPGrowthMsgShown)
            {
                hints.ShowMsg(hints.XPGrowthMsg);
                cfg.XPGrowthMsgShown = true;
                return;
            }

            if ((saveData.SwimSpeedLevel > 0 || saveData.WalkSpeedLevel > 0) && !cfg.SpeedToggleMsgShown)
            {
                hints.ShowMsg(hints.SpeedToggleMsg);
                cfg.SpeedToggleMsgShown = true;
                return;
            }

            if (!cfg.DoubleXPMsgShown)
            {
                hints.ShowMsg(hints.DoubleXPMsg);
                cfg.DoubleXPMsgShown = true;
                return;
            }
        }
        

        private bool CanShowMSG()
        {
            if (ErrorMessage.main.messages.Count > 0)
                return false;
            return true;
        }

        private void ShowMsg(string msg)
        {
            Log.InGameMSG("[RPG Framework] " + msg);
        }

        string XPGrowthMsg = "Did you know you can change how fast you level up? Open the game's settings" +
            " to change how fast you level up. Check it out";
        string SpeedToggleMsg = "You can toggle your Speed Boost On / Off with your SpeedBoostToggle key." +
                " Default is CapsLock. You change it in the game's settings";
        string DoubleXPMsg = "Don't like Double XP Events? Disable them in the game's settings";
    }
}
