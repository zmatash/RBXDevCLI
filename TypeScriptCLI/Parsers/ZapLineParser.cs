namespace TypeScriptCLI.Parsers;

public static class ZapLineParser
{
    public static string GetVariableAsString(string line)
    {
        var index = line.IndexOf('=');
        if (index == -1)
            throw new FormatException($"Invalid format: {line}");
        return line.Substring(index + 1).Trim();
    }

    public static (string type, string identifier)? GetVariableTypeAndIdentifier(string line)
    {
        var words = line.Split(" ");
        if (words.Length < 2)
            return null;

        return (words[0], words[1]);
    }

    public static bool GetBoolean(string variableString)
    {
        return variableString.Equals("true", StringComparison.OrdinalIgnoreCase);
    }


    public static string GetString(string variableString)
    {
        return variableString.Trim('"').Trim();
    }
}