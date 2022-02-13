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

```csharp
    Manager.Add(new Config("a string key", "text"));
    Manager.Add(new Config("a bool key", false));
    Manager.Add(new Config("a float key", 1.0));
    Manager.Add(new Config("an int key", 1));
```

# Getting Config Rule
Use the Method `GetConfigByKey()` to get the specified config value.

To Convert a Config Value to Bool use the `ParseBoolean()` Method
for Float use `ParseFloat()`, integer uses `ParseInt()`, double uses `ParseDouble()`
for string Values the Default Value will suffice.

```csharp
public string StringConfig = Manager.GetConfigByKey("a string key").Value;


public bool BooleanConfig = Manager.GetConfigByKey("a bool key").Value;


public float FloatConfig = Manager.GetConfigByKey("a float key").Value;


public int IntConfig = Manager.GetConfigByKey("an int key").Value;

```

# Saving Config Rule
By setting the Config Value Equal to an appropriate value converted to string will automatically write to the config file

```csharp
Manager.GetConfigByKey("a string key").Value = "Hello";

Manager.GetConfigByKey("a bool key").Value = true;

Manager.GetConfigByKey("a float key").Value = 1.5;

Manager.GetConfigByKey("an int key").Value = 1;
```
