using FMOD;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RPG_Framework.LevelUp
{
    public class LevelingSystem
    {
        public static void LevelUp()
        {

        }

        public static void PlayLevelUpSound() => PlayLevelUpSound("\\LevelUp.wav");
        public static void PlayLevelUpSound(string nameOfWaveFile)
        {
            string soundsDir = Environment.CurrentDirectory + "\\QMods\\RPG_Framework\\Assets\\Sounds";
            if (!Directory.Exists(soundsDir))
            {
                Directory.CreateDirectory(soundsDir);
                return;
            }

            if (Directory.GetFiles(soundsDir, "*.wav").Length <= 0) return;


            if (!nameOfWaveFile.StartsWith("\\"))
                nameOfWaveFile = "\\" + nameOfWaveFile;


            string levelUpFile = soundsDir + nameOfWaveFile;
            if (!File.Exists((levelUpFile).ToLower())) return;


            var sound = SMLHelper.V2.Utility.AudioUtils.CreateSound(levelUpFile);
            SMLHelper.V2.Utility.AudioUtils.PlaySound(sound);
        }
    }
}
