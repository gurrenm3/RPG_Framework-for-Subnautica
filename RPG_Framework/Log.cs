using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RPG_Framework
{
    class Log
    {
        public static string LogPath = Environment.CurrentDirectory + "\\QMods\\RPG_Framework\\Log.txt";
        public static bool gameStart = true;
        
        /// <summary>
        /// Output text to local Log.txt file. Does not replace old log with new one, unless its first time logging this play session
        /// </summary>
        /// <param name="text">Text to output</param>
        public static void Output(string text)
        {
            if (!gameStart)
                Output(text, false);
            else
            {
                Output(text, true);
                gameStart = false;
            }
        }
        
        /// <summary>
        /// Base log function. Will log text to local Log.txt file.
        /// </summary>
        /// <param name="text">Text to log</param>
        /// <param name="newLog">Replace the old log with a new one?</param>
        public static void Output(string text, bool newLog)
        {
            string writeText = "";
            Console.WriteLine("[RPGFramework] " + text);

            if (newLog)
            {
                if (File.Exists(LogPath))
                    File.Delete(LogPath);
            }
            else
            {
                if (File.Exists(LogPath) && File.ReadAllText(LogPath).Length > 0)
                    writeText = File.ReadAllText(LogPath) + "\n";                    
            }

            writeText += ">> " + text;

            StreamWriter writer = new StreamWriter(LogPath);
            writer.Write(writeText);
            writer.Close();
        }
        public static void InGameMSG(string text)
        {
            ErrorMessage.AddMessage(text);
            Output(text);
        }
    }
}
