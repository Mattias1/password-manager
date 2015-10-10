using System;
using System.Windows.Forms;

namespace PasswordManager
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args) {
            // Load the settigns
            Settings.Get.Load();

            // Simple things can be done from the command line
            // Todo

            // For other things I want a gui
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());

            // Save the settings
            Settings.Get.Save();
        }
    }
}
