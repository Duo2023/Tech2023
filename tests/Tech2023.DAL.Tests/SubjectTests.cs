using System.Collections;

using Microsoft.EntityFrameworkCore;
using Tech2023.DAL.Models;

namespace Tech2023.DAL.Tests;

public class SubjectTests
{
    internal static ApplicationDbContext CreateStub() => new(new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase("Tests").Options);

    [Theory]
    [ClassData(typeof(SubjectCreationSource))]
    public async Task CreateAsync(string subjectName, CurriculumSource source, CurriculumLevel level)
    {
        using var context = CreateStub();

        await Queries.Subjects.CreateSubjectAsync(context, new Subject()
        {
            Name = subjectName,
            Source = source,
            Level = level
        });
    }

    [Fact]
    public async Task ShouldFailOnNullRecord()
    {
        using var stubContext = CreateStub();

        await Assert.ThrowsAsync<ArgumentNullException>(async () => await Queries.Subjects.CreateSubjectAsync(stubContext, null!));
        await Assert.ThrowsAsync<ArgumentNullException>(async () => await Queries.Subjects.CreateSubjectAsync(null!, null!));
    }
}

file class SubjectCreationSource : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        for (int i = 0; i < 50; i++)
        {
            yield return new object[] { TestHelper.GenerateString(Random.Shared.Next(3, 10)), CurriculumSource.Ncea, CurriculumLevel.L3 };
        }

        for (int i = 0; i < 50; i++)
        {
            yield return new object[] { TestHelper.GenerateString(Random.Shared.Next(3, 10)), CurriculumSource.Cambridge, CurriculumLevel.L2 };
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
