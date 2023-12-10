using System.Text.Json.Serialization;

namespace apex_legends_buddy_api.Models;

public class GamepediaResponseParseText
{
    [JsonPropertyName("*")] public string? Asterisk { get; set; }
}