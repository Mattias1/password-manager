using System;
using System.Collections.Generic;

namespace PasswordManager
{
    class Account
    {
        public List<Field> Fields;

        public Account() {
            // Add the default fields
            this.Fields = new List<Field> { Field.DisplayName, Field.Identifier, Field.Username, Field.Email };
        }
    }

    class Field
    {
        public string Id, Value;

        public Field(string id = "", string value = "") {
            this.Id = id;
            this.Value = value;
        }

        // Some fields that will come in handy
        public static Field DisplayName {
            get { return new Field("PWM display name"); }
        }
        public static Field Identifier {
            get { return new Field("Identifier"); }
        }
        public static Field Username {
            get { return new Field("Name"); }
        }
        public static Field Email {
            get { return new Field("Email"); }
        }
    }
}
