using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace PasswordManager
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args) {
            // Load the settings and the all the account info
            Settings.Get.Load();
            List<Account> accounts = LoadAccounts();
            if (accounts == null)
                accounts = new List<Account>();

            // Simple things can be done from the command line
            // Todo

            // For other things I want a gui
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main(accounts));

            // Save the settings
            Settings.Get.Save();
        }

        public static List<Account> LoadAccounts() {
            // If the file doesnt exist, load the defaults
            if (!File.Exists(Settings.Get.FileLocation))
                return null;
            try {
                using (StreamReader file = new StreamReader(Settings.Get.FileLocation)) {
                    string json = file.ReadToEnd();
                    return JsonConvert.DeserializeObject<List<Account>>(json);
                }
            }
            catch {
                return null;
            }
        }
    }
}
