// <copyright file="ICaptchaClient.cs" company="Mario Smolcic">
//
// Copyright (C) Mario Smolcic.
// You are permitted to fork, copy, or use this content in any other form you can imagine.
//
// </copyright>

namespace CodeCraftingTips.GoogleCaptcha.Clients;

public interface ICaptchaClient
{
    Task<bool> IsCaptchaValidAsync(string captchaToken, CancellationToken cancellationToken);
}