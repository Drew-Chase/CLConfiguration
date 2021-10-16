using ChaseLabs.CLConfiguration.Object;
using com.drewchaseproject.MDM.Library.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ChaseLabs.CLConfiguration.List
{
    /// <summary>
    /// <para>
    /// Author: Drew Chase
    /// </para>
    /// <para>
    /// Company: Chase Labs
    /// </para>
    /// </summary>
    public class ConfigManager
    {
        /// <summary>
        /// Sets the Name of the Config
        /// <para>Good if You have multiple Configs</para>
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The Password used to Encrypt the Config File
        /// </summary>
        public string EncryptionPassword { get; private set; }

        /// <summary>
        /// If true the Config File will be encrypted.
        /// </summary>
        public bool UseEncryption { get; private set; }

        /// <summary>
        /// Returns A Human Readable Version of the Config Class
        /// <para>Example:</para>
        /// <code>Default Config File</code>
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{Name} Config File";
        }

        private readonly List<Config> ConfigList;
        private string _path = string.Empty;

        /// <summary>
        /// Returns the Current Path of the Config File
        /// </summary>
        public string PATH
        {
            get
            {
                return _path;
            }
            set
            {
                Directory.CreateDirectory(Directory.GetParent(value).FullName);
                if (!File.Exists(value))
                {
                    File.CreateText(value).Close();
                }
                _path = value;
            }
        }

        /// <summary>
        /// Initializes the Config Manager with a File Path
        /// </summary>
        /// <param name="file">File Path</param>
        public ConfigManager(string file)
        {
            UseEncryption = false;
            ConfigList = new List<Config>();
            PATH = file;
            FindPreExistingConfigs();
        }

        /// <summary>
        /// Initializes the Config Manager with a File Path and Encryption
        /// </summary>
        /// <param name="file"></param>
        /// <param name="useencryption"></param>
        /// <param name="encryption_password">Default is Machine Name</param>
        public ConfigManager(string file, bool useencryption, string encryption_password = "")
        {
            UseEncryption = useencryption;
            EncryptionPassword = encryption_password;
            ConfigList = new List<Config>();
            PATH = file;
            FindPreExistingConfigs();
        }

        /// <summary>
        /// Loads the Config File and PreExisting Configs
        /// </summary>
        public void FindPreExistingConfigs()
        {
            using (StreamReader reader = new StreamReader(PATH))
            {
                string text = reader.ReadToEnd();
                if (UseEncryption)
                    text = Crypto.DecryptStringAES(text);
                string[] items = text.Split('\n');
                //while (!reader.EndOfStream)
                foreach (string item in items)
                {
                    string key = "", value = "";
                    string txt = item.Replace("\n", "");
                    if (txt.Split(':').Length == 2)
                    {
                        key = txt.Split(':')[0];
                        value = txt.Split(':')[1];
                    }
                    else if (txt.Split(':').Length > 2)
                    {
                        key = txt.Split(':')[0];
                        for (int i = 0; i < txt.Split(':').Length; i++)
                        {
                            if (i == 0)
                            {
                                continue;
                            }

                            value += txt.Split(':')[i];
                            if (i != txt.Split(':').Length - 1)
                            {
                                value += ":";
                            }
                        }
                    }
                    else
                    {
                        return;
                    }
                    value = value.Replace(": ", "").Replace("\" ", "").Replace(" \"", "").Replace("\"", "");
                    if (value.First() == '(')
                    {
                        string type = value.Substring(1, value.IndexOf(')') - 1);
                        value = value.Replace($"({type})", "");
                        Add(key.Replace("\"", "").Replace(": ", "").Replace(" \"", ""), Convert.ChangeType(value, Type.GetType(type)));
                    }
                }

                reader.Dispose();
                reader.Close();
            }
        }

        /// <summary>
        /// Returns the Number of Config Inputs
        /// </summary>
        /// <returns></returns>
        public int Size()
        {
            return ConfigList.Count;
        }

        /// <summary>
        /// Returns The Config Input based on Config Key.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Config GetConfigByKey(string value)
        {
            Config cfg = null;
            ConfigList.ForEach((n) => { if (n.Key == value) { cfg = n; } });
            return cfg;
        }

        /// <summary>
        /// Returns The Config Input based on Config Value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Config GetConfigByValue(string value)
        {
            Config cfg = null;
            ConfigList.ForEach((n) => { if (n.Value == value) { cfg = n; } });
            return cfg;
        }

        /// <summary>
        /// Returns The Config Input based on Config Index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Config GetConfigByIndex(int index)
        {
            return ConfigList.ElementAtOrDefault(index);
        }

        /// <summary>
        /// Adds a Config Input by Config Object
        /// </summary>
        /// <param name="config"></param>
        public void Add(Config config)
        {
            if (GetConfigByKey(config.Key) == null)
            {
                ConfigList.Add(config);
            }

            Write();
        }

        /// <summary>
        /// Adds a Config to this ConfigManager using the Key and Value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(string key, dynamic value)
        {
            if (GetConfigByKey(key) == null)
            {
                ConfigList.Add(new Config(key, value, this));
            }

            Write();
        }

        /// <summary>
        /// Removes Config Input by Conifg Object
        /// </summary>
        /// <param name="config"></param>
        public void Remove(Config config)
        {
            ConfigList.Remove(config);
            Write();
        }

        /// <summary>
        /// Removes Config Input by Index
        /// </summary>
        /// <param name="index"></param>
        public void Remove(int index)
        {
            ConfigList.RemoveAt(index);
            Write();
        }

        /// <summary>
        /// Removes Config Input by Config Key
        /// </summary>
        /// <param name="key"></param>
        public void Remove(string key)
        {
            ConfigList.Remove(GetConfigByKey(key));
            Write();
        }

        /// <summary>
        /// Returns a ArrayList of all Config Objects
        /// </summary>
        /// <returns></returns>
        public List<Config> List()
        {
            return ConfigList;
        }

        /// <summary>
        /// Reads the Config File and Updates the Current Config Inputs
        /// </summary>
        /// <returns></returns>
        public string Read()
        {
            dynamic Value = "";
            try
            {
                using (StreamReader reader = new StreamReader(PATH))
                {
                    string content = UseEncryption ? Crypto.DecryptStringAES(reader.ReadToEnd(), EncryptionPassword) : reader.ReadToEnd();
                    foreach (string s in content.Split('\n'))
                    {
                        string txt = s.Replace("\n", "");

                        foreach (Config config in ConfigList)
                        {
                            if (txt.Replace("\"", "").Replace(": ", "").StartsWith(config.Key))
                            {
                                config.Value = txt.Replace(config.Key, "").Replace("\"", "").Replace(": ", "");
                                Value += config.Value + Environment.NewLine;
                            }
                        }
                    }
                    reader.Dispose();
                    reader.Close();
                }
                return Value;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return Read();
            }
        }

        /// <summary>
        /// Writes to file the Current List of Config Objects
        /// <para>Example:</para>
        /// <code>Key = Example<para>Value = Input<para>Output = "Example": "Input"</para></para></code>
        /// </summary>
        internal void Write()
        {
            try
            {
                string cfg = "";
                foreach (Config config in ConfigList)
                {
                    cfg += $"\"{config.Key}\": \"({config.Value.GetType()}){config.Value}\"\n";
                }
                using (StreamWriter writer = new StreamWriter(PATH))
                {
                    writer.Write(UseEncryption ? Crypto.EncryptStringAES(cfg, EncryptionPassword) : cfg);
                    writer.Flush();
                    writer.Dispose();
                    writer.Close();
                }
            }
            catch
            {
            }
        }
    }
}