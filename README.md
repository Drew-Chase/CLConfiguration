# Chase Labs Configuration Utility
This is a C# Config Manager Utility that allows easy config creation and use.
## To Install<br>
### with Package Manager<br>
`Install-Package ChaseLabs.Configuration`<br>
### with Nuget Manager<br>
`ChaseLabs.Configuration` and Download Latest

## For Sample Code Visit [WIKI](https://github.com/DcmanProductions/CLConfiguration/wiki)

# Getting Started
Config is saved as JSON and is constantly stored in memory.  See [Low Memory Config Manager](#low-memory-config) for more information.
```csharp
public ConfigManager Manager;

public void HandleConfig()
{
    // Saves config to /Path/To/Config/ConfigFileName.json
    Manager = new ConfigManager("ConfigFileName", "/Path/To/Config"); 
    // Change save rate every 5 seconds
    Manager = new ConfigManager([...], [...], 5000)
}
```

# Getting Config Rule
Use the Method `GetOrCreate([name], [default value])` to get the specified config value or have it created.

```csharp
public string StringConfig = Manager.GetOrCreate("a string key", "default value").Value;


public bool BooleanConfig = Manager.GetOrCreate("a bool key", false).Value;


public float FloatConfig = Manager.GetOrCreate("a float key", 0.0f).Value;


public int IntConfig = Manager.GetOrCreate("an int key", 0).Value;
```

# Saving Config Rule
By setting the Config Value Equal to an appropriate value converted to string will automatically write to the config file

```csharp
Manager.GetOrCreate("a string key", "Default Value").Value = "Hello";

Manager.GetOrCreate("a bool key", false).Value = true;

Manager.GetOrCreate("a float key", 0.0f).Value = 1.5;

Manager.GetOrCreate("an int key", 0).Value = 1;
```

# Low Memory Config
This configuration uses less memory, but is slower to access data. Config Items are stored in individual files and stored as binary instead of JSON

```csharp
public LowMemoryConfigManager Manager;

public void HandleConfig()
{
    // Saves config to /Path/To/Config/ConfigFileName.dat
    Manager = new LowMemoryConfigManager("ConfigFileName", "/Path/To/Config"); 
    // Change save rate every 5 seconds
    Manager = new LowMemoryConfigManager([...], [...], 5000)
}
```