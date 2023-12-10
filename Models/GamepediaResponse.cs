using System.Text.Json.Serialization;

namespace apex_legends_buddy_api.Models;

public class GamepediaResponse
{
    [JsonPropertyName("parse")] public GamepediaResponseParse Parse { get; set; } = new();
}