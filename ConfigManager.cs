using ChaseLabs.CLConfiguration.Object;
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
        private static ConfigManager _singleton;
        public static ConfigManager Singleton
        {
            get
            {
                if (_singleton == null)
                {
                    _singleton = new ConfigManager();
                }

                return _singleton;
            }
        }


        private List<Config> ConfigList;
        public string PATH { get; set; }

        protected ConfigManager() { }

        public void Init(string _path)
        {
            ConfigList = new List<Config>();
            PATH = _path;
            if (!Directory.Exists(Directory.GetParent(PATH).FullName))
            {
                Directory.CreateDirectory(PATH);
                System.Threading.Thread.Sleep(1000);
            }
            FindPreExistingConfigs();
        }

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

                    Add(new Config(key.Replace("\"", "").Replace(": ", "").Replace(" \"", ""), value.Replace(": ", "").Replace("\" ", "").Replace(" \"", "").Replace("\"", "")));
                }

                reader.Dispose();
                reader.Close();
            }
        }


        public int Size()
        {
            return ConfigList.Count;
        }

        public Config GetConfigByKey(string value)
        {
            Config cfg = null;
            ConfigList.ForEach((n) => { if (n.Key == value) { cfg = n; } });
            return cfg;
        }
        public Config GetConfigByValue(string value)
        {
            Config cfg = null;
            ConfigList.ForEach((n) => { if (n.Value == value) { cfg = n; } });
            return cfg;
        }

        public Config GetConfigByIndex(int index)
        {
            return ConfigList.ElementAtOrDefault(index);
        }

        public void Add(Config config)
        {
            if (GetConfigByKey(config.Key) == null)
            {
                ConfigList.Add(config);
            }

            Write();
        }

        public void Remove(Config config)
        {
            ConfigList.Remove(config);
            Write();
        }
        public void Remove(int index)
        {
            ConfigList.RemoveAt(index);
            Write();
        }
        public void Remove(string key)
        {
            ConfigList.Remove(GetConfigByKey(key));
            Write();
        }

        public List<Config> List()
        {
            return ConfigList;
        }

        public string Read()
        {
            string Value = "";
            try
            {
                using (StreamReader reader = new StreamReader(PATH))
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
                return Value;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                //System.Threading.Thread.Sleep(3 * 1000);
                return Read();
            }
            return "";
        }

        public void Write()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(PATH))
                {
                    foreach (Config config in ConfigList)
                    {
                        writer.WriteLine($"\"{config.Key}\": \"{config.Value}\"");
                    }
                    writer.Flush();
                    writer.Dispose();
                    writer.Close();
                }
            }
            catch
            {
            }
        }

        public void Close()
        {
        }

    }

}
