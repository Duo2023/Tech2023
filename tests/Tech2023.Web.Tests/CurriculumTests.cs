using Tech2023.DAL;
using Tech2023.DAL.Models;

namespace Tech2023.Web.Tests;

public class CurriculumTests
{
    [Theory]
    [InlineData("A2", CurriculumLevel.L3, CurriculumSource.Cambridge)]
    [InlineData("AS", CurriculumLevel.L2, CurriculumSource.Cambridge)]
    [InlineData("IGCSE", CurriculumLevel.L1, CurriculumSource.Cambridge)]
    [InlineData("LEVEL3", CurriculumLevel.L3, CurriculumSource.Ncea)]
    [InlineData("LEVEL2", CurriculumLevel.L2, CurriculumSource.Ncea)]
    [InlineData("LEVEL1", CurriculumLevel.L1, CurriculumSource.Ncea)]
    public void Assert_TryParseWorks(string input, CurriculumLevel expectedLevel, CurriculumSource curriculumSource)
    {
        expectedLevel.Should().BeDefined("Enum type should have a declaration for this value");

        curriculumSource.Should().BeDefined("Enum type should have a declaration for this value");

        bool result = Curriculum.TryParse(input, out var actualLevel, out var actualSource);

        result.Should().BeTrue("Parse should return true for success");

        actualLevel.Should().HaveSameValueAs(expectedLevel, "Expected level should match actual level");

        actualSource.Should().HaveSameValueAs(curriculumSource, "Source should be the same");
    }


    // on failure it should return false and it should zero out both of the exit parameters
    [Fact]
    public void Assert_TryParseFailedCorrectly()
    {
        var parseResult = Curriculum.TryParse("asdfjahsdfsadfjsadoifjasdfojasdf", out var level, out var source);

        parseResult.Should().BeFalse("Bad input is provided");

        level.Should().Be(default, "The function should have it's out parameters set to default");

        source.Should().Be(default, "The function should have it's out parameters set to default");
    }
}
