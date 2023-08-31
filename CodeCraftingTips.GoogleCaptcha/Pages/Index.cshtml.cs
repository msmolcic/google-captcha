// <copyright file="Index.cshtml.cs" company="Mario Smolcic">
//
// Copyright (C) Mario Smolcic.
// You are permitted to fork, copy, or use this content in any other form you can imagine.
//
// </copyright>

namespace CodeCraftingTips.GoogleCaptcha.Pages;

using System.Diagnostics.CodeAnalysis;
using CodeCraftingTips.GoogleCaptcha.Clients;
using CodeCraftingTips.GoogleCaptcha.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

[SuppressMessage(
    "StyleCop.CSharp.DocumentationRules",
    "SA1649:File name should match first type name",
    Justification = "Rule does not apply for page files.")]
public class IndexModel : PageModel
{
    private readonly ICaptchaClient _captchaClient;
    private readonly IOptionsSnapshot<CaptchaSettings> _captchaOptionsSnapshot;

    public IndexModel(
        ICaptchaClient captchaClient,
        IOptionsSnapshot<CaptchaSettings> captchaOptionsSnapshot)
    {
        _captchaClient = captchaClient;
        _captchaOptionsSnapshot = captchaOptionsSnapshot;
    }

    [BindProperty(Name = "g-recaptcha-response")]
    public string RecaptchaToken { get; set; } = default!;

    public string ReCaptchaSiteKey => _captchaOptionsSnapshot.Value.SiteKey;
    public CaptchaVersion ReCaptchaVersion => _captchaOptionsSnapshot.Value.Version;

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        bool isValid = await _captchaClient
            .IsCaptchaValidAsync(RecaptchaToken, cancellationToken);

        ViewData["Message"] = isValid
            ? "Verification Successful! User is likely human."
            : "Verification Failed! User might be a bot.";

        return Page();
    }
}