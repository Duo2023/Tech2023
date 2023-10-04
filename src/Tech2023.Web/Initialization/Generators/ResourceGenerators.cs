using Tech2023.DAL;
using Tech2023.DAL.Models;

namespace Tech2023.Web.Initialization.Generators;

// a generator to generate random cambridge resources

internal readonly struct CambridgeResourceGenerator : IGenerator<CambridgeResource>
{
    public CambridgeResource Generate()
    {
        var resource = new CambridgeResource()
        {
            Season = (Season)Random.Shared.Next((int)Season.Spring, (int)Season.Winter), // generate between the min and max to generate across the ranges for enums
            Variant = (Variant)Random.Shared.Next((int)Variant.One, (int)Variant.Three),
            Number = Random.Shared.Next(1, 6),
            Created = DateTimeOffset.UtcNow,
        };

        resource.SyncLatest();

        return resource;
    }
}

// a generator to generate random ncea resources

internal readonly struct NceaResourceGenerator : IGenerator<NceaResource>
{
    public NceaResource Generate()
    {
        var resource = new NceaResource()
        {
            AssessmentType = (NceaAssessmentType)Random.Shared.Next((int)NceaAssessmentType.Internal, (int)NceaAssessmentType.Unit),
            AchievementStandard = Random.Shared.Next(1, ushort.MaxValue),
            Description = "An achievement standard",
        };

        resource.SetToCurrent();

        return resource;
    }
}
