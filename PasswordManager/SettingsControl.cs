using System;
using System.Drawing;
using System.Windows.Forms;

namespace PasswordManager
{
    class SettingsControl : MattyUserControl
    {
        Btn btnOk, btnCancel, btnResetDefaults;

        public SettingsControl() {
            // Some basic settings
            this.Hide();

            // Add settings

            // Add buttons
            this.btnResetDefaults = new Btn("Reset defaults", this);
            this.btnResetDefaults.Size = new Size(this.btnResetDefaults.Width + 25, this.btnResetDefaults.Height);
            this.btnResetDefaults.Click += (o, e) => { this.setDefaults(); };
            this.btnOk = new Btn("Ok", this);
            this.btnOk.Click += (o, e) => { this.save(); };
            this.btnCancel = new Btn("Cancel", this);
            this.btnCancel.Click += (o, e) => { this.closeControl(); };

            // Load the settings
            this.VisibleChanged += (o, e) => { if (this.Visible) this.load(); };
        }

        public override void OnResize() {
            // Change settings locations

            // Change button locations
            this.btnResetDefaults.LocateInside(this, Btn.Horizontal.Left, Btn.Vertical.Bottom);
            this.btnCancel.LocateInside(this, Btn.Horizontal.Right, Btn.Vertical.Bottom);
            this.btnOk.LocateFrom(this.btnCancel, Btn.Horizontal.Left, Btn.Vertical.CopyBottom);
        }

        void load() {
            // Load from the settings
            Settings s = Settings.Get;
            // Todo
        }

        void setDefaults() {
            if (DialogResult.Yes == MessageBox.Show("Are you sure you want to reset to the defaults?", "Reset settings?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation)) {
                Settings.Get.SetDefaults();
                closeControl();
            }
        }

        void save() {
            // Save to the settings
            Settings s = Settings.Get;
            // Todo

            // Display the main Control
            closeControl();
        }

        void closeControl() {
            // Display the Password list control
            if (this.GoToControl != -1)
                this.ShowUserControl(this.GoToControl);
            else
                this.ShowUserControl(0);
        }
    }
}
