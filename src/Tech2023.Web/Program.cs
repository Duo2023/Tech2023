namespace Tech2023.Web;

/// <summary>
/// The program class
/// </summary>
public sealed class Program
{
    /// <summary>
    /// The entry point of the application
    /// </summary>
    /// <param name="args"></param>
    public static void Main(string[] args)
    {
#if DEBUG
        TailwindReload.Run();
#endif

        CreateHostBuilder(args).Build().Run();
    }

    /// <summary>
    /// Creates the host builder for the web application to run
    /// </summary>
    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>();
        });
}
