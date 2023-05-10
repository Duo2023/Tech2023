namespace Tech2023.Core.Tests;

public class HtmlHexStringTests
{
    [Theory]
    [InlineData(0x4)]
    public void ConvertToString_ReturnsExpectedResult(uint color)
    {
        string output = HtmlHexString.GetHtmlHexString(color);

        Assert.NotNull(output);
    }
}
