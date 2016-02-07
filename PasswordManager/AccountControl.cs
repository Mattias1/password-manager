using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using MattyControls;

namespace PasswordManager
{
    class AccountControl : MattyUserControl
    {
        List<Control> fieldTbs;
        Tb tbAddField;
        Account account;
        Btn btnSettings, btnSave, btnBack, btnAdd;

        public AccountControl() {
            // Init the fieldTbs. It's being filled later.
            this.fieldTbs = new List<Control>();

            // The button to add a new field
            this.tbAddField = new Tb(this);
            this.btnAdd = new Btn("Add field", this);
            this.btnAdd.Click += (o, e) => { this.addField(); };

            // Add the standard buttons
            this.btnSave = new Btn("Save", this);
            this.btnSave.Click += (o, e) => { this.save(); };
            this.btnBack = new Btn("Back", this);
            this.btnBack.Click += (o, e) => { this.ShowUserControl<AccountListControl>(); };
            this.btnSettings = new Btn("Settings", this);
            this.btnSettings.Click += (o, e) => { this.ShowUserControl<SettingsControl>(); };
        }

        private bool isTb(int index) {
            return this.account.Fields[index].AllowedValues == null;
        }
        private Tb getFieldTb(int index) {
            return (Tb)fieldTbs[index];
        }
        private Db getFieldDb(int index) {
            return (Db)fieldTbs[index];
        }

        private bool save() {
            // Set the values of the account fields
            for (int i = 0; i < this.account.Fields.Count; i++)
                this.account.Fields[i].Value = this.fieldTbs[i].Text;
            // Save to file
            bool success = ((Main)this.Parent).Save();
            if (success)
                MessageBox.Show("Changes saved successfully.", "Success");
            return success;
        }

        public override void OnResize() {
            if (this.fieldTbs.Count > 0) {
                // Locate the fields and add the label
                if (isTb(0))
                    this.getFieldTb(0).LocateInside(this);
                else
                    this.getFieldDb(0).LocateInside(this);
                for (int i = 1; i < this.account.Fields.Count; i++) {
                    if (isTb(i))
                        this.getFieldTb(i).LocateFrom(this.fieldTbs[i - 1], MattyControl.Horizontal.CopyLeft, MattyControl.Vertical.Bottom);
                    else
                        this.getFieldDb(i).LocateFrom(this.fieldTbs[i - 1], MattyControl.Horizontal.CopyLeft, MattyControl.Vertical.Bottom);
                }

                // Position the add field button
                this.tbAddField.LocateFrom(this.fieldTbs[this.fieldTbs.Count - 1], MattyControl.Horizontal.CopyLeft, MattyControl.Vertical.Bottom);
                this.btnAdd.LocateFrom(this.tbAddField, MattyControl.Horizontal.Right);

                // Add field tb labels and adjust their sizes
                for (int i = 0; i < this.account.Fields.Count; i++) {
                    if (isTb(i)) {
                        Tb tb = this.getFieldTb(i);
                        tb.AddLabel(this.account.Fields[i].Name + ":");
                        tb.Label.Size = new Size(tb.Label.Width + 9, tb.Label.Height);
                        tb.StretchRight(this);
                        tb.SelectionStart = 0;
                        tb.SelectionLength = 0;
                        tb.ScrollToCaret();
                        tb.SelectionLength = tb.TextLength;
                    }
                    else {
                        Db db = this.getFieldDb(i);
                        db.AddLabel(this.account.Fields[i].Name + ":");
                        db.Label.Size = new Size(db.Label.Width + 9, db.Label.Height);
                        db.StretchRight(this);
                    }
                }
            }
            else {
                // For backup, position the button inside the form
                this.btnAdd.LocateInside(this);
            }

            // Change standard button locations
            this.btnSave.LocateInside(this, MattyControl.Horizontal.Left, MattyControl.Vertical.Bottom);
            this.btnBack.LocateFrom(this.btnSave, MattyControl.Horizontal.Right, MattyControl.Vertical.CopyBottom);
            this.btnSettings.LocateInside(this, MattyControl.Horizontal.Right, MattyControl.Vertical.Bottom);
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
            this.ShowUserControl<AccountControl>();
            this.OnResize();
            if (isTb(0))
                this.getFieldTb(0).SelectAll();
            this.fieldTbs[0].Focus();
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
            // Adds a field textbox based on a field of the account
            if (field.AllowedValues == null) {
                Tb tb = new Tb(this);
                tb.Text = field.Value;
                this.fieldTbs.Add(tb);
                tb.Focus();
            }
            else {
                Db db = new Db(this);
                foreach (string item in field.AllowedValues)
                    db.Items.Add(item);
                db.Text = field.Value;
                this.fieldTbs.Add(db);
                db.Focus();
            }
        }
    }
}
