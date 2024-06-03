using System;
using System.Text.Json.Serialization;

namespace AuthApp.Domain.Api;

public class PersonApiResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("cpf")]
    public string Cpf { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
}

