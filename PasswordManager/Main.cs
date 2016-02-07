using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MattyControls;

namespace PasswordManager
{
    class Main : MattyForm
    {
        private static Size MinSize = new Size(450, 295);

        public Main(List<Account> accounts)
            : base(MinSize, Settings.Get) {
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);

            // Set some standard values
            this.Text = "Password Manager";
            this.MaximizeBox = false;

            // Add the controls
            AccountControl accountControl = new AccountControl();
            this.AddUserControl(new MattyUserControl[] { new AccountListControl(accountControl, accounts), accountControl, new SettingsControl() });
            this.ShowUserControl<AccountListControl>();
        }

        public bool Save() {
            string json = this.GetUserControl<AccountListControl>().ToJsonString();
            bool noError = false;
            try {
                using (StreamWriter file = new StreamWriter(Settings.Get.FileLocation)) {
                    file.WriteLine(json);
                    noError = true;
                }
            }
            catch {
                MessageBox.Show("Error saving the file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return noError;
        }
    }
}
