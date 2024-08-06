using System.Diagnostics;

namespace TypeScriptCLI.Parsers.Zap;

public class ZapConfig
{
    private readonly string _configFile;

    public ZapConfig(string zapConfigPath)
    {
        _configFile = zapConfigPath;
        Options = new ZapOptions();

        var parser = new ZapParser(zapConfigPath, this);
        parser.ParseZap(zapConfigPath);
    }

    public ZapOptions Options { get; }

    public void RunZap()
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = "zap",
            Arguments = _configFile, // Add any arguments if needed
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

        if (process.ExitCode != 0) throw new Exception($"ESLint error: {error}");
        Console.WriteLine(output);

        if (Options.TypeScript) PostProcessTypeFiles();
    }

    private static void WriteExportToTypes(string file)
    {
        using var writer = new StreamWriter(file, true);
        writer.WriteLine();
        writer.WriteLine("export {}");
    }

    private void PostProcessTypeFiles()
    {
        if (!File.Exists(Options.ClientTypes) || !File.Exists(Options.ServerTypes))
        {
            Console.WriteLine("Types file not found. Skipping post processing.");
            return;
        }

        Console.WriteLine("Post processing types files...");
        var eslint = new ESLint();
        eslint.RunEslint(Options.ClientTypes);
        eslint.RunEslint(Options.ServerTypes);
        WriteExportToTypes(Options.ClientTypes);
        WriteExportToTypes(Options.ServerTypes);
    }
}