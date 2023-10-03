namespace Tech2023.Core.Tests;

public class HtmlHexStringTests
{
    [Theory]
    [ClassData(typeof(HexStringProvider))]
    public void ConvertToString_ReturnsExpectedResult(uint value, string expected)
    {
        string output = HtmlHexString.GetHtmlHexString(value);

        output.Should().NotBeNull();

        output.Should().Be(expected);
    }
}
