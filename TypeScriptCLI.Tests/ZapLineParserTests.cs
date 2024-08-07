using TypeScriptCLI.Parsers;

namespace TypeScriptCLI.Tests;

public class ZapLineParserTests
{
    [TestCase("opt typescript = true", "opt", "typescript")]
    [TestCase("opt server_output = \"src/client/network/zap.luau\"", "opt", "server_output")]
    [TestCase("event TestEvent = {\n    from: Server,\n    type: Reliable,\n    call: ManyAsync,\n    data: i8,\n}",
        "event", "TestEvent")]
    public void ParseTypeAndIdentifierTest(string line, string type, string identifier)
    {
        var result = ZapLineParser.GetVariableTypeAndIdentifier(line);
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo((type, identifier)));
    }

    [TestCase("true", true)]
    [TestCase("false", false)]
    public void GetBooleanTest(string variableString, bool expected)
    {
        var result = ZapLineParser.GetBoolean(variableString);
        Assert.That(result, Is.EqualTo(expected));
    }

    [TestCase("opt typescript = true", "true")]
    [TestCase("opt typescript = false", "false")]
    [TestCase("opt server_output = \"src/client/network/zap.luau\"", "\"src/client/network/zap.luau\"")]
    public void GetVariableAsStringTest(string line, string variableString)
    {
        var result = ZapLineParser.GetVariableAsString(line);
        Assert.That(result, Is.EqualTo(variableString));
    }

    [TestCase("\"src/server/network/zap.luau\"", "src/server/network/zap.luau")]
    [TestCase("\"promise\"", "promise")]
    public void GetStringTest(string variableString, string expected)
    {
        var result = ZapLineParser.GetString(variableString);
        Assert.That(result, Is.EqualTo(expected));
    }
}