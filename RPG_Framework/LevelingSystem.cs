using FMOD;
using FMODUnity;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

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
            PlaySound(sound, VolumeControl.Master);
            //SMLHelper.V2.Utility.AudioUtils.PlaySound(sound);


            /*RuntimeManager.LowlevelSystem.getMasterChannelGroup(out ChannelGroup channels);
            var newChannels = channels;
            newChannels.setVolume(SoundSystem.masterVolume);
            RuntimeManager.LowlevelSystem.playSound(sound, newChannels, false, out Channel channel);*/

        }

        /// <summary>
        /// The a list the different volume controls in the game
        /// </summary>
        public enum VolumeControl { Master, Music, Voice, Ambient }

        /// <summary>
        /// Plays a <see cref="Sound"/> globally at specified volume
        /// </summary>
        /// <param name="sound">The sound which should be played</param>
        /// <param name="volumeControl">Which volume control to adjust sound levels by. How loud sound is.</param>
        /// <returns>The channel on which the sound was created</returns>
        public static Channel PlaySound(Sound sound, VolumeControl volumeControl)
        {
            float volumeLevel;
            switch (volumeControl)
            {
                case VolumeControl.Master:
                    volumeLevel = SoundSystem.masterVolume;
                    break;
                case VolumeControl.Music:
                    volumeLevel = SoundSystem.musicVolume;
                    break;
                case VolumeControl.Voice:
                    volumeLevel = SoundSystem.voiceVolume;
                    break;
                case VolumeControl.Ambient:
                    volumeLevel = SoundSystem.ambientVolume;
                    break;
                default:
                    volumeLevel = 1f;
                    break;
            }

            RuntimeManager.LowlevelSystem.getMasterChannelGroup(out ChannelGroup channels);
            var newChannels = channels;
            newChannels.setVolume(volumeLevel);
            RuntimeManager.LowlevelSystem.playSound(sound, newChannels, false, out Channel channel);
            return channel;
        }
    }
}
