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

        private string _value;

        /// <summary>
        /// Returns the Configs Key
        /// </summary>
        public string Key { get; }
        /// <summary>
        /// Returns the Configs Value
        /// </summary>
        public string Value
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
        public Config(string _key, object _value, ConfigManager _manager)
        {
            Key = _key;
            this._value = _value.ToString();
            Manager = _manager;
        }
        /// <summary>
        /// Attempts to Parse the Configs Value as Int
        /// </summary>
        /// <returns></returns>
        public int ParseInt()
        {
            int.TryParse(Value, out int result);
            return result;
        }
        /// <summary>
        /// Attempts to Parse the Configs Value as Double
        /// </summary>
        /// <returns></returns>
        public double ParseDouble()
        {
            double.TryParse(Value, out double result);
            return result;
        }
        /// <summary>
        /// Attempts to Parse the Configs Value as Float
        /// </summary>
        /// <returns></returns>
        public float ParseFloat()
        {
            float.TryParse(Value, out float result);
            return result;
        }
        /// <summary>
        /// Attempts to Parse the Configs Value as Boolean
        /// </summary>
        /// <returns></returns>
        public bool ParseBoolean()
        {
            bool.TryParse(Value, out bool result);
            return result;
        }
    }
}
