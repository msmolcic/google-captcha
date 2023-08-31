// <copyright file="GoogleCaptchaEnterpriseClient.cs" company="Mario Smolcic">
//
// Copyright (C) Mario Smolcic.
// You are permitted to fork, copy, or use this content in any other form you can imagine.
//
// </copyright>

namespace CodeCraftingTips.GoogleCaptcha.Clients.Google;

using System.Threading;
using System.Threading.Tasks;
using CodeCraftingTips.GoogleCaptcha.Clients;
using CodeCraftingTips.GoogleCaptcha.Settings;
using global::Google.Cloud.RecaptchaEnterprise.V1;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

internal class GoogleCaptchaEnterpriseClient : ICaptchaClient
{
    private readonly RecaptchaEnterpriseServiceClient _client;
    private readonly IOptionsSnapshot<CaptchaSettings> _captchaOptionsSnapshot;
    private readonly ILogger<GoogleCaptchaEnterpriseClient> _logger;

    public GoogleCaptchaEnterpriseClient(
        RecaptchaEnterpriseServiceClient client,
        IOptionsSnapshot<CaptchaSettings> captchaOptionsSnapshot,
        ILogger<GoogleCaptchaEnterpriseClient> logger)
    {
        _client = client;
        _captchaOptionsSnapshot = captchaOptionsSnapshot;
        _logger = logger;
    }

    public async Task<bool> IsCaptchaValidAsync(
        string captchaToken,
        CancellationToken cancellationToken)
    {
        try
        {
            var request = new CreateAssessmentRequest
            {
                Assessment = new Assessment()
                {
                    Event = new Event
                    {
                        Token = captchaToken,
                        SiteKey = _captchaOptionsSnapshot.Value.SiteKey
                    }
                },
                Parent = _captchaOptionsSnapshot.Value.VerificationUrl
            };

            Assessment response = await _client.CreateAssessmentAsync(request);

            return response.TokenProperties.Valid;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Google ReCaptcha validation failed.");
            return false;
        }
    }
}