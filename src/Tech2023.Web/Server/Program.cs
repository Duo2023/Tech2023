using System.Reflection;
using Tech2023.Web.Server;

namespace Tech2023.Web;

public sealed class Program
{
    public static void Main(string[] args)
    {
        Console.Title = Assembly.GetExecutingAssembly().GetName().FullName;

        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
        => Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(builder =>
        {
            builder.UseStartup<Startup>();
        });
}
