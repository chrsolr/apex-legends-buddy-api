using System.Text.Json.Serialization;

public class GamepediaResponse
{
    [JsonPropertyName("parse")]
    public GamepediaResponseParse Parse { get; set; } = new GamepediaResponseParse();
}

public class GamepediaResponseParse
{
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("text")]
    public GamepediaResponseParseText Text { get; set; } = new GamepediaResponseParseText();
}

public class GamepediaResponseParseText
{
    [JsonPropertyName("*")]
    public string? Asterisk { get; set; } = string.Empty;
}
