﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace PasswordManager
{
    class Account
    {
        [JsonProperty("fields")]
        public List<Field> Fields;

        [JsonIgnore]
        public string DisplayName {
            get { return this.Fields[0].Value; }
        }
        [JsonIgnore]
        public string Identifier {
            get { return this.Fields[1].Value; }
        }

        public Account() { }

        public bool Initialize() {
            // Only initialize the account if it's not yet initialized
            if (this.Fields != null)
                return false;
            // Add the default fields
            this.Fields = new List<Field> { Field.DisplayName, Field.Identifier, Field.Username, Field.Email, Field.Counter, Field.Type };
            return true;
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
        [JsonProperty("options")]
        public string[] AllowedValues;

        public Field(string name = "", string value = "", string[] allowedValues = null) {
            this.Name = name;
            this.Value = value;
            this.AllowedValues = allowedValues;
        }

        // Some fields that will come in handy
        public static Field DisplayName {
            get { return new Field("PWM display name", "New account"); }
        }
        public static Field Identifier {
            get { return new Field("Identifier", "<insert unique identifier>"); }
        }
        public static Field Username {
            get { return new Field("Name"); }
        }
        public static Field Email {
            get { return new Field("Email", Settings.Get.DefaultEmail); }
        }
        public static Field Counter {
            get { return new Field("Counter", "0"); }
        }
        public static Field Type {
            get {
                return new Field("Type", "Maximum Security Password", new string[] {
                    "Maximum Security Password", "Long Password", "Medium Password", "Short Password", "Basic Password", "PIN" 
                });
            }
        }
    }
}
