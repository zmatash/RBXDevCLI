using TypeScriptCLI.Models.Zap;
using TypeScriptCLI.Parsers;

namespace TypeScriptCLI.Tests;

public class ZapFileParserTests
{
    private string _rootDir;
    private Zap _zap;

    [SetUp]
    public void Setup()
    {
        var dirInfo = new DirectoryInfo(Directory.GetCurrentDirectory());
        var rootInfo = dirInfo!.Parent!.Parent!.Parent;
        _rootDir = Path.Join(rootInfo!.FullName, "Resources");

        var zapConfigPath = Path.Join(_rootDir, "zap.config");
        _zap = ZapFileParser.ParseZapFile(zapConfigPath);
    }

    [Test]
    public void ParseZapFile_ShouldNotBeNull()
    {
        Assert.That(_zap, Is.Not.Null);
    }

    [Test]
    public void ParseZapFile_ShouldHaveTypeScriptOption()
    {
        Assert.That(_zap.Options.TypeScript, Is.True);
    }

    [Test]
    public void ParseZapFile_ShouldHaveCorrectClientOutputPath()
    {
        Assert.That(NormalisePath(_zap.Options.ClientOutput),
            Is.EqualTo(Path.Join("src", "client", "network", "zap.luau")));
    }

    [Test]
    public void ParseZapFile_ShouldHaveCorrectServerOutputPath()
    {
        Assert.That(NormalisePath(_zap.Options.ServerOutput),
            Is.EqualTo(Path.Join("src", "server", "network", "zap.luau")));
    }

    [Test]
    public void ParseZapFile_ShouldHaveCorrectYieldType()
    {
        Assert.That(_zap.Options.YieldType, Is.EqualTo("promise"));
    }

    [TearDown]
    public void TearDown()
    {
        var files = Directory.GetFiles(Path.Join(_rootDir, "src"), "*", SearchOption.AllDirectories);
        foreach (var file in files) File.Delete(file);
    }

    private static string NormalisePath(string path)
    {
        return path.Replace('/', '\\');
    }
}