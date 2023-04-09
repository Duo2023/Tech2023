using System.Reflection;

namespace Tech2023.Web.API;

// Entry point of the application that builds the host builder and runs using the Startup class in the Startup.cs file

public sealed class Program
{
    /// <summary>
    /// The main method, the entry point of the application that contains the string arguments passed in
    /// </summary>
    /// <param name="args">The arguments supplied to the application</param>
    public static void Main(string[] args)
    {
        Console.Title = $"{Assembly.GetExecutingAssembly().GetName().Name!} | {((nint.Size == 8) ? "x64" : "x32")}";

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
