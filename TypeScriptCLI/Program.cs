// See https://aka.ms/new-console-template for more information

var filePath = "H:\\NVIDIA_SHIELD\\TV\\House Of The Dragon\\S02\\zap.config";


using var reader = new StreamReader(filePath);
while (reader.ReadLine() is { } line)
{
    Console.WriteLine(line);
}