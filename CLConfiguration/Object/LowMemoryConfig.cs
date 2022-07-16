using System;
using System.IO;

namespace ChaseLabs.CLConfiguration.Object;

/// <summary>
/// <para> Author: Drew Chase </para>
/// <para> Company: Chase Labs </para>
/// </summary>
public class LowMemoryConfig
{
    #region Private Fields

    //private bool Encrypt;
    private LowMemoryConfigManager Manager;

    #endregion Private Fields

    #region Internal Constructors

    /// <summary>
    /// Creates a Config Object with Key and Value
    /// </summary>
    /// <param name="_key"> </param>
    /// <param name="_value"> </param>
    /// <param name="_manager"> </param>
    /// <param name="encrypt_output"> </param>
    internal LowMemoryConfig(string _key, dynamic _value, LowMemoryConfigManager _manager/*, bool encrypt_output*/)
    {
        Manager = _manager;
        //Encrypt = encrypt_output;
        Key = _key;

        string path = Path.Combine(Manager.Path, $"{Key}.dat");
        if (!File.Exists(path)) Write(_value);

    }

    #endregion Internal Constructors

    #region Public Properties

    /// <summary>
    /// Returns the Configs Key
    /// </summary>
    public string Key { get; }

    /// <summary>
    /// Returns the Configs Value
    /// </summary>
    public dynamic Value
    {
        get => /*Encrypt ? AESMath.DecryptStringAES(Read(), Manager.EncryptionPassword) : */Read();
        set
        {
            Write(/*Encrypt ? AESMath.EncryptStringAES(value, Manager.EncryptionPassword) :*/ value);
        }
    }

    #endregion Public Properties

    #region Private Methods

    private dynamic Read()
    {
        string path = Path.Combine(Manager.Path, $"{Key}.dat");
        using BinaryReader reader = new(new FileStream(path, FileMode.Open, FileAccess.Read));
        string bin = reader.ReadString();
        string type = bin.Substring(1, bin.IndexOf(')') - 1);
        GC.Collect();
        return Convert.ChangeType(bin.Replace($"({type})", ""), Type.GetType(type));
    }

    private void Write(dynamic value)
    {
        string path = Path.Combine(Manager.Path, $"{Key}.dat");
        using BinaryWriter writer = new(new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write));
        writer.Write($"({value.GetType()}){value}");
        GC.Collect();
    }
    /// <summary>
    /// Returns the Value
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return Value.ToString();
    }

    #endregion Private Methods
}