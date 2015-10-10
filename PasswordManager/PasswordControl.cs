using System;
using System.Windows.Forms;
using System.Collections.Generic;

namespace PasswordManager
{
    class PasswordControl : MattyUserControl
    {
        List<Tb> fields;
        Account account;
        Btn btnSettings, btnSave, btnBack, btnAdd;

        public PasswordControl() {
            // Add a field
            this.btnAdd = new Btn("Add field", this);
            this.btnAdd.Click += (o, e) => {  };

            // Add the standard buttons
            this.btnSave = new Btn("Save", this);
            this.btnSave.Click += (o, e) => {  };
            this.btnBack = new Btn("Back", this);
            this.btnBack.Click += (o, e) => { this.ShowUserControl(0); };
            this.btnSettings = new Btn("Settings", this);
            this.btnSettings.Click += (o, e) => {
                this.ShowUserControl(2);
                this.GoToControl = 1;
            };
        }

        public override void OnResize() {
            // Locate the fields
            // TODO

            // Change standard button locations
            this.btnSave.LocateInside(this, Btn.Horizontal.Left, Btn.Vertical.Bottom);
            this.btnBack.LocateFrom(this.btnSave, Btn.Horizontal.Right, Btn.Vertical.CopyBottom);
            this.btnSettings.LocateInside(this, Btn.Horizontal.Right, Btn.Vertical.Bottom);
        }

        public void View(Account account) {
            // Initialise for this particular account
            this.account = account;
            // TODO

            // View the control
            this.ShowUserControl(1);
        }
    }
}
