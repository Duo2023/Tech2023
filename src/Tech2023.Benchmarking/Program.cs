namespace Tech2023.Benchmarking;

/* src/Tech2023.Benchmarking/Program.cs */
/* benchmarks will be automatically discovered and can be chosen to run when executed */

public sealed class Program
{
    public static void Main(string[] args)
    {
#if DEBUG
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine("The executable cannot be run as it was built using Debug configuration instead of Release configuration. Please rebuild the executable using the Release configuration and try again.");
        Environment.Exit(0);
#endif

        BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
    }
}
