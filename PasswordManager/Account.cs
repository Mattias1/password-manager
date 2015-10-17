using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace PasswordManager
{
    class Account
    {
        [JsonProperty("fields")]
        public List<Field> Fields;

        public Account() {
            // Add the default fields
            this.Fields = new List<Field> { Field.DisplayName, Field.Identifier, Field.Username, Field.Email };
        }

        public Field AddField(string name) {
            // Check if field key is valid
            if (name == "")
                return null;
            foreach (Field f in this.Fields)
                if (f.Name.ToLower() == name.ToLower())
                    return null;
            
            // Add a new field
            Field field = new Field(name);
            this.Fields.Add(field);
            return field;
        }
    }

    class Field
    {
        [JsonProperty("name")]
        public string Name;
        [JsonProperty("value")]
        public string Value;

        public Field(string name = "", string value = "") {
            this.Name = name;
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
