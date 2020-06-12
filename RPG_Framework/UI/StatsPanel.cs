using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace RPG_Framework.UI
{
    class StatsPanel
    {
        public StatsPanel()
        {
            Log.InGameMSG("Starting...");
            Start();
        }

        #region Properties

        private static StatsPanel panel;
        public GameObject Prefab { get; set; }
        public GameObject Main { get; set; }

        private bool visible;
        public bool Visible
        {
            get { return visible; }
            set 
            {
                visible = value;
                if (visible)
                    panel.Main.SetActive(true);
                else
                    Hide();
            }
        }
        #endregion

        private void Start()
        {
            panel = this;

            string executingLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string bundleLocation = Path.Combine(executingLocation, "statsui");

            AssetBundle assetBundle = AssetBundle.LoadFromFile(bundleLocation);
            Prefab = assetBundle.LoadAsset<GameObject>("Canvas");
            Main = GameObject.Instantiate(Prefab);
            Main.SetActive(false);

            Show();
        }

        public static StatsPanel GetPanel()
        {
            return panel;
        }

        public static void Show()
        {
            if (panel == null || panel.Main == null || panel.Prefab == null)
                panel = new StatsPanel();

            panel.Visible = true;
        }

        public void Hide()
        {
            Close();
        }

        public static void Close()
        {
            //Visible = false;
            //GameObject.Destroy(Prefab);
            //Main.SetActive(false);
            //GameObject.DestroyImmediate(Main);
            //panel = null;
            panel.Main.SetActive(false);
            //panel = null;
        }
    }
}
