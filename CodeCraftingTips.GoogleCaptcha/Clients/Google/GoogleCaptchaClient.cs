// <copyright file="GoogleCaptchaClient.cs" company="Mario Smolcic">
//
// Copyright (C) Mario Smolcic.
// You are permitted to fork, copy, or use this content in any other form you can imagine.
//
// </copyright>

namespace CodeCraftingTips.GoogleCaptcha.Clients.Google;

using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using CodeCraftingTips.GoogleCaptcha.Clients.Google.Models;
using CodeCraftingTips.GoogleCaptcha.Settings;
using Microsoft.Extensions.Options;

internal class GoogleCaptchaClient : ICaptchaClient
{
    private const decimal _botScoreThreshold = 0.5m;

    private static readonly JsonSerializerOptions _jsonSerializerOptions
        = new() { PropertyNameCaseInsensitive = true };

    private readonly HttpClient _httpClient;
    private readonly IOptionsSnapshot<CaptchaSettings> _captchaOptionsSnapshot;
    private readonly ILogger<GoogleCaptchaClient> _logger;

    public GoogleCaptchaClient(
        HttpClient httpClient,
        IOptionsSnapshot<CaptchaSettings> captchaOptionsSnapshot,
        ILogger<GoogleCaptchaClient> logger)
    {
        _httpClient = httpClient;
        _captchaOptionsSnapshot = captchaOptionsSnapshot;
        _logger = logger;
    }

    public async Task<bool> IsCaptchaValidAsync(
        string captchaToken,
        CancellationToken cancellationToken)
    {
        try
        {
            using var request = new HttpRequestMessage(
                HttpMethod.Post,
                $"?secret={_captchaOptionsSnapshot.Value.SecretKey}&response={captchaToken}");

            using HttpResponseMessage response = await _httpClient.SendAsync(request, cancellationToken);
            var responseJson = await response.Content.ReadAsStringAsync(cancellationToken);

            CaptchaVerificationResponse? captchaVerification =
                JsonSerializer.Deserialize<CaptchaVerificationResponse>(responseJson, _jsonSerializerOptions);

            return IsCaptchaValid(captchaVerification);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Google ReCaptcha validation failed.");
            return false;
        }
    }

    private bool IsCaptchaValid(CaptchaVerificationResponse? response)
    {
        if (response?.Success != true)
        {
            return false;
        }

        return _captchaOptionsSnapshot.Value.Version switch
        {
            CaptchaVersion.V2 => true,
            CaptchaVersion.V3 => response.Score > _botScoreThreshold,
            _ => throw new NotSupportedException(
                $"'{_captchaOptionsSnapshot.Value.Version}' is not supported")
        };
    }
}