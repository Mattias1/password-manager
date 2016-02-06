using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Newtonsoft.Json;
using MattyControls;

namespace PasswordManager
{
    class AccountListControl : MattyUserControl
    {
        AccountControl accountControl;
        List<Account> accounts;
        Tb tbFilter;
        Lb lbAccounts;
        Btn btnCreate, btnView, btnDelete, btnSettings;

        public AccountListControl(AccountControl accountControl, List<Account> accounts) {
            // Some variables
            this.accounts = accounts;
            this.accountControl = accountControl;
            this.VisibleChanged += (o, e) => { if (this.Visible) this.fillAccounts(); };

            // The controls
            this.tbFilter = new Tb(this);
            this.lbAccounts = new Lb(this);
            this.lbAccounts.MouseDoubleClick += (o, e) => { this.ViewAccount(); };

            // Add standard buttons
            this.btnCreate = new Btn("Create new", this);
            this.btnCreate.Click += (o, e) => { this.CreateAccount(); };
            this.btnView = new Btn("View", this);
            this.btnView.Click += (o, e) => { this.ViewAccount(); };
            this.btnDelete = new Btn("Delete", this);
            this.btnDelete.Click += (o, e) => { this.DeleteAccount(); };
            this.btnSettings = new Btn("Settings", this);
            this.btnSettings.Click += (o, e) => { this.ShowUserControl<SettingsControl>(); };
        }

        public override void OnResize() {
            // The locations of the normal controls
            this.tbFilter.LocateInside(this);
            this.lbAccounts.LocateFrom(this.tbFilter, MattyControl.Horizontal.CopyLeft, MattyControl.Vertical.Bottom);

            // Locate the standard buttons
            this.btnCreate.LocateInside(this, MattyControl.Horizontal.Left, MattyControl.Vertical.Bottom);
            this.btnView.LocateFrom(this.btnCreate, MattyControl.Horizontal.Right, MattyControl.Vertical.CopyBottom);
            this.btnDelete.LocateFrom(this.btnView, MattyControl.Horizontal.Right, MattyControl.Vertical.CopyBottom);
            this.btnSettings.LocateInside(this, MattyControl.Horizontal.Right, MattyControl.Vertical.Bottom);

            // Add labels
            int labelWidth = 60;
            this.tbFilter.AddLabel("Filter: ", MattyControl.Distance, true, labelWidth);
            this.lbAccounts.Size = new Size(this.lbAccounts.Width, this.tbFilter.Height); // Temporary set a new height, to position the label correctly :P
            this.lbAccounts.AddLabel("Accounts: ", MattyControl.Distance, true, labelWidth);

            // Change sizes
            this.tbFilter.Size = new Size(this.Width - MattyControl.Distance - this.tbFilter.Location.X, this.tbFilter.Height);
            this.lbAccounts.Size = new Size(this.tbFilter.Width, this.btnCreate.Location.Y - this.lbAccounts.Location.Y - MattyControl.Distance);
        }

        private void fillAccounts() {
            // Empty the list
            this.lbAccounts.Items.Clear();
            // Add all accounts
            for (int i = 0; i < this.accounts.Count; i++)
                this.lbAccounts.Items.Add(this.accounts[i].DisplayName);
        }

        private int getSelected() {
            // Find the index of the selected account
            int idx = this.lbAccounts.SelectedIndex;
            if (idx == -1 || idx >= this.accounts.Count)
                return -1;
            return idx;
        }

        public void ViewAccount() {
            // View the selected account
            int idx = this.getSelected();
            if (idx >= 0)
                this.accountControl.View(this.accounts[idx]);
        }

        public void DeleteAccount() {
            // Delete the selected account
            int idx = this.getSelected();
            if (idx >= 0) {
                var result = MessageBox.Show("Are you sure you want to delete this account?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (result == DialogResult.Yes) {
                    this.accounts.RemoveAt(idx);
                    this.fillAccounts();
                    ((Main)this.Parent).Save();
                }
            }
        }

        public void CreateAccount() {
            // Create a new account
            Account account = new Account();
            account.Initialize();
            this.accounts.Add(account);
            this.accountControl.View(account);
        }

        public string ToJsonString() {
            return JsonConvert.SerializeObject(this.accounts);
        }
    }
}
