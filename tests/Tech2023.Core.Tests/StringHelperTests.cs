namespace Tech2023.Core.Tests;

public class StringHelperTests
{
    [Theory(DisplayName = "StringHelpers.ToTitleCase Functionality")]
    [ClassData(typeof(SubjectStringSource))]
    public void ToTitleCase_Works(string input, string expected)
    {
        var result = StringHelpers.ToTitleCase(input);

        Assert.Equal(expected, result);
    }

    [Fact(DisplayName = "Null argument should return string.Empty")]
    public void ToTitleOnNull_ReturnsExpected()
    {
        var result = StringHelpers.ToTitleCase(null!); // override annotations

        Assert.Equal(string.Empty, result);
    }

    [Fact(DisplayName = "StringHelpers.ToTitleCase should not stack overflow")]
    public void CheckLargeInput_ForStackOverflow()
    {
        string str = new string('1', 100_000); // would def cause stack overflow

        _ = StringHelpers.ToTitleCase(str);
    }
}
