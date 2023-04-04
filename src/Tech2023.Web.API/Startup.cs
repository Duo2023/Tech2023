﻿using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Tech2023.DAL;
using Tech2023.Web.API.Extensions;
using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Tech2023.Web.Shared.Authentication;
using Tech2023.Web.Shared.Authenticaton;
using Tech2023.Web.Shared;

namespace Tech2023.Web.API;

/// <summary>
/// The startup class of the web api, this configures the middeware and services of the web api
/// This also adds the authentication services to this web api
/// </summary>
public sealed class Startup
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Startup"/> class with the specified <see cref="IConfiguration"/>
    /// </summary>
    /// <param name="configuration">Configuration which holds application settings</param>
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    /// <summary>
    /// Configuration for the startup class and the rest of the application
    /// </summary>
    public IConfiguration Configuration { get; }

    /// <summary>
    /// Adds services to the service collection to be used in dependency injection
    /// </summary>
    /// <param name="services">The collection of service descriptors that the web api uses</param>
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        services.AddApplicationOptions(Configuration);

        services.AddDbContextFactory<ApplicationDbContext>(options =>
        {
#if DEBUG
            options.UseInMemoryDatabase($"{Assembly.GetExecutingAssembly().FullName}");
#else
            options.UseSqlServer(Configuration.GetConnectionString("Default"), 
                migrations => migrations.MigrationsAssembly("Tech2023.DAL.Migrations"));
#endif
        });

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration.GetJwtOption("Issuer"),
                    ValidAudience = Configuration.GetJwtOption("Audience"),
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetJwtOption("Secret")!))
                };
            });

        services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
        {
            options.User.RequireUniqueEmail = false;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

        services.AddTransient<IClaimsService, ClaimsService>();
        services.AddTransient<IJwtTokenService, JwtTokenService>();
    }

    /// <summary>
    /// Configures the application environment
    /// </summary>
    /// <param name="app">The application itself</param>
    /// <param name="env">The environment of the web application</param>
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
