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

        /// <summary>
        /// Check if an object is null and throw an Argument exception if it is
        /// </summary>
        /// <param name="obj">Object to check if null</param>
        public static void ThrowIfArgumentIsNull(object obj, string argumentName, string message = "")
        {
            if (obj != null)
                return;

            if (string.IsNullOrEmpty(message))
                throw new ArgumentNullException(argumentName);
            else
                throw new ArgumentNullException(argumentName, message);
        }


        public static void ThrowIfStringIsNull(string stringToCheck, string message)
        {
            if (String.IsNullOrEmpty(stringToCheck))
            {
                throw new Exception(message);
            }
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