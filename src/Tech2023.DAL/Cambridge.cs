using Tech2023.Core;

namespace Tech2023.DAL;

/// <summary>
/// Provides methods for parsing cambridge resource and formatting
/// </summary>
public static class Cambridge
{
    internal const string PaperIdentifier = "paper";
    internal const char Seperator = '_';
    internal const char Version = 'v';

    /// <summary>
    /// Formats the <see cref="CambridgeResource"/> as a string to be passed between routes
    /// </summary>
    /// <remarks>
    /// Convenience method
    /// </remarks>
    public static string GetString(CambridgeResource resource)
    {
        return GetString(resource.Number, resource.Season, resource.Variant);
    }

    /// <summary>
    /// Formats the Cambridge resource items as a string to be passed between routes and so on
    /// </summary>
    /// <param name="number">The paper number of the resource</param>
    /// <param name="season">The season of the resource</param>
    /// <param name="variant">The variant of the resource</param>
    public static string GetString(int number, Season season, Variant variant)
    {
        var builder = new ValueStringBuilder(stackalloc char[32]);

        builder.Append(PaperIdentifier); // paper

        builder.Append((uint)number); // 0-9

        builder.Append(Seperator);  // _

        builder.Append(GetStringForSeason(season)); // spring/winter/summer

        builder.Append(Seperator); // _

        builder.Append(Version); // v

        builder.Append((uint)variant); // 0-9

        return builder.ToString();
    }

    internal const int ExpectedLength = 16; // paper1_summer_v1

    // summer, spring, and winter all have 6 characters. (number and variant are always >=0 & <= 9

    /// <summary>
    /// Tries to parse a string into resource identifiers
    /// </summary>
    [SkipLocalsInit]
    public static bool TryParseResource(ReadOnlySpan<char> str, out int number, out Season season, out Variant variant)
    {
        // we know that the length is always the same because the variant and number should always be one digit 
        // and the season regardless of which one has the same amount of characters
        if (str.Length != ExpectedLength)
        {
            goto Failed;
        }

        // if it doesn't start with 'paper' the string is automatically not valid
        if (!str.StartsWith(PaperIdentifier))
        {
            goto Failed;
        }

        // The paper number is located right after 'paper', quick convert the char to a number
        number = (byte)(str[PaperIdentifier.Length] - '0');

        // validate that the number is a single digit
        if (int.IsNegative(number) || number > 9)
        {
            goto Failed;
        }

        // the seperator should be located right after paper{digit}_ <-
        if (str[PaperIdentifier.Length + 1] != Seperator)
        {
            goto Failed;
        }

        // the season is always 6 characters so take a span and slice it to 6 characters
        if (!TryGetSeasonFromString(str.Slice(PaperIdentifier.Length + 2, 6), out season)) 
        {
            goto Failed;
        }

        // the seperator is located right after the season
        if (str[PaperIdentifier.Length + 8] != Seperator)
        {
            goto Failed;
        }

        // 'v' is located after the seperator
        if (str[PaperIdentifier.Length + 9] != Version)
        {
            goto Failed;
        }

        // do the same number trick as we did above
        variant = (Variant)(byte)(str[PaperIdentifier.Length + 10] - '0');

        if (!Enum.IsDefined(variant))
        {
            goto Failed;
        }

        return true;

Failed:
        number = default;
        season = default;
        variant = default;
        return false;
    }


    internal static string GetStringForSeason(Season season)
    {
        return season switch
        {
            Season.Summer => "summer",
            Season.Spring => "spring",
            Season.Winter => "winter",
            _ => throw new InvalidOperationException(),
        };
    }

    internal static bool TryGetSeasonFromString(ReadOnlySpan<char> input, out Season season)
    {
        switch (input)
        {
            case "summer":
                season = Season.Summer;
                return true;
            case "spring":
                season = Season.Spring;
                return true;
            case "winter":
                season = Season.Winter;
                return true;
        }

        season = default;
        return false;
    }
}
