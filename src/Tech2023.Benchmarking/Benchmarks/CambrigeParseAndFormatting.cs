using System.Text.RegularExpressions;

using Tech2023.DAL;

namespace Tech2023.Benchmarking.Benchmarks;

[MemoryDiagnoser]
public class CambridgeParsingAndFormatting
{
    const string Value = "paper1_summer_v2";

    [Benchmark]
    public void Parse()
    {
        bool result = Cambridge.TryParseResource(Value, out _, out _, out _);

        if (!result) { throw new Exception(); }
    }

    [Benchmark]
    public void RegexParse()
    {
        bool result = Impl.TryParseString(Value, out _, out _, out _);

        if (!result) { throw new Exception(); }
    }
}

internal partial class Impl
{
    public static bool TryParseString(string input, out int number, out Season seasonValue, out byte variant)
    {
        number = 0;
        seasonValue = default;
        variant = 0;

        if (string.IsNullOrEmpty(input))
            return false;

        var match = MyRegex().Match(input);

        if (!match.Success)
            return false;

        if (!int.TryParse(match.Groups[1].Value, out number))
            return false;

        var seasonStr = match.Groups[2].Value.ToLower();

        switch (seasonStr)
        {
            case "summer":
                seasonValue = Season.Summer;
                break;
            case "winter":
                seasonValue = Season.Winter;
                break;
            case "spring":
                seasonValue = Season.Spring;
                break;
            default:
                return false;
        }

        if (!byte.TryParse(match.Groups[3].Value, out variant))
            return false;

        if (variant > 9)
            return false;

        return true;
    }

    [GeneratedRegex("paper(\\d+)_(\\w+)_v(\\d)")]
    private static partial Regex MyRegex();
}
