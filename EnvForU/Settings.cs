using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace EnvForU
{
    public class Settings
    {
        public string GitVersion
        {
            get
            {
                var p = new Process();
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.FileName = @"git.exe";
                p.StartInfo.WorkingDirectory = Environment.CurrentDirectory;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.RedirectStandardInput = true;
                p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                p.StartInfo.Arguments = "describe --tags --abbrev=0";
                p.StartInfo.CreateNoWindow = true;
                p.Start();

                var tagRef = p.StandardOutput.ReadLine();


                return tagRef.Split('/')
                    .Last()
                    .Replace("'", "");
            }
        }

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
        public Settings(string pathToSettings)
        {
            if (File.Exists(pathToSettings))
            {
                //validate file format
                var lines = File.ReadAllLines(pathToSettings);
                if (!IsValidFile(lines)) throw new FormatException("File does not match expected format.  No spaces allowed in key.  Must be an = on every non null line");
                foreach (var line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    if (line.Contains("="))
                    {
                        var parts = line.Split('=');
                        Dictionary.Add(parts[0].ToUpper(), parts[1]);
                    }
                }
            }
            //Write over it with actual environment variables as they should take precedence
            var envVars = Environment.GetEnvironmentVariables();
            foreach (var key in envVars.Keys)
            {
                Dictionary[key.ToString().ToUpper()] = envVars[key].ToString();
            }
            var gitDetected = this.Get("PATH").Contains("Git");
            if (gitDetected)
            {
                Dictionary["VERSION"] = GitVersion;
            }

        }

        private bool IsValidFile(string[] lines)
        {
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                if (!line.Contains("=")) return false;
                var parts = line.Split('=');
                var key = parts[0].Trim();
                if (key.Contains(" ")) return false;
            }
            return true; //TODO: Validate with Regex. Throw exception if it's an invalid format
        }
        public string this[string key]
        {
            get { return Dictionary[key]; }
            set { Dictionary[key] = value; }
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
