using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using MattyControls;

namespace PasswordManager
{
    class Settings : SettingsSingleton
    {
        protected override string path {
            get { return Application.StartupPath + Path.DirectorySeparatorChar + "passwordmanager.ini"; }
        }

        public static Settings Get {
            get { return SettingsSingleton.GetSingleton<Settings>(); }
        }

        public string FileLocation {
            get { return this.get("file", Application.StartupPath); }
            set { this.set("file", value); }
        }

        public string DefaultEmail {
            get { return this.get("defaultemail", ""); }
            set { this.set("defaultemail", value); }
        }
    }
}
