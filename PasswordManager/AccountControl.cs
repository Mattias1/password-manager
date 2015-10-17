using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace PasswordManager
{
    class AccountControl : MattyUserControl
    {
        List<Tb> fieldTbs;
        Tb tbAddField;
        Account account;
        Btn btnSettings, btnSave, btnBack, btnAdd;

        public AccountControl() {
            // Init the fieldTbs. It's being filled later.
            this.fieldTbs = new List<Tb>();

            // The button to add a new field
            this.tbAddField = new Tb(this);
            this.btnAdd = new Btn("Add field", this);
            this.btnAdd.Click += (o, e) => { this.addField(); };

            // Add the standard buttons
            this.btnSave = new Btn("Save", this);
            this.btnSave.Click += (o, e) => { };
            this.btnBack = new Btn("Back", this);
            this.btnBack.Click += (o, e) => { this.ShowUserControl(0); };
            this.btnSettings = new Btn("Settings", this);
            this.btnSettings.Click += (o, e) => {
                this.ShowUserControl(2);
                this.GoToControl = 1;
            };
        }

        public override void OnResize() {
            if (this.fieldTbs.Count > 0) {
                // Locate the fields and add the label
                this.fieldTbs[0].LocateInside(this);
                for (int i = 1; i < this.account.Fields.Count; i++)
                    this.fieldTbs[i].LocateFrom(this.fieldTbs[i - 1], Btn.Horizontal.CopyLeft, Btn.Vertical.Bottom);

                // Position the add field button
                this.tbAddField.LocateFrom(this.fieldTbs[this.fieldTbs.Count - 1], Btn.Horizontal.CopyLeft, Btn.Vertical.Bottom);
                this.btnAdd.LocateFrom(this.tbAddField, Btn.Horizontal.Right);

                // Add field tb labels and adjust their sizes
                for (int i = 0; i < this.account.Fields.Count; i++) {
                    this.fieldTbs[i].AddLabel(this.account.Fields[i].Name + ":");
                    this.fieldTbs[i].Label.Size = new Size(this.fieldTbs[i].Label.Width + 9, this.fieldTbs[i].Label.Height);
                    this.fieldTbs[i].Size = new Size(this.Width - this.fieldTbs[i].Location.X - 10, this.fieldTbs[i].Height);
                }
            }
            else {
                // For backup, position the button inside the form
                this.btnAdd.LocateInside(this);
            }

            // Change standard button locations
            this.btnSave.LocateInside(this, Btn.Horizontal.Left, Btn.Vertical.Bottom);
            this.btnBack.LocateFrom(this.btnSave, Btn.Horizontal.Right, Btn.Vertical.CopyBottom);
            this.btnSettings.LocateInside(this, Btn.Horizontal.Right, Btn.Vertical.Bottom);
        }

        public void View(Account account) {
            // Initialise for this particular account
            this.account = account;

            // Remove the old field textboxes
            for (int i = this.fieldTbs.Count - 1; i >= 0; i--)
                this.Controls.Remove(this.fieldTbs[i]);
            this.fieldTbs.Clear();
            // Create the field textboxes
            for (int i = 0; i < this.account.Fields.Count; i++)
                this.addFieldTb(this.account.Fields[i]);

            // View the control
            this.ShowUserControl(1);
            this.OnResize();
        }

        private bool addField() {
            // Add the field
            Field field = this.account.AddField(this.tbAddField.Text);
            if (field == null)
                return false;
            this.addFieldTb(field);

            // Position the new buttons, reposition the add field button itself, and, for good measure, also reposition everything else
            this.OnResize();
            return true;
        }

        private void addFieldTb(Field field) {
            Tb tb = new Tb(this);
            tb.Text = field.Value;
            this.fieldTbs.Add(tb);
        }
    }
}
