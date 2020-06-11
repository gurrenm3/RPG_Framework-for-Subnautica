using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace RPG_Framework.UI
{
    class Test
    {
        static bool hasLoaded = false;
        static GameObject prefab;
        static int i = 0;
        public static void Start()
        {
            if (!hasLoaded)
            {
                string executingLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string bundleLocation = Path.Combine(executingLocation, "testui");

                AssetBundle assetBundle = AssetBundle.LoadFromFile(bundleLocation);
                prefab = assetBundle.LoadAsset<GameObject>("Canvas");
                GameObject.Instantiate(prefab);
                
                hasLoaded = true;                
            }
            i++;
            prefab.GetComponent<Text>().text = "HEALTH " + i.ToString();
        }
    }
}
