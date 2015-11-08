using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace PasswordManager
{
    class Main : Form
    {
        private static Size MinSize = new Size(450, 290);

        MattyUserControl[] userControls;
        public int GoToControl;

        public Main(List<Account> accounts) {
            // Load and apply the settings
            Settings s = Settings.Get;
            this.StartPosition = FormStartPosition.Manual;
            this.Location = s.Position;
            this.ClientSize = new Size(s.Size);
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);

            // Set some standard values
            this.Text = "Password Manager";
            this.MaximizeBox = false;

            // Add the controls
            AccountControl accountControl = new AccountControl();
            this.userControls = new MattyUserControl[] { new AccountListControl(accountControl, accounts), accountControl, new SettingsControl() };
            foreach (MattyUserControl u in this.userControls) {
                u.Size = this.ClientSize;
                this.Controls.Add(u);
                u.OnResize();
            }

            // Locate the controls
            this.onResizeEnd(this, new EventArgs());

            // Register events
            this.LocationChanged += (o, e) => { Settings.Get.Position = this.Location; };
            this.ResizeEnd += onResizeEnd;
        }

        void onResizeEnd(object o, EventArgs e) {
            // Make sure its not too small
            this.ClientSize = new Size(Math.Max(this.ClientSize.Width, MinSize.Width), Math.Max(this.ClientSize.Height, MinSize.Height));

            // Save the size to the settings
            Settings.Get.Size = new Point(this.ClientSize);

            // Resize the user controls
            foreach (MattyUserControl u in this.userControls) {
                u.Size = this.ClientSize;
                u.OnResize();
            }
        }

        /// <summary>
        /// Show usercontrol at index i and hide all others
        /// </summary>
        /// <param name="i">The index</param>
        public void ShowUserControl(int i) {
            foreach (MattyUserControl u in this.userControls)
                u.Hide();
            this.userControls[i].Show();
            this.userControls[i].Size = this.ClientSize;
            this.userControls[i].OnResize();
        }

        public bool Save() {
            string json = ((AccountListControl)this.userControls[0]).ToJsonString();
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

    class MattyUserControl : UserControl
    {
        public int GoToControl {
            get { return this.Main.GoToControl; }
            set { this.Main.GoToControl = value; }
        }
        public Main Main {
            get { return (Main)this.Parent; }
        }

        /// <summary>
        /// Show usercontrol at index i and hide all others
        /// </summary>
        /// <param name="i">The index</param>
        public void ShowUserControl(int i) {
            this.GoToControl = -1;
            this.Main.ShowUserControl(i);
        }

        /// <summary>
        /// This method gets called after the usercontrol is resized
        /// </summary>
        public virtual void OnResize() { }
    }
}
