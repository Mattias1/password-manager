using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace PasswordManager
{
    class AccountListControl : MattyUserControl
    {
        AccountControl accountControl;
        List<Account> accounts;
        Tb tbFilter;
        Lb lbAccounts;
        Btn btnCreate, btnView, btnSettings;

        public AccountListControl(AccountControl accountControl) {
            // Some variables
            this.accounts = new List<Account>();
            this.accountControl = accountControl;

            // The controls
            this.tbFilter = new Tb(this);
            this.lbAccounts = new Lb(this);

            // Add standard buttons
            this.btnCreate = new Btn("Create new", this);
            this.btnCreate.Click += (o, e) => { this.CreateAccount(); };
            this.btnView = new Btn("View", this);
            this.btnView.Click += (o, e) => { this.ViewAccount(); };
            this.btnSettings = new Btn("Settings", this);
            this.btnSettings.Click += (o, e) => { this.ShowUserControl(2); };
        }

        public override void OnResize() {
            // The locations of the normal controls
            this.tbFilter.LocateInside(this);
            this.lbAccounts.LocateFrom(this.tbFilter, Btn.Horizontal.CopyLeft, Btn.Vertical.Bottom);

            // Locate the standard buttons
            this.btnCreate.LocateInside(this, Btn.Horizontal.Left, Btn.Vertical.Bottom);
            this.btnView.LocateFrom(this.btnCreate, Btn.Horizontal.Right, Btn.Vertical.CopyBottom);
            this.btnSettings.LocateInside(this, Btn.Horizontal.Right, Btn.Vertical.Bottom);

            // Add labels
            int labelWidth = 60;
            this.tbFilter.AddLabel("Filter: ", 10, true, labelWidth);
            this.lbAccounts.Size = new Size(this.lbAccounts.Width, this.tbFilter.Height); // Temporary set a new height, to position the label correctly :P
            this.lbAccounts.AddLabel("Accounts: ", 10, true, labelWidth);

            // Change sizes
            this.tbFilter.Size = new Size(this.Width - 10 - this.tbFilter.Location.X, this.tbFilter.Height);
            this.lbAccounts.Size = new Size(this.tbFilter.Width, this.btnCreate.Location.Y - this.lbAccounts.Location.Y - 10);
        }

        public void ViewAccount() {
            // Load the selected account
            int idx = this.lbAccounts.SelectedIndex;
            if (idx == -1 || idx >= this.accounts.Count)
                return;
            this.accountControl.View(this.accounts[idx]);
        }

        public void CreateAccount() {
            // Create a new account
            this.accountControl.View(new Account());
        }
    }
}
