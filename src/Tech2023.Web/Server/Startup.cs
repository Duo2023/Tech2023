using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

namespace Tech2023.Web.Server;

public sealed class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not found"));
        });

        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddDefaultIdentity<ApplicationUser>(options =>
        {
            options.SignIn.RequireConfirmedAccount = true;
        }).AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddIdentityServer()
            .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

        services.AddAuthentication()
            .AddIdentityServerJwt();

        services.AddControllersWithViews();

        services.AddRazorPages();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
            app.UseWebAssemblyDebugging();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseBlazorFrameworkFiles();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseIdentityServer();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapRazorPages();
            endpoints.MapControllers();

            endpoints.MapFallbackToFile("index.html");
        });
    }
}
