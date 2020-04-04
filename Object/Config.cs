using ChaseLabs.CLConfiguration.Interfaces;
using ChaseLabs.CLConfiguration.List;
using System;
using System.IO;

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

        string _value, _key;
        public string Key => _key;
        public string Value
        {
            get
            {
                return _value;
            }
            set
            {

                _value = value;
                Configs.Singleton.Write();
            }
        }

        public Config(string _key, string _value)
        {
            this._key = _key;
            this._value = _value;
        }

        public int ParseInt()
        {
            int.TryParse(Value, out int result);
            return result;
        }
        public double ParseDouble()
        {
            double.TryParse(Value, out double result);
            return result;
        }
        public float ParseFloat()
        {
            float.TryParse(Value, out float result);
            return result;
        }
        public bool ParseBoolean()
        {
            bool.TryParse(Value, out bool result);
            return result;
        }
    }
}
