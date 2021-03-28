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

        private readonly string _path;
        /// <summary>
        /// Returns the Current Path of the Config File
        /// </summary>
        public string PATH => _path;

        /// <summary>
        /// Initializes the Config Manager with a File Path
        /// </summary>
        /// <param name="_path"></param>
        /// <param name="_encrypt"></param>
        public ConfigManager(string _path, bool _encrypt = false)
        {
            UseEncryption = _encrypt;
            ConfigList = new List<Config>();
            this._path = _path;
            if (!Directory.Exists(Directory.GetParent(PATH).FullName))
            {
                Directory.CreateDirectory(Directory.GetParent(PATH).FullName);
                System.Threading.Thread.Sleep(1000);
            }

            if (!File.Exists(PATH))
            {
                File.CreateText(PATH).Close();
            }

            FindPreExistingConfigs();
        }

        /// <summary>
        /// Loads the Config File and PreExisting Configs
        /// </summary>
        public void FindPreExistingConfigs()
        {
            using (StreamReader reader = new StreamReader(PATH))
            {
                while (!reader.EndOfStream)
                {
                    string key = "", value = "";
                    string txt = reader.ReadLine();
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

                    Add(key.Replace("\"", "").Replace(": ", "").Replace(" \"", ""), value.Replace(": ", "").Replace("\" ", "").Replace(" \"", "").Replace("\"", ""));
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
        public void Add(string key, object value)
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
            string Value = "";
            try
            {
                using (StreamReader reader = new StreamReader(PATH))
                {
                    if (UseEncryption)
                    {
                        while (!reader.EndOfStream)
                        {
                            string txt = reader.ReadLine();
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
                    else
                    {
                        string content = Crypto.DecryptStringAES(reader.ReadToEnd());
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
                    }
                }
                return Value;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                //System.Threading.Thread.Sleep(3 * 1000);
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
                    cfg += $"\"{config.Key}\": \"{config.Value}\"\n";
                }
                using (StreamWriter writer = new StreamWriter(PATH))
                {
                    writer.Write(UseEncryption ? Crypto.EncryptStringAES(cfg) : cfg);
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
