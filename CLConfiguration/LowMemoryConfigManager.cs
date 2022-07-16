using ChaseLabs.CLConfiguration.Object;
using System;
using System.IO;

namespace ChaseLabs.CLConfiguration;

public class LowMemoryConfigManager
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
    /// Config File Name not including extension. <br /> Ex: <i> config_file <b> <u> NOT </u></b>
    /// config_file.json </i>
    /// </param>
    /// <param name="directory"> Path to Config Items </param>
    /// <exception cref="ArgumentException" />
    public LowMemoryConfigManager(string name, string directory)
    {
        //EncryptionPassword = directory;
        Path = Directory.CreateDirectory(directory).FullName;
        Name = name;
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
    /// Returns the Directory that holds all of the data files
    /// </summary>
    public string Path { get; init; }

    /// <summary>
    /// If true the Config File will be encrypted.
    /// </summary>
    //public bool UseEncryption { get; private set; }

    #endregion Public Properties

    #region Public Methods

    /// <summary>
    /// Gets an existing config item or creates one with value
    /// </summary>
    /// <param name="key"> </param>
    /// <param name="default_value"> </param>
    /// <param name="encrypt_output"> if the value should be encrypted </param>
    /// <returns> </returns>
    public LowMemoryConfig GetOrCreate(string key, dynamic default_value/*, bool encrypt_output = false*/)
    {
        return new(key, default_value, this/*, encrypt_output*/);
    }

    /// <summary>
    /// Removes Config Input by Config Key
    /// </summary>
    /// <param name="key"> </param>
    public void Remove(string key)
    {
        string file = System.IO.Path.Combine(Path, $"{key}.dat");
        if (File.Exists(file))
        {
            File.Delete(file);
        }
    }

    #endregion Public Methods
}