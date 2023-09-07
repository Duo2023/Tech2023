using System.Diagnostics;

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
    public static bool TryParse(ReadOnlySpan<char> input, out CurriculumLevel level, out CurriculumSource source)
    {
        switch (input)
        {
            case "L1":
                level = CurriculumLevel.L1;
                source = CurriculumSource.Ncea;
                return true;
            case "IGSCE":
                level = CurriculumLevel.L1;
                source = CurriculumSource.Cambridge;
                return true;
            case "L2":
                level = CurriculumLevel.L2;
                source = CurriculumSource.Ncea;
                return true;
            case "AS":
                level = CurriculumLevel.L2;
                source = CurriculumSource.Cambridge;
                return true;
            case "L3":
                level = CurriculumLevel.L3;
                source = CurriculumSource.Ncea;
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
            case "L1":
            case "IGSCE":
                level = CurriculumLevel.L1;
                return true;
            case "L2":
            case "AS":
                level = CurriculumLevel.L2;
                return true;
            case "L3":
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
            return level.ToString();
        }
        else return string.Empty;
    }
}
