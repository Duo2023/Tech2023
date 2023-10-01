using Tech2023.DAL;

namespace Tech2023.Web.Tests;

public class CambridgeTests
{
    [Theory]
    [InlineData(1, Season.Spring, Variant.One, "paper1_spring_v1")]
    public void Format_Works(int paperNumber, Season season, Variant variant, string expected)
    {
        var result = Cambridge.GetString(paperNumber, season, variant);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("paper1_spring_v1", 1, Season.Spring, Variant.One)]
    [InlineData("paper2_summer_v2", 2, Season.Summer, Variant.Two)]
    [InlineData("paper3_winter_v3", 3, Season.Winter, Variant.Three)]
    public void TryParse_Works(string input, int number, Season season, Variant variant)
    {
        var result = Cambridge.TryParseResource(input, out var actualNumber, out var actualSeason, out var actualVariant);

        Assert.True(result, "Parse should return successful");

        Assert.Equal(number, actualNumber);
        Assert.Equal(season, actualSeason);
        Assert.Equal(variant, actualVariant);
    }
}
