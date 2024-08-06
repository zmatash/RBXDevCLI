namespace TypeScriptCLI.Models.Zap;

public class ZapOptions
{
    private string? _clientOutput;
    private string? _serverOutput;
    private bool _typeScript;


    public string? ClientOutput
    {
        get => _clientOutput;
        set
        {
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("ClientOutput cannot be null or empty.");
            _clientOutput = value;
            if (TypeScript) ClientTypes = _clientOutput.Replace(".luau", ".d.ts");
        }
    }

    public string? ClientTypes { get; private set; }

    public string? ServerOutput
    {
        get => _serverOutput;
        set
        {
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("ServerOutput cannot be null or empty.");
            _serverOutput = value;
            if (TypeScript) ServerTypes = _serverOutput.Replace(".luau", ".d.ts");
        }
    }

    public string? ServerTypes { get; private set; }

    public bool TypeScript
    {
        get => _typeScript;
        set
        {
            _typeScript = value;
            if (!string.IsNullOrWhiteSpace(_clientOutput) && value)
                ClientTypes = _clientOutput.Replace(".luau", ".d.ts");

            if (!string.IsNullOrWhiteSpace(_serverOutput) && value)
                ServerTypes = _serverOutput.Replace(".luau", ".d.ts");
        }
    }
}