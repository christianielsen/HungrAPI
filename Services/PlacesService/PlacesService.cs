using HungrAPI.Services.PlacesService.Dtos;

namespace HungrAPI.Services.PlacesService;

public class PlacesService(IHttpClientFactory httpClientFactory, IConfiguration configuration) : IPlacesService
{
    private readonly HttpClient _httpClient =
        httpClientFactory.CreateClient(Constants.HttpClient.AuthenticatedGoogleApiClient);
    
    public async Task<PlacesDto?> GetRestaurantsAsync(Location location, int radius = 1500)
    {
        var apiKey = configuration["Google:ApiKey"];

        var url = $"nearbysearch/json" +
                  $"?location={location.Latitude},{location.Longitude}" +
                  $"&radius={radius}" +
                  $"&type=restaurant" +
                  $"&keyword=halal" +
                  $"&key={apiKey}";
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        
        return await response.Content.ReadFromJsonAsync<PlacesDto>();
    }
}