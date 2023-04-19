using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Microsoft.Extensions.Options;
using Tech2023.Web.Shared;
using Tech2023.Web.Shared.Authentication;
using Tech2023.Web.Shared.Email;

namespace Tech2023.Web.API.Extensions;

/// <summary>
/// Configures application options
/// </summary>
internal static class OptionExtensions
{
    public static IServiceCollection AddApplicationOptions(this IServiceCollection services, IConfiguration configuration)
    {
        Debug.Assert(services != null);
        Debug.Assert(configuration != null);

        services.AddOptions();
        services.ValidateAndConfigure<JwtOptions>(nameof(JwtOptions), configuration);
        services.ValidateAndConfigure<EmailOptions>(nameof(EmailOptions), configuration);

        return services;
    }

    internal static IServiceCollection ValidateAndConfigure<TOptions>(this IServiceCollection services, string sectionName, IConfiguration configuration)
        where TOptions : class
    {
        var section = configuration.GetSection(sectionName);

        services
            .AddOptions<TOptions>()
            .Bind(section)
            .ValidateByDataAnnotation(sectionName);

        return services;
    }

    public static OptionsBuilder<TOptions> ValidateByDataAnnotation<TOptions>(
        this OptionsBuilder<TOptions> builder,
        string sectionName)
        where TOptions : class
    {
        return builder.PostConfigure(x => Validate(x, sectionName));
    }

    internal static void Validate<T>(T instance, string sectionName)
        where T : notnull
    {
        var results = new List<ValidationResult>();

        var context = new ValidationContext(instance);

        if (Validator.TryValidateObject(instance, context, results))
            return;

        throw new ConfigurationException($"{sectionName} was found invalid with the following error(s):\n{string.Join('\n', results.Select(error => error.ErrorMessage))}");
    }
}
