using TypeScriptCLI.Models.Zap;

namespace TypeScriptCLI.Parsers;

public class ZapParser
{
    private readonly Dictionary<string, Action<string, string>> _tokens;
    private readonly ZapConfig _zap;

    public ZapParser(string zapConfigPath, ZapConfig zap)
    {
        if (!File.Exists(zapConfigPath)) throw new FileNotFoundException($"Zap config not found at: {zapConfigPath}");
        _zap = zap;
        _tokens = new Dictionary<string, Action<string, string>>
        {
            { "opt", ParseOpt }
        };
    }

    public void ParseZap(string file)
    {
        if (!File.Exists(file)) throw new FileNotFoundException($"Zap config not found at: {file}");

        using var reader = new StreamReader(file);
        while (reader.ReadLine() is { } line)
        {
            var (token, identifier) = GetTokenAndIdentifier(line);
            if (_tokens.TryGetValue(token, out var method)) method(identifier, line);
        }
    }

    private static (string token, string identifier) GetTokenAndIdentifier(string line)
    {
        var firstTwoWords = line.Split(" ");
        return (firstTwoWords[0], firstTwoWords[1]);
    }

    private static int GetEqualsIndex(string line)
    {
        var index = line.IndexOf('=');
        if (index == -1)
            throw new FormatException(
                $"Invalid format: {line}");
        return index;
    }


    private static bool GetBoolType(string line)
    {
        var index = GetEqualsIndex(line);
        var value = line.Substring(index + 1).Trim();
        return value.Equals("true", StringComparison.OrdinalIgnoreCase);
    }

    private static string? GetPathType(string line)
    {
        var index = GetEqualsIndex(line);
        var value = line.Substring(index + 1).Trim();
        return value;
    }

    private void ParseOpt(string identifier, string line)
    {
        switch (identifier)
        {
            case "typescript":
                _zap.Options.TypeScript = GetBoolType(line);
                break;
            case "server_output":
                _zap.Options.ServerOutput = GetPathType(line);
                break;
            case "client_output":
                _zap.Options.ClientOutput = GetPathType(line);
                break;

            default:
                Console.WriteLine($"Unknown option: {identifier}, skipping...");
                return;
        }
    }
}