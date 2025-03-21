﻿using System.Text.Json.Serialization;

namespace Sameposty.Services.FacebookTokenManagerService.Models;
public class FacebookCategory
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
}
