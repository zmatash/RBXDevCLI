using TypeScriptCLI.Models.Zap;

namespace TypeScriptCLI.Parsers;

public static class ZapFileParser
{
    public static Zap ParseZapFile(string pathToFile)
    {
        if (!File.Exists(pathToFile)) throw new FileNotFoundException($"Zap file not found: {pathToFile}");

        var zap = new Zap(pathToFile);

        using var reader = new StreamReader(pathToFile);
        while (reader.ReadLine() is { } line)
        {
            var result = ZapLineParser.GetVariableTypeAndIdentifier(line);
            if (result is null) continue;
            var (type, identifier) = result.Value;

            if (type == "opt") ParseOpt(identifier, line, zap);
        }

        return zap;
    }

    private static void ParseOpt(string optIdentifier, string line, Zap zap)
    {
        var optHandlers = new Dictionary<string, Action<string>>
        {
            { "typescript", s => { zap.Options.TypeScript = ZapLineParser.GetBoolean(s); } },
            { "server_output", s => { zap.Options.ServerOutput = ZapLineParser.GetString(s); } },
            { "client_output", s => { zap.Options.ClientOutput = ZapLineParser.GetString(s); } },
            { "yield_type", s => { zap.Options.YieldType = ZapLineParser.GetString(s); } }
        };
        var variable = ZapLineParser.GetVariableAsString(line);
        if (string.IsNullOrEmpty(variable)) return;
        if (optHandlers.TryGetValue(optIdentifier, out var handler)) handler(variable);
    }
}