using System.Globalization;

using Tech2023.Core;

namespace Tech2023.Benchmarking.Benchmarks;

[MemoryDiagnoser]
public class StringConversionBenchmarks
{
    internal const string Input = "MATHEMATICS";

    [Benchmark]
    public string Naiive()
    {
        return Impl.ToTitleCase(Input);
    }

    [Benchmark]
    public string Optimized()
    {
        return StringHelpers.ToTitleCase(Input);
    }
}


file class Impl
{
    public static string ToTitleCase(string input)
    {
        return CultureInfo.InvariantCulture.TextInfo.ToTitleCase(input.ToLower());
    }
}
