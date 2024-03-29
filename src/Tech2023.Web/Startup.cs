﻿using System.Reflection;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Tech2023.Core;
using Tech2023.DAL;
using Tech2023.Web.Extensions;
using Tech2023.Web.Initialization;
using Tech2023.Web.Shared;
using Tech2023.Web.Shared.Authentication;
using Tech2023.Web.Shared.Email;
using Tech2023.Web.Shared.Files;
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

        services.Configure<RouteOptions>(options =>
        {
            options.LowercaseUrls = true;
            options.AppendTrailingSlash = true;
        });

        services.AddDbContextFactory<ApplicationDbContext>(options =>
        {
#if DEBUG
            options.EnableDetailedErrors();
            options.EnableSensitiveDataLogging();

            options.UseInMemoryDatabase($"{Assembly.GetExecutingAssembly().FullName}");
#else
            options.UseSqlServer(Configuration.GetConnectionString("Default"), 
                migrations => migrations.MigrationsAssembly("Tech2023.DAL.Migrations"));
#endif
        });

        services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
        {
            options.SignIn.RequireConfirmedAccount = true;
            options.User.RequireUniqueEmail = true;

            options.Password.RequiredLength = AuthConstants.MinPasswordLength;
            options.Password.RequireNonAlphanumeric = false;

        }).AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

        services.AddHttpClient(Clients.Crawler, client =>
        {
            client.BaseAddress = new Uri("https://www.nzqa.govt.nz/"); // change this to a configuration switch potentially
        });

#if DEBUG
        services.AddHostedService<AutoReloadService>();
#endif

        services.AddApplicationOptions(Configuration);

        services.AddMemoryCache();

        services.AddTransient<IEmailClient, EmailClient>();

        services.AddTransient<IEmailConfirmationService<ApplicationUser>, EmailConfirmationService>();

        services.AddTransient<IFileSaver, FileSaver>();

        services.AddTransient<IDataInitializer, Initializer>();
    }

    /// <summary>
    /// Method gets called by the runtime, configures the HTTP request pipeline
    /// </summary>
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IDataInitializer initializer)
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

        app.UseStatusCodePagesWithReExecute(Routes.Error, "?code={0}");

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        });

        Async.RunSync(initializer.InitializeAsync);
    }
}
