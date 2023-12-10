using System.Text.Json.Serialization;

namespace apex_legends_buddy_api.Models;

public class GamepediaResponseParse
{
    [JsonPropertyName("title")] public string Title { get; set; } = string.Empty;
    [JsonPropertyName("text")] public GamepediaResponseParseText Text { get; set; } = new();
}