using System.Diagnostics;

namespace TypeScriptCLI.Tools;

// ReSharper disable once InconsistentNaming
public class ESLint
{
    private readonly string _eslintCmd;
    private readonly string _eslintrc;
    private readonly string _workingDir;

    public ESLint(string? workingDir = null)
    {
        var (eslint, eslintrc) = GetEslint();
        _eslintCmd = eslint;
        _eslintrc = eslintrc;
        _workingDir = workingDir ?? Directory.GetCurrentDirectory();
    }

    public void RunEslint(string file, bool log = true)
    {
        var originalWorkingDir = SetTemporaryWorkingDir();
        var processStartInfo = new ProcessStartInfo
        {
            FileName = _eslintCmd,
            Arguments = $"--config {_eslintrc} {file} --fix",
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
        Directory.SetCurrentDirectory(originalWorkingDir);
        if (log) Console.WriteLine(output);
    }

    private string SetTemporaryWorkingDir()
    {
        var originalWorkingDir = Directory.GetCurrentDirectory();
        Directory.SetCurrentDirectory(_workingDir);
        return originalWorkingDir;
    }

    private static (string, string) GetEslint()
    {
        var workingDir = Directory.GetCurrentDirectory();
        var nodeModules = FindNodeModules();
        if (!Directory.Exists(nodeModules))
            throw new DirectoryNotFoundException($"node_modules not found at: {nodeModules}");
        var eslint = Path.Combine(nodeModules, ".bin", "eslint.cmd");
        if (!File.Exists(eslint)) throw new FileNotFoundException($"eslint not found at: {eslint}");
        var eslintConfig = Path.Combine(workingDir, ".eslintrc.json");
        if (!File.Exists(eslintConfig)) throw new FileNotFoundException($"eslint config not found at: {eslintConfig}");

        return (eslint, eslintConfig);
    }

    public static string? FindNodeModules()
    {
        var current = Directory.GetCurrentDirectory();
        var dirs = Directory.GetDirectories(current);
        if (!dirs.Contains("node_modules"))
        {
            var currentInfo = new DirectoryInfo(current);
            while (currentInfo != null && !Directory.Exists(Path.Combine(currentInfo.FullName, "node_modules")))
                currentInfo = currentInfo.Parent;

            return currentInfo == null ? null : Path.Combine(currentInfo.FullName, "node_modules");
        }

        return Path.Combine(current, "node_modules");
    }
}