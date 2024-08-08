namespace TypeScriptCLI.CLI;

public static class Cli
{
    public static int UseNumericalMenu(Dictionary<int, string> options)
    {
        int? response = null;
        while (!response.HasValue || !options.ContainsKey(response.Value))
        {
            Console.WriteLine("Select an option:");
            Console.WriteLine(string.Concat(Enumerable.Repeat("-", 17)));

            Console.WriteLine("Q. Quit Application");
            foreach (var entry in options) Console.WriteLine($"{entry.Key}. {entry.Value}");

            var input = Console.ReadLine()?.ToLower();
            switch (input)
            {
                case null:
                    continue;
                case "q":
                    Environment.Exit(0);
                    break;
            }

            try
            {
                response = int.Parse(input);
            }
            catch (FormatException)
            {
                Console.Clear();
                Console.WriteLine("Invalid input, enter a number option.\n");
                continue;
            }

            if (options.ContainsKey(response.Value)) break;
            Console.Clear();
            Console.WriteLine("Invalid option, enter a number option.\n");
        }

        return response.Value;
    }
}