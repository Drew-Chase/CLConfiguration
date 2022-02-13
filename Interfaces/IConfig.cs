namespace ChaseLabs.CLConfiguration.Interfaces
{
    /// <summary>
    /// The Skellington of a Config Object
    /// </summary>
    public interface IConfig
    {
        #region Public Properties

        /// <summary>
        /// Returns the Configs Key
        /// </summary>
        string Key { get; }

        /// <summary>
        /// Returns the Configs Value
        /// </summary>
        dynamic Value { get; set; }

        #endregion Public Properties
    }
}