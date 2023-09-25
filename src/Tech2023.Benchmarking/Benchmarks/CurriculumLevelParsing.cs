using Tech2023.DAL;

namespace Tech2023.Benchmarking.Benchmarks;

[MemoryDiagnoser]
public class CurriculumLevelParsing
{
    [Benchmark]
    public void Parse_Both()
    {
        _ = Curriculum.TryParse("LEVEL1", out _, out _);
        _ = Curriculum.TryParse("IGSCE", out _, out _);
    }
}
