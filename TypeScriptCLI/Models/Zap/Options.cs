namespace TypeScriptCLI.Models.Zap;

public class Options
{
    private string _yieldType = "yield";
    public bool TypeScript { get; set; } = false;

    public string YieldType
    {
        get => _yieldType;
        set
        {
            var possibleValues = new[] { "yield", "promise", "future" };
            if (!possibleValues.Contains(value))
                throw new ArgumentException(
                    $"Yield type must be one of: {string.Join(", ", possibleValues)}, not {value}");
            if (TypeScript && value == "future")
                throw new ArgumentException("Yield type cannot be 'future' when TypeScript is enabled");
            _yieldType = value;
        }
    }

    public string ServerOutput { get; set; } = "./network/server.lua";

    public string ClientOutput { get; set; } = "path/to/client/output.lua";

    public string? GetClientTypes()
    {
        var outputExt = Path.GetExtension(ClientOutput);
        var types = outputExt.Replace(outputExt, ".d.ts");
        return File.Exists(types) ? types : null;
    }

    public string? GetServerTypes()
    {
        var outputExt = Path.GetExtension(ServerOutput);
        var types = outputExt.Replace(outputExt, ".d.ts");
        return File.Exists(types) ? types : null;
    }
}