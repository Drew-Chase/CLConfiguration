using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnostics.Windows.Configs;
using BenchmarkDotNet.Running;
using ChaseLabs.CLConfiguration;

class Program
{
    static void Main()
    {
        BenchmarkRunner.Run<MemoryTester>();
    }
}

[MemoryDiagnoser]
public class MemoryTester
{
    [Benchmark]
    public void TestLowMemory()
    {
        try
        {

            LowMemoryConfigManager manager = new("Application", Path.GetFullPath("./config/Low Memory"));
            string @string = manager.GetOrCreate("string", "Hello World").Value;
            bool @bool = manager.GetOrCreate("bool", false).Value;
            int @int = manager.GetOrCreate("int", 5).Value;
            float @float = manager.GetOrCreate("float", 20.50f).Value;
            double @double = manager.GetOrCreate("double", 12.12d).Value;
        }
        catch { }
    }
    [Benchmark]
    public void TestHumanReadable()
    {
        try
        {

            ConfigManager manager = new("Application", Path.GetFullPath("./config/Human Readable"));
            string @string = manager.GetOrCreate("string", "Hello World").Value;
            bool @bool = manager.GetOrCreate("bool", false).Value;
            int @int = manager.GetOrCreate("int", 5).Value;
            float @float = manager.GetOrCreate("float", 20.50f).Value;
            double @double = manager.GetOrCreate("double", 12.12d).Value;
        }
        catch { }
    }
}