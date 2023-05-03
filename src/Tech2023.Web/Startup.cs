﻿using Tech2023.Web.Shared;
#if DEBUG
    using Tech2023.Web.Workers;
#endif

namespace Tech2023.Web;

/// <summary>
/// The startup class of the application
/// </summary>
public sealed class Startup
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Startup"/> class
    /// </summary>
    /// <param name="configuration">The configuration to be used</param>
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    /// <summary>
    /// The configuration used by the startup
    /// </summary>
    public IConfiguration Configuration { get; }

    /// <summary>
    /// Called by the runtime to add services to the service container
    /// </summary>
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMvc().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.AddContext<WebSerializationContext>();
        });

        services.AddHttpClient(Clients.API, client =>
        {
            string uriString = Environment.GetEnvironmentVariable("API_BASE_URL") ?? throw new ConfigurationException("The API url should be configured");

            client.BaseAddress = new Uri(uriString);
        });

#if DEBUG
        services.AddHostedService<AutoReloadService>();
#endif
    }

    /// <summary>
    /// Method gets called by the runtime, configures the HTTP request pipeline
    /// </summary>
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        });
    }
}
