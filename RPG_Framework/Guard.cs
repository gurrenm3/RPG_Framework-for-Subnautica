using RPG_Framework.Stats;
using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace RPG_Framework
{
    class Guard
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        public static extern short GetKeyState(int keyCode);


        public static bool IsStringValid(string text)
        {
            if (text.Trim() == "" || text == null)
                return false;
            
            return true;
        }

        public static bool IsXPNegative(float xp, float xpNextLevel, out float newXP)
        {
            if (xp > 0)
            {
                newXP = xp;
                return false;
            }

            newXP = xpNextLevel / 2;            
            return true;
        }

        public static bool IsGamePaused()
        {
            if (Time.timeScale == 0f)
                return true;

            return false;
        }

        internal bool toggleOn = false;
        public static bool CanUseSpeedBoost()
        {
            Guard guard = new Guard();

            KeyCode speedKey = Config.GetConfig().SpeedBoostToggle;

            if (speedKey == KeyCode.CapsLock)
                guard.toggleOn = guard.CapsLockOn();
            else if (speedKey == KeyCode.Numlock)
                guard.toggleOn = guard.NumLockOn();
            else if (speedKey == KeyCode.ScrollLock)
                guard.toggleOn = guard.ScrollLockOn();
            else
                guard.toggleOn = RPGKeyPress.IsSpeedToggled();

            return guard.toggleOn;
        }

        private bool CapsLockOn() => (((ushort)GetKeyState(0x14)) & 0xffff) != 0;
        private bool NumLockOn() => (((ushort)GetKeyState(0x90)) & 0xffff) != 0;
        private bool ScrollLockOn() => (((ushort)GetKeyState(0x91)) & 0xffff) != 0;
    }
}
