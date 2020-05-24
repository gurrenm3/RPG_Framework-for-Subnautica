using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace RPG_Framework.Updater
{
    class WebHandler
    {

        public string processGit_Text(string url, string deleteText, int lineNumber)    //call this one read git text and return the url we want. Delete text is the starting word, for example "toolbox2019: "
        {
            if (!Guard.IsStringValid(url))
                return null;

            string[] split = url.Split('\n');
            return split[lineNumber].Replace(deleteText, "");
        }
        public string Get_GitVersion(string url)    // will read processed git url and return a git version number
        {
            if (!Guard.IsStringValid(url))
                return null;

            string[] version = url.Split('/');
            return (version[version.Length - 2]).Replace(".", "");
        }
    }
}
