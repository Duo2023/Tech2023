using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

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

        services.AddApplicationOptions(Configuration );

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration.GetSection("Jwt")["Issuer"],
                    ValidAudience = Configuration.GetSection("Jwt")["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("Jwt")["Secret"]!))
                };
            });
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
