using System;
using System.Diagnostics;
using System.Reflection;
using UnityEngine;

namespace RPG_Framework.Lib.Web
{
    public class UpdateHandler
    {
        internal static readonly string rpgGitURL = "https://api.github.com/repos/gurrenm3/RPG_Framework-for-Subnautica/releases";
        private static bool finishedCheckingUpdates = false;

        public string GitReleasesURL { get; set; } = "";

        public UpdateHandler()
        {
            GitReleasesURL = rpgGitURL;
        }

        public UpdateHandler(string gitReleaseURL)
        {
            GitReleasesURL = (String.IsNullOrEmpty(gitReleaseURL)) ? rpgGitURL : gitReleaseURL;
        }


        public void HandleUpdates()
        {
            if (!CanCheckForUpdate())
                return;

            var unparsedGitText = GetGitText();
            if (string.IsNullOrEmpty(unparsedGitText))
                return;

            var releaseConfig = CreateReleaseConfigFromText(unparsedGitText);
            if (releaseConfig is null)
                return;

            if (IsUpdate(releaseConfig))
                Logger.Log("An update is available for RPG Framework!", LogType.Both);
            else
                Logger.Log("RPG Framework is up to date!", LogType.Both);

            finishedCheckingUpdates = true;
        }

        private float nextUpdateCheck;
        private bool CanCheckForUpdate()
        {
            if (finishedCheckingUpdates || Time.time < nextUpdateCheck)
                return false;

            const int nextCheckTime = 1800;
            nextUpdateCheck = Time.time + nextCheckTime;
            return true;
        }

        private string GetGitText()
        {
            WebReader reader = new WebReader();
            var gitText = reader.ReadText_FromURL(GitReleasesURL, maxTries: 50);
            return gitText;
        }

        private GithubReleaseConfig[] CreateReleaseConfigFromText(string unparsedGitText)
        {
            var releaseConfig = GithubReleaseConfig.FromJson(unparsedGitText);
            return releaseConfig;
        }

        private bool IsUpdate(GithubReleaseConfig[] githubReleaseConfigs)
        {
            GetCurrentAndLatestVersion(githubReleaseConfigs, out string latestGitVersion, out string currentVersion);
            var latest = VersionToInt(latestGitVersion);
            var current = VersionToInt(currentVersion);
            return latest > current;
        }

        private void GetCurrentAndLatestVersion(GithubReleaseConfig[] githubReleaseConfigs, out string latestGitVersion, out string currentVersion)
        {
            var mostRecentRelease = githubReleaseConfigs[0];
            latestGitVersion = mostRecentRelease.TagName;
            currentVersion = GetCurrentVersion();

            CleanVersionTexts(latestGitVersion, currentVersion, out latestGitVersion, out currentVersion);
        }

        private string GetCurrentVersion()
        {
            var assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo currentVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            return currentVersionInfo.FileVersion;
        }

        private void CleanVersionTexts(string latestGitVersion, string currentVersion, out string processedLatestVersion, out string processedCurrentVersion)
        {
            const string delimiter = ".";
            processedLatestVersion = latestGitVersion.Replace(delimiter, "");
            processedCurrentVersion = currentVersion.Replace(delimiter, "");

            bool areSameLength = (processedLatestVersion.Length == processedCurrentVersion.Length);
            if (areSameLength)
                return;

            const string fillerChar = "0";
            while (!areSameLength)
            {
                int lLength = processedLatestVersion.Length;
                int cLength = currentVersion.Length;

                processedLatestVersion = (lLength < cLength) ? processedLatestVersion + fillerChar : processedLatestVersion;
                processedCurrentVersion = (cLength < lLength) ? processedCurrentVersion + fillerChar : processedCurrentVersion;
                areSameLength = (processedLatestVersion.Length == processedCurrentVersion.Length);
            }
        }

        private int VersionToInt(string versionText)
        {
            Int32.TryParse(versionText, out int version);
            return version;
        }
    }
}