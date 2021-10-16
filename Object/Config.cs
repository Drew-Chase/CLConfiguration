using ChaseLabs.CLConfiguration.Interfaces;
using ChaseLabs.CLConfiguration.List;

namespace ChaseLabs.CLConfiguration.Object
{
    /// <summary>
    /// <para>
    /// Author: Drew Chase
    /// </para>
    /// <para>
    /// Company: Chase Labs
    /// </para>
    /// </summary>
    public class Config : IConfig
    {
        public ConfigManager Manager;

        private dynamic _value;

        /// <summary>
        /// Returns the Configs Key
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// Returns the Configs Value
        /// </summary>
        public dynamic Value
        {
            get => _value;
            set
            {
                _value = value;
                Manager.Write();
            }
        }

        /// <summary>
        /// Creates a Config Object with Key and Value
        /// </summary>
        /// <param name="_key"></param>
        /// <param name="_value"></param>
        /// <param name="_manager"></param>
        public Config(string _key, dynamic _value, ConfigManager _manager)
        {
            Key = _key;
            this._value = _value;
            Manager = _manager;
        }
    }
}