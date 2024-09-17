namespace TypeScriptCLI.Parsers;

public static class ZapDefinitionsParser
{
    public static void ParseDefinitionsFile(string pathToFile)
    {
        if (!File.Exists(pathToFile)) throw new FileNotFoundException($"Zap definitions file not found: {pathToFile}");
        var lines = File.ReadAllLines(pathToFile);
        var replacementLines = new List<string>();

        replacementLines.Add("export namespace Zap {");

        foreach (var line in lines)
        {
            var parsedLine = ParseLine(line);
            replacementLines.Add(parsedLine);
        }

        replacementLines.Add("}");

        File.WriteAllLines(pathToFile, replacementLines);
    }


    private static string ParseLine(string line)
    {
        var words = line.Split(" ");
        var editedLine = new List<string>();

        for (var i = 0; i < words.Length; i++)
        {
            var word = words[i];
            if (word == "type" && i == 0 && i != words.Length - 1)
            {
                var nextWord = words[i + 1];
                if (nextWord.EndsWith("Packet")) editedLine.Add("export");
            }
            else if (word == "declare")
            {
                continue;
            }

            editedLine.Add(word);
        }

        return string.Join(" ", editedLine);
    }
}