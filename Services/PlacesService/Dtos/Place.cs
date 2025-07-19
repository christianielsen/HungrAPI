namespace HungrAPI.Services.PlacesService.Dtos;

public class Place
{
    public string Name { get; set; }
    public string Vicinity { get; set; }
    public Geometry Geometry { get; set; }
    public double? Rating { get; set; }
    public int? UserRatingsTotal { get; set; }
    public PhotoDetails[] Photos { get; set; }
}