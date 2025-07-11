namespace HungrAPI.Services.PlacesService.Dtos;

public class PlacesDto
{
    public List<Place> Results { get; set; }
    public string Status { get; set; }
    public string NextPageToken { get; set; }
}