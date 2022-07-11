using ChaseLabs.CLConfiguration.Object;
using ChaseLabs.Math;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ChaseLabs.CLConfiguration.List
{
    /// <summary>
    /// <para>Author: Drew Chase</para>
    /// <para>Company: Chase Labs</para>
    /// </summary>
    public class ConfigManager
    {
        #region Fields

        internal bool outdated = false;

        private string _path = string.Empty;

        #endregion Fields

        #region Public Constructors

        /// <summary>
        /// Initializes the Config Manager with a File Path
        /// </summary>
        /// <param name="file">File Path</param>
        /// <param name="save_interval">
        /// How often the cached config values is written to file in milliseconds, default is 5
        /// seconds or 5000ms
        /// </param>
        public ConfigManager(string file, int save_interval = 5000)
        {
            UseEncryption = false;
            EncryptionPassword = "";
            ConfigList = new();
            PATH = file;
            FindPreExistingConfigs();
            System.Timers.Timer timer = new()
            {
                AutoReset = true,
                Interval = save_interval,
                Enabled = true,
            };
            timer.Elapsed += (s, e) =>
            {
                if (outdated)
                {
                    Write();
                }
            };
            Write();
        }

        /// <summary>
        /// Initializes the Config Manager with a File Path and Encryption
        /// </summary>
        /// <param name="file"></param>
        /// <param name="useencryption"></param>
        /// <param name="encryption_password">Default is Machine Name</param>
        /// <param name="save_interval">
        /// How often the cached config values is written to file in milliseconds, default is 5
        /// seconds or 5000ms
        /// </param>
        public ConfigManager(string file, bool useencryption, string encryption_password = "", int save_interval = 5000)
        {
            UseEncryption = useencryption;
            EncryptionPassword = encryption_password;
            ConfigList = new();
            PATH = file;
            FindPreExistingConfigs();
            System.Timers.Timer timer = new()
            {
                AutoReset = true,
                Interval = save_interval,
                Enabled = true,
            };
            timer.Elapsed += (s, e) =>
            {
                if (outdated)
                {
                    Write();
                }
            };
        }

        #endregion Public Constructors

        #region Properties

        /// <summary>
        /// The Password used to Encrypt the Config File
        /// </summary>
        public string EncryptionPassword { get; private set; }

        /// <summary>
        /// Sets the Name of the Config
        /// <para>Good if You have multiple Configs</para>
        /// </summary>
        public string Name { get; set; }

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
        /// If true the Config File will be encrypted.
        /// </summary>
        public bool UseEncryption { get; private set; }

        private Dictionary<string, Config> ConfigList { get; set; }

        #endregion Properties

        #region Public Methods

        /// <summary>
        /// Adds a Config to this ConfigManager using the Key and Value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(string key, dynamic value)
        {
            if (!ConfigList.ContainsKey(key))
            {
                ConfigList.Add(key, new(key, value, this));
            }

            Write();
        }

        /// <summary>
        /// Gets an existing config item or creates one with value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="default_value"></param>
        /// <returns></returns>
        public Config GetOrCreate(string key, dynamic default_value)
        {
            if (!ConfigList.ContainsKey(key))
            {
                Add(key, default_value);
            }
            return ConfigList[key];
        }

        /// <summary>
        /// Removes Config Input by Config Key
        /// </summary>
        /// <param name="key"></param>
        public void Remove(string key)
        {
            ConfigList.Remove(key);
            Write();
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
        /// Returns A Human Readable Version of the Config Class
        /// <para>Example:</para>
        /// <code>Default Config File</code>
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(Read());
        }

        #endregion Public Methods

        #region Internal Methods

        /// <summary>
        /// Reads the Config File and Updates the Current Config Inputs
        /// </summary>
        /// <returns></returns>
        internal JObject Read()
        {
            JObject json = null;
            try
            {
                using (StreamReader reader = new(PATH))
                {
                    string content = UseEncryption ? AESMath.DecryptStringAES(reader.ReadToEnd(), EncryptionPassword) : reader.ReadToEnd();
                    json = JsonConvert.DeserializeObject<JObject>(content);
                    reader.Dispose();
                    reader.Close();
                }
                return json;
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
            JObject json = new();
            foreach ((string key, Config config) in ConfigList)
            {
                if (config.Value is Array || config.Value is ArrayList || config.Value is IList)
                {
                    json.Add(key, new JArray(config.Value));
                }
                else
                {
                    json.Add(key, config.Value);
                }
            }
            StreamWriter writer = null;
            try
            {
                writer = new(PATH, false);
                writer.Write(UseEncryption ? AESMath.EncryptStringAES(JsonConvert.SerializeObject(json), EncryptionPassword) : JsonConvert.SerializeObject(json, Formatting.Indented));
                outdated = false;
            }
            catch (IOException)
            {
                outdated = true;
            }
            finally
            {
                if (writer != null)
                {
                    writer.Flush();
                    writer.Dispose();
                    writer.Close();
                }
            }
        }

        #endregion Internal Methods

        #region Private Methods

        /// <summary>
        /// Loads the Config File and PreExisting Configs
        /// </summary>
        private void FindPreExistingConfigs()
        {
            JObject json = Read();
            if (json != null)
            {
                ConfigList.Clear();
                foreach (var token in Read())
                {
                    Add(token.Key, token.Value);
                }
            }
        }

        #endregion Private Methods
    }
}