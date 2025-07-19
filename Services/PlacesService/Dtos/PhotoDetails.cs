using System.Text.Json.Serialization;

namespace HungrAPI.Services.PlacesService.Dtos;

public class PhotoDetails
{
    public int Height { get; set; }
    [JsonPropertyName("html_attributions")]
    public string[] HtmlAttributions { get; set; }
    [JsonPropertyName("photo_reference")]
    public string PhotoReference { get; set; }
    public int Width { get; set; }
}