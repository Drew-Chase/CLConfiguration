
namespace ChaseLabs.CLConfiguration.Object;

/// <summary>
/// <para> Author: Drew Chase </para>
/// <para> Company: Chase Labs </para>
/// </summary>
public class Config
{
    #region Private Fields

    private dynamic _value;
    //private bool Encrypt;
    private ConfigManager Manager;

    #endregion Private Fields

    #region Internal Constructors

    /// <summary>
    /// Creates a Config Object with Key and Value
    /// </summary>
    /// <param name="_key"> </param>
    /// <param name="_value"> </param>
    /// <param name="_manager"> </param>
    /// <param name="encrypt_output"> </param>
    internal Config(string _key, dynamic _value, ConfigManager _manager/*, bool encrypt_output*/)
    {
        Key = _key;
        this._value = _value;
        Manager = _manager;
        //Encrypt = encrypt_output;
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
        get => /*Encrypt ? AESMath.DecryptStringAES(_value, Manager.EncryptionPassword) : */_value;
        set
        {
            _value = /*Encrypt ? AESMath.EncryptStringAES(value, Manager.EncryptionPassword) : */value;
            Manager.MarkDirty();
        }
    }
    /// <summary>
    /// Returns the Value
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return Value.ToString();
    }

    #endregion Public Properties
}