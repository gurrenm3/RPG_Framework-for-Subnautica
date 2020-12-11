using HarmonyLib;
using System.Reflection;
using SMLHelper.V2.Handlers;
using RPG_Framework.Lib;
using System;

namespace RPG_Framework
{
    class MainPatcher
    {
        public static void Patch()
        {
            var assembly = Assembly.GetExecutingAssembly();
            new Harmony($"GurrenM4_{assembly.GetName().Name}").PatchAll(assembly);

            Logger.MessageLogged += Logger_MessageLogged;
            OptionsPanelHandler.RegisterModOptions(new Options());
        }

        private static void Logger_MessageLogged(object sender, Logger.LogEvents e)
        {
            var logType = e.LogType;
            if (logType == LogType.Both || logType == LogType.InGame)
                ErrorMessage.AddMessage(e.Message);
            if (logType == LogType.Both || logType == LogType.LogFile)
                Console.WriteLine("[RPGFramework] " + e.Message);
        }
    }
}

