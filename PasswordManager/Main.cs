using System;
using System.Drawing;
using System.Windows.Forms;

namespace PasswordManager
{
    class Main : Form
    {
        private static Size MinSize = new Size(290, 200);

        MattyUserControl[] userControls;

        public Main() {
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
            this.userControls = new MattyUserControl[] { new PasswordListControl(), new PasswordControl(), new SettingsControl() };
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
    }

    class MattyUserControl : UserControl
    {
        /// <summary>
        /// Show usercontrol at index i and hide all others
        /// </summary>
        /// <param name="i">The index</param>
        public void ShowUserControl(int i) {
            ((Main)this.Parent).ShowUserControl(i);
        }

        /// <summary>
        /// This method gets called after the usercontrol is resized
        /// </summary>
        public virtual void OnResize() { }
    }
}
