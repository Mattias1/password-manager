using System;
using System.Drawing;
using System.Windows.Forms;
using MattyControls;

namespace PasswordManager
{
    class SettingsControl : MattyUserControl
    {
        Tb tbFileLocation, tbDefaultEmail;
        Btn btnOk, btnCancel, btnResetDefaults, btnBrowseFileLocation;

        public SettingsControl() {
            // Some basic settings
            this.Hide();

            // Add settings
            this.tbFileLocation = new Tb(this);
            this.btnBrowseFileLocation = new Btn("Browse", this);
            this.btnBrowseFileLocation.Click += this.browseFileLocation;
            this.tbDefaultEmail = new Tb(this);

            // Add buttons
            this.btnResetDefaults = new Btn("Reset defaults", this);
            this.btnResetDefaults.Size = new Size(this.btnResetDefaults.Width + 25, this.btnResetDefaults.Height);
            this.btnResetDefaults.Click += (o, e) => { this.setDefaults(); };
            this.btnOk = new Btn("Ok", this);
            this.btnOk.Click += (o, e) => { this.save(true); };
            this.btnCancel = new Btn("Cancel", this);
            this.btnCancel.Click += (o, e) => { this.closeControl(); };

            // Load the settings
            this.VisibleChanged += (o, e) => { if (this.Visible) this.load(); };
        }

        public override void OnResize() {
            // Change settings locations
            this.tbFileLocation.LocateInside(this);
            this.tbDefaultEmail.LocateFrom(this.tbFileLocation, MattyControl.Horizontal.CopyLeft, MattyControl.Vertical.Bottom);
            this.tbFileLocation.AddLabel("File location:");
            this.tbDefaultEmail.AddLabel("Default email:");
            this.btnBrowseFileLocation.LocateInside(this, MattyControl.Horizontal.Right, MattyControl.Vertical.Top);
            this.tbFileLocation.StretchRight(this.btnBrowseFileLocation);
            this.tbDefaultEmail.StretchRight(this);

            // Change button locations
            this.btnResetDefaults.LocateInside(this, MattyControl.Horizontal.Left, MattyControl.Vertical.Bottom);
            this.btnCancel.LocateInside(this, MattyControl.Horizontal.Right, MattyControl.Vertical.Bottom);
            this.btnOk.LocateFrom(this.btnCancel, MattyControl.Horizontal.Left, MattyControl.Vertical.CopyBottom);
        }

        void load() {
            // Load from the settings
            Settings s = Settings.Get;
            this.tbFileLocation.Text = s.FileLocation;
            this.tbDefaultEmail.Text = s.DefaultEmail;
        }

        void setDefaults() {
            if (DialogResult.Yes == MessageBox.Show("Are you sure you want to reset to the defaults?", "Reset settings?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation)) {
                Settings.Get.SetDefaults();
                closeControl();
            }
        }

        void save(bool close = false) {
            // Save to the settings
            Settings s = Settings.Get;
            s.FileLocation = this.tbFileLocation.Text;
            s.DefaultEmail = this.tbDefaultEmail.Text;

            // Display the main Control
            s.Save();
            if (close)
                closeControl();
        }

        void closeControl() {
            this.ShowLastVisitedUserControl();
        }

        void browseFileLocation(object o, EventArgs e) {
            // Get the file json file location with a shiny dialog
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Title = "The account details file";
            dialog.Filter = "JSON files|*.json";
            dialog.InitialDirectory = Settings.Get.FileLocation;
            dialog.FileName = "passwordmanager.json";
            if (dialog.ShowDialog() == DialogResult.OK) {
                this.tbFileLocation.Text = dialog.FileName;
            }
        }
    }
}
