using Tech2023.DAL;

namespace Tech2023.Web.Tests;

public class CambridgeTests
{
    [Theory]
    [InlineData(1, Season.Spring, Variant.One, "1_Spring_One")]
    public void Format_Works(int paperNumber, Season season, Variant variant, string expected)
    {
        var result = Cambridge.GetString(paperNumber, season, variant);

        Assert.Equal(expected, result);
    }
}
