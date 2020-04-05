﻿using ChaseLabs.CLConfiguration.Interfaces;
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
        private string _value;
        private readonly string _key;
        /// <summary>
        /// Returns the Configs Key
        /// </summary>
        public string Key => _key;
        /// <summary>
        /// Returns the Configs Value
        /// </summary>
        public string Value
        {
            get => _value;
            set
            {

                _value = value;
                ConfigManager.Singleton.Write();
            }
        }
        /// <summary>
        /// Creates a Config Object with Key and Value
        /// </summary>
        /// <param name="_key"></param>
        /// <param name="_value"></param>
        public Config(string _key, string _value)
        {
            this._key = _key;
            this._value = _value;
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
