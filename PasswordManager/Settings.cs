﻿using System;
using System.IO;
using System.Net;
using System.Text;
using System.Drawing;
using System.Collections.Generic;

namespace PasswordManager
{
    class Settings
    {
        public const string VersionString = "0.0";

        #region Settings internals

        private const string path = "settings.ini";

        // Singleton code
        /// <summary>
        /// The settings instance
        /// </summary>
        private static Settings instance;
        /// <summary>
        /// The instance of the settings singleton
        /// </summary>
        public static Settings Get {
            get { return Settings.instance == null ? Settings.instance = new Settings() : Settings.instance; }
        }

        // The list with all settings
        /// <summary>
        /// The list with all settings. All keys are lowercase.
        /// </summary>
        private Dictionary<string, string> hashList;

        // String
        private string get(string key, string defaultValue) {
            if (!this.hashList.ContainsKey(key))
                this.set(key, defaultValue);
            return this.hashList[key];
        }
        private void set(string key, string value) {
            this.hashList[key] = value;
        }
        // Bool
        private bool get(string key, bool defaultValue) {
            return bool.Parse(this.get(key, defaultValue.ToString()));
        }
        private void set(string key, bool value) {
            this.set(key, value.ToString());
        }
        // Int
        private int get(string key, int defaultValue) {
            return int.Parse(this.get(key, defaultValue.ToString()));
        }
        private void set(string key, int value) {
            this.set(key, value.ToString());
        }
        // Float
        private float get(string key, float defaultValue) {
            return float.Parse(this.get(key, defaultValue.ToString()));
        }
        private void set(string key, float value) {
            this.set(key, value.ToString());
        }
        // Vector2 (assuming integer values)
        private Point get(string key, Point defaultValue) {
            return Str2Vec(this.get(key, Vec2Str(defaultValue)));
        }
        private void set(string key, Point value) {
            this.set(key, Vec2Str(value));
        }
        // Int[]
        private int[] get(string key, int[] defaultValue, string separator = ",") {
            string[] values = this.get(key, String.Join<int>(separator, defaultValue)).Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries);
            int[] result = new int[values.Length];
            for (int i = 0; i < result.Length; i++)
                result[i] = int.Parse(values[i]);
            return result;
        }
        private void set(string key, int[] value, string separator = ",") {
            this.set(key, String.Join<int>(separator, value));
        }

        // Settings properties

        // Private settings methods
        private Settings() {
            this.SetDefaults();
        }

        // Public settings methods
        public override string ToString() {
            return this.ToString(null);
        }
        public string ToString(char seperator) {
            return this.ToString(null, seperator);
        }
        public string ToString(List<string> properties, char seperator = ';') {
            return this.ToString(properties, seperator, Environment.NewLine);
        }
        public string ToString(List<string> properties, char seperator, string endSeperator) {
            // Write all hashlist values with keys in the properties list to a string, write all if the propertieslist is left null
            StringBuilder s = new StringBuilder();
            foreach (var tuple in this.hashList)
                if (properties == null || properties.Contains(tuple.Key))
                    s.Append(tuple.Key + seperator + tuple.Value + endSeperator);
            return s.ToString();
        }

        public void FromString(string s, char seperator = ';') {
            this.FromString(s, seperator, Environment.NewLine);
        }
        public void FromString(string s, char seperator, string endSeperator) {
            // Override hashlist with values from s (values not in s will be left intact)
            string[] lines = s.Split(endSeperator.ToCharArray());
            for (int i = 0; i < lines.Length; i++) {
                string[] keyVal = lines[i].Split(seperator);
                if (keyVal[0] != "")
                    this.hashList[keyVal[0]] = keyVal[1];
            }
        }

        /// <summary>
        /// Load the settings from file
        /// </summary>
        /// <returns>Whether there was an error loading</returns>
        public bool Load() {
            bool noError = false;
            // If the file doesnt exist, load the defaults
            if (!File.Exists(Settings.path))
                return false;
            try {
                using (StreamReader file = new StreamReader(Settings.path)) {
                    this.FromString(file.ReadToEnd());
                    noError = true;
                }
            }
            catch {
                //MessageScreen.Show("(!) The inifile was incorrectly modified." + System.Environment.NewLine + "The highscores will be resetted.", Color.Black, 0, 2);
                //this.ResetHighscores();
                return false;
            }

            return noError;
        }

        /// <summary>
        /// Save the settings to file
        /// </summary>
        /// <returns>Whether there was an error saving</returns>
        public bool Save() {
            bool noError = false;
            try {
                using (StreamWriter file = new StreamWriter(Settings.path)) {
                    file.WriteLine(this.ToString());
                    noError = true;
                }
            }
            catch {
                return false;
            }
            return noError;
        }

        /// <summary>
        /// Set the default values.
        /// </summary>
        public void SetDefaults() {
            this.hashList = new Dictionary<string, string>();
        }

        /// <summary>
        /// Parse an vector2 (with integer values) to a string
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static string Vec2Str(Point v, char seperator = ',') {
            return ((int)v.X).ToString() + seperator + ((int)v.Y).ToString();
        }
        /// <summary>
        /// Parse a string to a vector2 (with integer values)
        /// </summary>
        /// <param name="s"></param>
        /// <param name="seperator"></param>
        /// <returns></returns>
        public static Point Str2Vec(string s, char seperator = ',') {
            string[] vs = s.Split(seperator);
            return new Point(int.Parse(vs[0]), int.Parse(vs[1]));
        }

        /// <summary>
        /// Parse an vector2 (with floating point values) to a string
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static string Vec2StrF(Point v, char seperator = ',') {
            return v.X.ToString() + seperator + v.Y.ToString();
        }
        /// <summary>
        /// Parse a string to a vector2 (with floating point values)
        /// </summary>
        /// <param name="s"></param>
        /// <param name="seperator"></param>
        /// <returns></returns>
        public static Point Str2VecF(string s, char seperator = ',') {
            string[] vs = s.Split(seperator);
            return new Point(int.Parse(vs[0]), int.Parse(vs[1]));
        }

        #endregion

        public Point Position {
            get { return this.get("position", Point.Empty); }
            set { this.set("position", value); }
        }

        public Point Size {
            get { return this.get("position", Point.Empty); }
            set { this.set("position", value); }
        }
    }
}
