using System;
using System.Collections.Generic;

namespace PasswordManager
{
    class Account
    {
        Field displayName, identifier, username, email;
        List<Field> otherFiels;

        public Account() {
            // Add the default fields
            this.displayName = Field.DisplayName;
            this.identifier = Field.Identifier;
            this.username = Field.Username;
            this.email = Field.Email;
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
            get { return new Field("PW manager display name"); }
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
