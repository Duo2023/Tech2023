namespace Tech2023.Core.Tests;

public class StringHelperTests
{
    [Theory]
    [ClassData(typeof(SubjectStringSource))]
    public void ToTitleCase_Works(string input, string expected)
    {
        var result = StringHelpers.ToTitleCase(input);

        Assert.Equal(expected, result);
    }
}
