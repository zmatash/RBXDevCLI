// See https://aka.ms/new-console-template for more information


using TypeScriptCLI.CLI;
using TypeScriptCLI.Models.Zap;

namespace TypeScriptCLI;

internal class Program
{
    private static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            StartGui();
            return;
        }

        var command = args[0];
        switch (command)
        {
            case "zap":
                ZapCommand(args.Skip(1).ToArray());
                break;
        }
    }

    private static void StartGui()
    {
        var options = new Dictionary<int, string>
        {
            { 1, "Zap" }
        };

        var isRunning = true;
        while (isRunning)
        {
            var userInput = Cli.UseNumericalMenu(options);
            switch (userInput)
            {
                case 1:
                    isRunning = false;
                    break;
            }  
        }  

        Console.Clear();
        Console.WriteLine("Exiting...");
    }

    private static void ZapCommand(string[] args)
    {
        var zapFile = args[0];
        if (!File.Exists(zapFile))
        {
            Console.WriteLine($"File not found: {Path.Join(Directory.GetCurrentDirectory(), zapFile)}");
            return;
        }

        var zap = Zap.FromFile(zapFile);
        zap.Run();
    }


}