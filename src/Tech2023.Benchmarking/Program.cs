namespace Tech2023.Benchmarking;

/* src/Tech2023.Benchmarking/Program.cs */
/* benchmarks will be automatically discovered and can be chosen to run when executed */

public sealed class Program
{
    public static void Main(string[] args) 
        => BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
}
