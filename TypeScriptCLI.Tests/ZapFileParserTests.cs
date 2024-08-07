using TypeScriptCLI.Parsers;

namespace TypeScriptCLI.Tests;

public class ZapFileParserTests
{
    private string _rootDir;

    [SetUp]
    public void Setup()
    {
        var dirInfo = new DirectoryInfo(Directory.GetCurrentDirectory());
        var rootInfo = dirInfo!.Parent!.Parent!.Parent;
        _rootDir = Path.Join(rootInfo!.FullName, "Resources");
    }

    [Test]
    public void ParseZapFileTest()
    {
        var normalisePath = (string path) => path.Replace('/', '\\');

        var zapConfigPath = Path.Join(_rootDir, "zap.config");
        var zap = ZapFileParser.ParseZapFile(zapConfigPath);

        Assert.That(zap, Is.Not.Null);
        Assert.That(zap.Options.TypeScript, Is.True);
        Assert.That(normalisePath(zap.Options.ClientOutput),
            Is.EqualTo(Path.Join("src", "client", "network", "zap.luau")));
        Assert.That(normalisePath(zap.Options.ServerOutput),
            Is.EqualTo(Path.Join("src", "server", "network", "zap.luau")));
        Assert.That(zap.Options.YieldType, Is.EqualTo("promise"));
    }

    [TearDown]
    public void TearDown()
    {
        var files = Directory.GetFiles(Path.Join(_rootDir, "src"), "*", SearchOption.AllDirectories);
        foreach (var file in files) File.Delete(file);
    }
}