// <copyright file="CaptchaSettings.cs" company="Mario Smolcic">
//
// Copyright (C) Mario Smolcic.
// You are permitted to fork, copy, or use this content in any other form you can imagine.
//
// </copyright>

namespace CodeCraftingTips.GoogleCaptcha.Settings;

using System.ComponentModel.DataAnnotations;

public class CaptchaSettings
{
    public const string SectionName = "ReCaptcha";

    public string SiteKey { get; set; } = default!;

    public string SecretKey { get; set; } = default!;

    public string VerificationUrl { get; set; } = default!;

    [EnumDataType(typeof(CaptchaVersion))]
    public CaptchaVersion Version { get; set; } = default!;
}