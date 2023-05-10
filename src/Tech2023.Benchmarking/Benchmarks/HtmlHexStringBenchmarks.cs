using Tech2023.Core;

namespace Tech2023.Benchmarking;

[MemoryDiagnoser]
public class HtmlHexStringBenchmarks
{
    internal const uint Code = 0xFF_FF_AE_9F; // #ffffae9f
    [Benchmark]
    public string Implementation()
    {
        return HtmlHexString.GetHtmlHexString(Code);
    }

    [Benchmark]
    public string Naiive()
    {
        return $"#{Code:x}";
    }
}
