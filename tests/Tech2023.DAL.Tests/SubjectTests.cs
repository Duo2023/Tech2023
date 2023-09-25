using System.Collections;
using System.Runtime.CompilerServices;

using Microsoft.EntityFrameworkCore;
using Tech2023.DAL.Models;
using Tech2023.DAL.Queries;

namespace Tech2023.DAL.Tests;

public class SubjectTests
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static ApplicationDbContext CreateStub() => new(new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase("Tests").Options);

    [Theory]
    [ClassData(typeof(SubjectCreationSource))]
    public async Task CreateAsync(string subjectName, CurriculumSource source, CurriculumLevel level)
    {
        Assert.True(Enum.IsDefined(source), TestHelper.GetInvalidMessage(nameof(source)));
        Assert.True(Enum.IsDefined(level), TestHelper.GetInvalidMessage(nameof(level)));

        using var context = CreateStub();

        await Subjects.CreateSubjectAsync(context, new Subject()
        {
            Name = subjectName,
            Source = source,
            Level = level
        });

        var subject = await Subjects.FindSubjectAsync(context, source, level, subjectName);

        Assert.NotNull(subject);

        Assert.Equal(subjectName, subject.Name, ignoreCase: true);

        Assert.True(Enum.IsDefined(subject.Level), "Level from database is not defined");

        Assert.Equal(source, subject.Source);
        Assert.Equal(level, subject.Level);
    }

    [Fact]
    public async Task ShouldFailOnNullRecord()
    {
        using var stubContext = CreateStub();

        await Assert.ThrowsAsync<ArgumentNullException>(async () => await Subjects.CreateSubjectAsync(stubContext, null!));
        await Assert.ThrowsAsync<ArgumentNullException>(async () => await Subjects.CreateSubjectAsync(null!, null!));
    }
}

// file scoped class because we do not need to use this generator anywhere else in this project
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
