using System.Diagnostics;
using TypeScriptCLI.Parsers;
using TypeScriptCLI.Tools;

namespace TypeScriptCLI.Models.Zap;

public class Zap
{
    private readonly string _configPath;
    private readonly string _zapRoot;

    public Zap(string configPath)
    {
        _configPath = configPath;
        var configDir = Path.GetDirectoryName(configPath);
        _zapRoot = configDir ?? throw new ArgumentNullException(nameof(configDir));
    }

    public Options Options { get; set; } = new();


    public static Zap FromFile(string zapConfigPath)
    {
        return ZapFileParser.ParseZapFile(zapConfigPath);
    }

    public void Run()
    {
        Console.WriteLine("Running Zap...");

        var startInfo = new ProcessStartInfo
        {
            FileName = "zap",
            Arguments = _configPath, // Add any arguments if needed
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = new Process();
        process.StartInfo = startInfo;
        process.Start();
        var output = process.StandardOutput.ReadToEnd();
        var error = process.StandardError.ReadToEnd();
        process.WaitForExit();

        if (process.ExitCode != 0) throw new Exception($"Zap error: {error}");
        Console.WriteLine(output);

        if (Options.TypeScript) PostProcessTypeFiles();
        Console.WriteLine("Zap finished.");
    }

    private void PostProcessTypeFiles()
    {
        var clientTypes = Path.Join(_zapRoot, Options.GetClientTypes());
        var serverTypes = Path.Join(_zapRoot, Options.GetServerTypes());

        if (!File.Exists(clientTypes) || !File.Exists(serverTypes))
        {
            Console.WriteLine("Types file not found. Skipping post processing.");
            return;
        }

        Console.WriteLine("Applying type fix...");
        WriteExportToTypes(clientTypes);
        WriteExportToTypes(serverTypes);

        var nodeModules = ESLint.FindNodeModules();
        if (Directory.Exists(nodeModules))
        {
            Console.WriteLine("Formatting types...");
            var eslint = new ESLint(_zapRoot);
            eslint.RunEslint(Path.GetFullPath(clientTypes));
            eslint.RunEslint(Path.GetFullPath(serverTypes));
        }

        Console.WriteLine("Post processing complete.");
    }

    private static void WriteExportToTypes(string file)
    {
        using var writer = new StreamWriter(file, true);
        writer.WriteLine();
        writer.WriteLine("export {}");
    }
}