# Chase Labs Configuration Utility
This is a C# Config Manager Utility that allows easy config creation and use.
## To Install<br>
### with Package Manager<br>
`Install-Package ChaseLabs.Configuration`<br>
### with Nuget Manager<br>
`ChaseLabs.Configuration` and Download Latest

## For Sample Code Visit [WIKI](https://github.com/DcmanProductions/CLConfiguration/wiki)

# Getting Started
```csharp
public ConfigManager Manager;

public void HandleConfig()
{
    Manager = new ConfigManager("/Path/To/File.conf"); // Plain Text
    Manager = new ConfigManager("/Path/To/File.conf", true); // With Encryption and Salt is your machine name
    Manager = new ConfigManager("/Path/To/File.conf", true, "Encryption Password"); // With Encryption and Salt is "Encryption Password"
}
```
# Creating Config Rule
The First Value of the Config Object is the `Key` and the second value is the `Value`
For the initial `Value` should be the default value.
It will be overruled if a config entry is found.

You do NOT need to create a config item, you can just call GetOrCreate and It will create an option

```csharp
    Manager.Add("a string key", "text");
    Manager.Add("a bool key", false);
    Manager.Add("a float key", 1.0);
    Manager.Add("an int key", 1);
```

# Getting Config Rule
Use the Method `GetOrCreate([name], [default value])` to get the specified config value or have it created.

```csharp
public string StringConfig = Manager.GetOrCreate("a string key", "default value").Value;


public bool BooleanConfig = Manager.GetOrCreate("a bool key", false).Value;


public float FloatConfig = Manager.GetOrCreate("a float key", 0.0f).Value;


public int IntConfig = Manager.GetOrCreate("an int key",0).Value;

```

# Saving Config Rule
By setting the Config Value Equal to an appropriate value converted to string will automatically write to the config file

```csharp
Manager.GetOrCreate("a string key", "Default Value").Value = "Hello";

Manager.GetOrCreate("a bool key", false).Value = true;

Manager.GetOrCreate("a float key", 0.0f).Value = 1.5;

Manager.GetOrCreate("an int key",0).Value = 1;
```
