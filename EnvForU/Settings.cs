using System;
using System.Collections.Generic;
using System.IO;

namespace EnvForU
{
    public class Settings
    {
        private Dictionary<string, string> Dictionary { get; set; } = new Dictionary<string, string>();
        /// <summary>
        /// Uses .env in the project root.  Designed to be used in dev so we look there.  
        /// Production should use environment variables proper.
        /// </summary>
        public Settings() : this("../../.env")
        {
        }
        /// <summary>
        /// Relative to the executable or absolute.
        /// </summary>
        /// <param name="pathToSettings"></param>
        public Settings(string pathToSettings, bool useGitForVersion = false, string pathForGit = null)
        {
            bool forceFile = false;
            if (File.Exists(pathToSettings))
            {
                //validate file format
                var lines = File.ReadAllLines(pathToSettings);
                if (lines.Length > 0)
                    forceFile = lines[0].StartsWith("!!");
                if (!IsValidFile(lines)) throw new FormatException("File does not match expected format.  No spaces allowed in key.  Must be an = on every non null line");
                foreach (var line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#")) continue;
                    if (line.Contains("="))
                    {
                        var parts = line.Split('=');
                        Dictionary.Add(parts[0].ToUpper(), parts[1]);
                    }
                }
            }
            //Write over it with actual environment variables as they should take precedence
            //unless the .env files first 2 characters are !!
            var envVars = Environment.GetEnvironmentVariables();
            foreach (var key in envVars.Keys)
            {
                if (!Dictionary.ContainsKey(key.ToString()) || !forceFile)
                    Dictionary[key.ToString().ToUpper()] = envVars[key].ToString();
            }
        }

        private bool IsValidFile(string[] lines)
        {
            int lineNum = 0;
            foreach (var line in lines)
            {
                lineNum++;
                if (lineNum == 1 && line.StartsWith("!!")) continue;
                if (string.IsNullOrWhiteSpace(line)) continue;
                if (line.StartsWith("#")) continue;
                if (!line.Contains("=")) return false;
                var parts = line.Split('=');
                var key = parts[0].Trim();
                if (key.Contains(" ")) return false;
            }
            return true; 
        }
        public string this[string key]
        {
            get { return Dictionary[key]; }
            set { Dictionary[key] = value; }
        }
        public IEnumerable<string> Keys
        {
            get
            {
                foreach (var key in Dictionary.Keys)
                    yield return key;
            }
        }
        public IEnumerable<string> Values
        {
            get
            {
                return Dictionary.Values;
            }
        }
        public string Get(string key)
        {
            return Dictionary[key.ToUpper()];
        }
        public void Set(string key, string value)
        {
            Dictionary[key.ToUpper().Trim()] = value.Trim();
        }
    }
}
