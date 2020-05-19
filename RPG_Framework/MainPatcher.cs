using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace RPG_Framework
{
    class MainPatcher
    {
        public static void Patch()
        {
            var harmony = HarmonyInstance.Create("gurrenm4.RPG_Framework");   // Change this line to match your mod. 
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}
