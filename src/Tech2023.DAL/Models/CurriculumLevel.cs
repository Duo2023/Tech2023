using System.Diagnostics;
using Tech2023.Core;
using Tech2023.DAL.Models;

namespace Tech2023.DAL;

public enum CurriculumLevel : byte
{
    /// <summary>Either NCEA L1 or Cambridge IGSCE</summary>
    L1 = 1,

    /// <summary>Either NCEA L2 or Cambridge AS/A</summary>
    L2 = 2,

    /// <summary>Either NCEA L3 or Cambridge A2</summary>
    L3 = 3
}

public static class CurriculumLevelHelpers
{
    const string NceaSpecifier = "LEVEL";

    public static bool TryParse(ReadOnlySpan<char> input, out CurriculumLevel level, out CurriculumSource source)
    {
        if (input.StartsWith("LEVEL") && input.Length == NceaSpecifier.Length + 1)
        {
            var number = (CurriculumLevel)byte.Parse(input[NceaSpecifier.Length..]);

            if (!Enum.IsDefined(number))
            {
                level = default;
                source = default;
                return false;
            }

            level = number;
            source = CurriculumSource.Ncea;
            return true;
        }

        switch (input)
        {
            case "IGSCE":
                level = CurriculumLevel.L1;
                source = CurriculumSource.Cambridge;
                return true;
            case "AS":
                level = CurriculumLevel.L2;
                source = CurriculumSource.Cambridge;
                return true;
            case "A2":
                level = CurriculumLevel.L3;
                source = CurriculumSource.Cambridge;
                return true;
            default:
                level = default;
                source = default;
                return false;
        }
    }

    public static bool TryParse(ReadOnlySpan<char> input, out CurriculumLevel level)
    {
        switch (input)
        {
            case "LEVEL1":
            case "IGSCE":
                level = CurriculumLevel.L1;
                return true;
            case "LEVEL2":
            case "AS":
                level = CurriculumLevel.L2;
                return true;
            case "LEVEL3":
            case "A2":
                level = CurriculumLevel.L3;
                return true;
            default:
                level = default;
                return false;
        }
    }

    public static string ToString(CurriculumLevel level, CurriculumSource source)
    {
        Debug.Assert(Enum.IsDefined(source));

        if (source == CurriculumSource.Cambridge)
        {
            return level switch
            {
                CurriculumLevel.L1 => "IGSCE",
                CurriculumLevel.L2 => "AS",
                CurriculumLevel.L3 => "A2",
                _ => string.Empty,
            };
        }
        else if (source == CurriculumSource.Ncea)
        {
            return $"LEVEL{(byte)level}";
        }
        else return string.Empty;
    }
}
