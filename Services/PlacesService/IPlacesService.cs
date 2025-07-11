using HungrAPI.Services.PlacesService.Dtos;

namespace HungrAPI.Services.PlacesService;

public interface IPlacesService
{
    Task<PlacesDto?> GetRestaurantsAsync(Location location, int radius = 1500);
}