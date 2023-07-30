namespace Tech2023.Core.Tests;

public class HtmlHexStringTests
{
    [Theory]
    [ClassData(typeof(HexStringProvider))]
    public void ConvertToString_ReturnsExpectedResult(uint value, string expected)
    {
        string output = HtmlHexString.GetHtmlHexString(value);

        Assert.NotNull(output);
        Assert.Equal(expected, output);
    }
}
