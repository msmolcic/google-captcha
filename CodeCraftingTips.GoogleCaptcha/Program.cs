// <copyright file="Program.cs" company="Mario Smolcic">
//
// Copyright (C) Mario Smolcic.
// You are permitted to fork, copy, or use this content in any other form you can imagine.
//
// </copyright>

using CodeCraftingTips.GoogleCaptcha.Extensions;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

builder.Services
    .AddCaptchaOptions(builder.Configuration)
    .AddCaptchaClients(builder.Environment)
    .AddRazorPages();

var application = builder.Build();

if (!application.Environment.IsDevelopment())
{
    application
        .UseExceptionHandler("/Error")
        .UseHsts();
}

application
    .UseHttpsRedirection()
    .UseStaticFiles()
    .UseRouting()
    .UseAuthorization();

application.MapRazorPages();

application.Run();