using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace RPG_Framework
{
    class Guard
    {
        public static bool IsStringValid(string text)
        {
            if (text.Trim() == "" || text == null)
                return false;
            
            return true;
        }

        public static bool IsGamePaused()
        {
            return false;
        }
    }
}
