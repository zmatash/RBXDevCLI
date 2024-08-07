using System.Diagnostics;
using TypeScriptCLI.Parsers;
using TypeScriptCLI.Tools;

namespace TypeScriptCLI.Models.Zap;

public class Zap
{
    private readonly string _configPath;

    public Zap(string configPath)
    {
        _configPath = configPath;
    }

    public Options Options { get; set; } = new();


    public static Zap FromFile(string zapConfigPath)
    {
        return ZapFileParser.ParseZapFile(zapConfigPath);
    }

    public void Run()
    {
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
    }

    private void PostProcessTypeFiles()
    {
        var clientTypes = Options.GetClientTypes();
        var serverTypes = Options.GetServerTypes();
        if (clientTypes == null || serverTypes == null)
        {
            Console.WriteLine("Types file not found. Skipping post processing.");
            return;
        }

        var rootDir = Directory.GetCurrentDirectory();
        var nodeModules = Path.Join(rootDir, "node_modules");
        if (Directory.Exists(nodeModules))
        {
            Console.WriteLine("Formatting type files...");
            var eslint = new ESLint();
            eslint.RunEslint(clientTypes);
            eslint.RunEslint(serverTypes);
        }

        Console.WriteLine("Post processing type files...");
        WriteExportToTypes(clientTypes);
        WriteExportToTypes(serverTypes);
    }

    private static void WriteExportToTypes(string file)
    {
        using var writer = new StreamWriter(file, true);
        writer.WriteLine();
        writer.WriteLine("export {}");
    }
}