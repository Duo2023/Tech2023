using System.Text.Json;
using System.Text.Json.Serialization;

using Tech2023.Core.Json.Converters;

namespace Tech2023.Benchmarking;

// This benchmark is to compare our TimeSpanConvert which converts into a TimeSpan into json form using the ticks property instead of serializing to a string

[MemoryDiagnoser]
[BenchmarkCategory(Categories.JSON)]
public class TimeSpanConverterBenchmarks
{
    internal const long TickCount = 1231232131231231;

    [Benchmark]
    public string Serialize()
    {
        var span = TimeSpan.FromTicks(TickCount);

        return JsonSerializer.Serialize(new A() { Span = span });
    }

    [Benchmark]
    public string SerializeWithout()
    {
        var span = TimeSpan.FromTicks(TickCount);

        return JsonSerializer.Serialize(new B() { Span = span });
    }
}

file class A
{
    [JsonConverter(typeof(TimeSpanConverter))]
    public TimeSpan Span { get; set; }
}

file class B
{
    public TimeSpan Span { get; set; }
}
