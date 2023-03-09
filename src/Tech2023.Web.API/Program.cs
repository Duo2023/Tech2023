namespace Tech2023.Web.API;

// Entry point of the application that builds the host builder and runs using the Startup class in the Startup.cs file

public sealed class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    /// <summary>
    /// Creates the host builder using the specified args
    /// </summary>
    /// <param name="args">The string arguments for the web app</param>
    /// <returns><see cref="IHostBuilder"/> the host builder to build and run</returns>
    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(builder =>
        {
            builder.UseStartup<Startup>();
        });
}
