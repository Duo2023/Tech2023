using Tech2023.DAL;

namespace Tech2023.Benchmarking.Benchmarks;

[MemoryDiagnoser]
public class CurriculumLevelParsing
{
    [Benchmark]
    public void Parse_Both()
    {
        _ = CurriculumLevelHelpers.TryParse("LEVEL1", out _, out _);
        _ = CurriculumLevelHelpers.TryParse("IGSCE", out _, out _);
    }
}
