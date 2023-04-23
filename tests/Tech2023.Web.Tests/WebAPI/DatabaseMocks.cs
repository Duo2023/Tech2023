using Microsoft.EntityFrameworkCore;
using Tech2023.DAL;

namespace Tech2023.Web.Tests.WebAPI;

internal class TestDbContextFactory : IDbContextFactory<ApplicationDbContext>
{
    internal readonly DbContextOptions<ApplicationDbContext> _options;

    public TestDbContextFactory(DbContextOptions<ApplicationDbContext> options)
    {
        _options = options;
    }

    public ApplicationDbContext CreateDbContext()
    {
        return new(_options);
    }
}

internal class DbOptions
{
    internal static readonly DbContextOptions<ApplicationDbContext> _options
        = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase("TestDb").Options;


    public static DbContextOptions<ApplicationDbContext> Get()
    {
        return _options;
    }
}
