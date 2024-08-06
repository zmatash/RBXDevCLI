using System.Diagnostics;

namespace RobloxTS_DevCLI.ToolWrappers;

public class ESLint
{
    private readonly string _eslintCmd;
    private readonly string _eslintrc;

    public ESLint()
    {
        var (eslint, eslintrc) = GetEslint();
        _eslintCmd = eslint;
        _eslintrc = eslintrc;
    }

    public void RunEslint(string file)
    {
        var processStartInfo = new ProcessStartInfo
        {
            FileName = _eslintCmd,
            Arguments = $"--config {_eslintrc} {file}",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = new Process();
        process.StartInfo = processStartInfo;
        process.Start();
        var output = process.StandardOutput.ReadToEnd();
        var error = process.StandardError.ReadToEnd();
        process.WaitForExit();

        if (process.ExitCode != 0) throw new Exception($"ESLint error: {error}");

        Console.WriteLine(output);
    }

    private static (string, string) GetEslint()
    {
        var workingDir = Directory.GetCurrentDirectory();
        var nodeModules = Path.Combine(workingDir, "node_modules");
        if (!Directory.Exists(nodeModules))
            throw new DirectoryNotFoundException($"node_modules not found at: {nodeModules}");
        var eslint = Path.Combine(nodeModules, "eslint", ".bin", "eslint.cmd");
        if (!File.Exists(eslint)) throw new FileNotFoundException($"eslint not found at: {eslint}");
        var eslintConfig = Path.Combine(workingDir, ".eslintrc.json");
        if (!File.Exists(eslintConfig)) throw new FileNotFoundException($"eslint config not found at: {eslintConfig}");

        return (eslint, eslintConfig);
    }
}