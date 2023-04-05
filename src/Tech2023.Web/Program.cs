using Microsoft.Extensions.DependencyInjection;
using Tech2023.Web.Shared;

namespace Tech2023.Web;

public class Program
{
    public static void Main(string[] args)
    {
#if DEBUG
        TailwindReload.Run();
#endif
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        builder.Services.AddHttpClient(Clients.API, client =>
        {
            string uriString = builder.Configuration["ApiUrl"] ?? throw new ConfigurationException("The API url should be configured");

            client.BaseAddress = new(uriString);
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}
