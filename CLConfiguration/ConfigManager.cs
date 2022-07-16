using ChaseLabs.CLConfiguration.Object;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace ChaseLabs.CLConfiguration;

/// <summary>
/// <para> Author: Drew Chase </para>
/// <para> Company: Chase Labs </para>
/// </summary>
public class ConfigManager
{
    #region Private Fields

    private string _path = string.Empty;

    private bool dirty = false;

    #endregion Private Fields

    #region Public Constructors

    /// <summary>
    /// Initializes the Config Manager with a File Path
    /// </summary>
    /// <param name="name">
    /// Config File Name not including extension. <br /> Ex: <i> config_file <b> <u> NOT
    /// </u></b> config_file.json </i>
    /// </param>
    /// <param name="directory"> Path to Config Items </param>
    /// <param name="save_interval">
    /// How often the cached config values is written to file in milliseconds, default is 5
    /// seconds or 5000ms
    /// </param>
    /// <exception cref="ArgumentException" />
    public ConfigManager(string name, string directory, int save_interval = 5000)
    {
        //EncryptionPassword = directory;
        ConfigList = new();
        Name = name;
        Path = System.IO.Path.Combine(Directory.CreateDirectory(directory).FullName, $"{Name}.json");
        FindPreExistingConfigs();
        System.Timers.Timer timer = new()
        {
            AutoReset = true,
            Interval = save_interval,
            Enabled = true,
        };
        timer.Elapsed += (s, e) =>
        {
            if (dirty)
            {
                Write();
            }
        };
        AppDomain.CurrentDomain.ProcessExit += (s, e) =>
        {
            Write();
        };
        Write();
    }

    #endregion Public Constructors

    #region Public Properties

    /// <summary>
    /// The Password used to Encrypt the Config File
    /// </summary>
    //public string EncryptionPassword { get; private set; }


    /// <summary>
    /// Sets the Name of the Config
    /// <para> Good if You have multiple Configs </para>
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Returns the Current Path of the Config File
    /// </summary>
    public string Path { get; init; }

    /// <summary>
    /// If true the Config File will be encrypted.
    /// </summary>
    //public bool UseEncryption { get; private set; }

    #endregion Public Properties

    #region Private Properties

    private Dictionary<string, Config> ConfigList;

    #endregion Private Properties

    #region Public Methods

    /// <summary>
    /// Gets an existing config item or creates one with value
    /// </summary>
    /// <param name="key"> </param>
    /// <param name="default_value"> </param>
    /// <param name="encrypt_output"> if the value should be encrypted </param>
    /// <returns> </returns>
    public Config GetOrCreate(string key, dynamic default_value/*, bool encrypt_output = false*/)
    {
        if (!ConfigList.ContainsKey(key))
        {
            MarkDirty();
            ConfigList.Add(key, new(key, default_value, this/*, encrypt_output*/));
        }
        return ConfigList[key];
    }

    /// <summary>
    /// Tells the Config Writer to output current config values to file Only is used for Human
    /// Readable Configurations
    /// </summary>
    public void MarkDirty()
    {
        dirty = true;
    }

    /// <summary>
    /// Removes Config Input by Config Key
    /// </summary>
    /// <param name="key"> </param>
    public void Remove(string key)
    {
        ConfigList.Remove(key);
        Write();
    }

    /// <summary>
    /// Returns the Number of Config Inputs
    /// </summary>
    /// <returns> </returns>
    public int Size()
    {
        return ConfigList.Count;
    }

    /// <summary>
    /// Returns A Human Readable Version of the Config Class
    /// <para> Example:  </para>
    /// <code>Default Config File </code>
    /// </summary>
    /// <returns> </returns>
    public override string ToString()
    {
        return JsonConvert.SerializeObject(Read());
    }

    #endregion Public Methods

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
            if (File.Exists(Path))
            {
                foreach (var token in Read())
                {
                    GetOrCreate(token.Key, token.Value);
                }
            }
        }
    }

    /// <summary>
    /// Reads the Config File and Updates the Current Config Inputs
    /// </summary>
    /// <returns> </returns>
    /// <exception cref="StackOverflowException" />
    private JObject Read(int attempts = 0)
    {
        JObject json = new JObject();
        try
        {
            using StreamReader reader = new(new FileStream(Path, FileMode.Open, FileAccess.Read));
            string content = /*UseEncryption ? AESMath.DecryptStringAES(reader.ReadToEnd(), EncryptionPassword) :*/ reader.ReadToEnd();
            json = JsonConvert.DeserializeObject<JObject>(content);
        }
        catch (FileNotFoundException)
        {
            MarkDirty();
            Write();
        }
        catch (UnauthorizedAccessException e)
        {
            Console.Error.WriteLine($"Unable to access Config File: \"{Path}\"");
            Console.Error.WriteLine(e.StackTrace);
        }
        catch (Exception e)
        {
            if (attempts >= 5)
            {
                Console.Error.WriteLine($"Unable to read Config File: \"{Path}\"");
                Console.WriteLine(e.StackTrace);
            }
            else
                return Read(attempts + 1);
        }
        return json;
    }

    /// <summary>
    /// Writes to file the Current List of Config Objects
    /// <para> Example:  </para>
    /// <code>Key = Example<para>Value = Input<para>Output = "Example": "Input"</para></para> </code>
    /// </summary>
    private void Write()
    {
        if (dirty)
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
                writer = new(Path, false);
                writer.Write(/*UseEncryption ? AESMath.EncryptStringAES(JsonConvert.SerializeObject(json), EncryptionPassword) :*/ JsonConvert.SerializeObject(json, Formatting.Indented));
                dirty = false;
            }
            catch (IOException)
            {
                dirty = true;
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
    }

    #endregion Private Methods
}