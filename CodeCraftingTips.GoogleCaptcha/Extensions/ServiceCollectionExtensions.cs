// <copyright file="ServiceCollectionExtensions.cs" company="Mario Smolcic">
//
// Copyright (C) Mario Smolcic.
// You are permitted to fork, copy, or use this content in any other form you can imagine.
//
// </copyright>

namespace CodeCraftingTips.GoogleCaptcha.Extensions;

using CodeCraftingTips.GoogleCaptcha.Clients;
using CodeCraftingTips.GoogleCaptcha.Clients.Google;
using CodeCraftingTips.GoogleCaptcha.Settings;
using Google.Cloud.RecaptchaEnterprise.V1;
using Microsoft.Extensions.Options;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCaptchaOptions(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddOptions<CaptchaSettings>()
            .Bind(configuration.GetRequiredSection(CaptchaSettings.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return services;
    }

    public static IServiceCollection AddCaptchaClients(
        this IServiceCollection services,
        IHostEnvironment environment)
    {
        if (environment.IsProduction())
        {
            services.AddSingleton(RecaptchaEnterpriseServiceClient.Create());
            services.AddSingleton<ICaptchaClient, GoogleCaptchaEnterpriseClient>();

            return services;
        }

        services.AddHttpClient<ICaptchaClient, GoogleCaptchaClient>(
            (httpClient, serviceProvider) =>
            {
                var captchaOptions = serviceProvider
                    .GetRequiredService<IOptionsSnapshot<CaptchaSettings>>();

                httpClient.BaseAddress = new Uri(captchaOptions.Value.VerificationUrl);

                var logger = serviceProvider
                    .GetRequiredService<ILogger<GoogleCaptchaClient>>();

                return new GoogleCaptchaClient(httpClient, captchaOptions, logger);
            });

        return services;
    }
}