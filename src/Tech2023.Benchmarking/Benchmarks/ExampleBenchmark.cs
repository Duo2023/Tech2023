namespace Tech2023.Benchmarking;

[MemoryDiagnoser] // add to see memory usage
public class ExampleBenchmark
{
    [Benchmark(Baseline = true)] // add to include method to test
    public bool Method()
    {
        return true;
    }
}
