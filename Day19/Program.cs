// See https://aka.ms/new-console-template for more information
using Day19;

DayClass day = new DayClass();

var watch = new System.Diagnostics.Stopwatch();
watch.Start();


day.Part1();
day.Part2();

watch.Stop();
Console.WriteLine("Execution Time: {0} ms", watch.ElapsedMilliseconds);

Console.Write("Press Enter to continue...");
Console.ReadLine();
