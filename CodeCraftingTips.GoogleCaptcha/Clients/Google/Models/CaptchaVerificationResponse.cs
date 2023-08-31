// <copyright file="CaptchaVerificationResponse.cs" company="Mario Smolcic">
//
// Copyright (C) Mario Smolcic.
// You are permitted to fork, copy, or use this content in any other form you can imagine.
//
// </copyright>

namespace CodeCraftingTips.GoogleCaptcha.Clients.Google.Models;

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

public record CaptchaVerificationResponse(
    bool Success,
    decimal Score,
    string Action,
    string Hostname,
    [property:JsonPropertyName("challenge_ts")]
    DateTimeOffset ChallengeTimestamp,
    [property: JsonPropertyName("error-codes")]
    List<string> ErrorCodes);