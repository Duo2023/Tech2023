using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Tech2023.Web.Shared.Email;

namespace Tech2023.Web.IntegrationTests;

public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup>
    where TStartup : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder.ConfigureServices(services =>
        {
            var emailOptionsDescriptor = services.FirstOrDefault(descriptor =>
                descriptor.ServiceType == typeof(IConfigureOptions<EmailOptions>));

            if (emailOptionsDescriptor != null)
                services.Remove(emailOptionsDescriptor);

            services.Configure<EmailOptions>(options =>
            {
                options.SenderName = "Tests";
                options.SmtpServer = "test-smtp.example.com";
                options.Port = 587;
                options.FromEmail = "test@test.com";
                options.Username = options.FromEmail;
                options.Password = "*********";
            });
        });
    }
}
