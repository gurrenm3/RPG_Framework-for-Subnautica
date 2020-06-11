using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using UnityEngine;

namespace RPG_Framework.Updater
{
    class UpdateHandler
    {
        WebHandler reader;
        WebClient client = new WebClient();
        private static UpdateHandler update;

        string readURL = "";
        string gitURL = "https://raw.githubusercontent.com/gurrenm3/RPG_Framework-for-Subnautica/master/Versions.txt";

        int numOfCheckedUpdates = 0;
        int maxCheckUpdates = 1000;
        bool checkedUpdates = false;

        public static void CheckForUpdates()
        {
            if (update == null) update = new UpdateHandler();

            if (!update.CanCheckForUpdate()) return;

            if (update.numOfCheckedUpdates >= update.maxCheckUpdates)
            {
                Log.InGameMSG("RPG Framework timed out when checking for updates. This isn't bad and" +
                    " it will try again later");
                return;
            }            

            update.numOfCheckedUpdates++;
            bool result = update.IsUpdate();
            if (!result) return;

            update.reader = new WebHandler();
            string processedURL = update.reader.processGit_Text(update.readURL, "RPG_Framework: ", 0);
            string gitVersion = update.reader.Get_GitVersion(processedURL);

            if (!update.IsGitVersionValid(gitVersion))
            {
                Log.Output("Github version info is invalid");
                return;
            }

            bool isUpdate = update.CompareVersionsForUpdate(gitVersion, update.GetCurrentVersion());
            if(!isUpdate)
            {
                if(update.checkedUpdates == false)
                {
                    Log.InGameMSG("RPG Framework is up to date!");
                    update.checkedUpdates = true;
                }

                return;
            }

            Log.InGameMSG("There is an update available for RPG Framework");
            return;
        }

        private static float nextUpdateCheck;
        private bool CanCheckForUpdate()
        {
            if (Time.time < nextUpdateCheck)
                return false;

            nextUpdateCheck = Time.time + 1800;
            return true;
        }

        public bool IsUpdate()
        {
            try
            {
                readURL = client.DownloadString(gitURL);
                if (Guard.IsStringValid(readURL)) return true;
            }
            catch {  }

            return false;
        }


        public bool IsGitVersionValid(string gitVersion)
        {
            if (!Guard.IsStringValid(gitVersion))
            {
                Log.Output("Failed to read latest version of RPG Framework from GitHub");
                return false;
            }
            return true;
        }


        public string GetCurrentVersion()
        {
            string modPath = Environment.CurrentDirectory + "\\QMods\\RPG_Framework\\mod.json";
            if (!File.Exists(modPath)) return "";

            string version = "";
            string[] lines = File.ReadAllLines(modPath);
            foreach(string line in lines)
            {
                if (!line.Contains("Version")) continue;

                foreach (char c in line)
                {
                    if (Int32.TryParse(c.ToString(), out int number))
                        version = version + c;
                }
            }
            return version;
        }

        public bool CompareVersionsForUpdate(string gitVersion, string currentVersion)
        {
            if (Int32.Parse(currentVersion) < Int32.Parse(gitVersion))
                return true;

            return false;
        }
    }
}
