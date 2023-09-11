using Tech2023.DAL;
using Tech2023.DAL.Models;

namespace Tech2023.Web.Tests;

public class CurriculumTests
{
    [Theory]
    [InlineData("LEVEL3", CurriculumLevel.L3, CurriculumSource.Ncea)]
    public void Assert_TryParseWorks(string input, CurriculumLevel expectedLevel, CurriculumSource curriculumSource)
    {
        bool result = CurriculumLevelHelpers.TryParse(input, out var actualLevel, out var actualSource);

        Assert.True(result); // on fail the parse has failed and both should be default
        Assert.Equal(expectedLevel, actualLevel);
        Assert.Equal(curriculumSource, actualSource);

        // sanity check that both the enum declarations exist
        Assert.True(Enum.IsDefined(expectedLevel));
        Assert.True(Enum.IsDefined(curriculumSource));

        Assert.True(Enum.IsDefined(actualLevel));
        Assert.True(Enum.IsDefined(actualSource));
    }


    // on failure it should return false and it should zero out both of the exit parameters
    [Fact]
    public void Assert_TryParseFailedCorrectly()
    {
        var parseResult = CurriculumLevelHelpers.TryParse("asdfjahsdfsadfjsadoifjasdfojasdf", out var level, out var source);

        Assert.False(parseResult);
        Assert.Equal(default, level);
        Assert.Equal(default, source);
    }
}
