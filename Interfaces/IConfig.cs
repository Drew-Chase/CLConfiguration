namespace ChaseLabs.CLConfiguration.Interfaces
{
    public interface IConfig
    {
        string Key { get; }
        string Value { get; set; }
    }
}
