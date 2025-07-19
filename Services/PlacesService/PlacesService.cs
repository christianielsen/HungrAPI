using HungrAPI.Services.PlacesService.Dtos;

namespace HungrAPI.Services.PlacesService;

public class PlacesService(IHttpClientFactory httpClientFactory, IConfiguration configuration) : IPlacesService
{
    private readonly HttpClient _httpClient =
        httpClientFactory.CreateClient(Constants.HttpClient.AuthenticatedGoogleApiClient);

    private readonly string _apiKey = configuration["Google:ApiKey"];

    public async Task<PlacesDto?> GetRestaurantsAsync(Location location, int radius = 1500)
    {
        var url = $"nearbysearch/json" +
                  $"?location={location.Latitude},{location.Longitude}" +
                  $"&radius={5000}" +
                  $"&type=restaurant" +
                  $"&keyword=halal" +
                  $"&key={_apiKey}";
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<PlacesDto>();
    }

    public async Task<PhotoDto> GetPlacePhotoAsync(string photoReference)
    {
        var url = $"photo?maxwidth=400&photo_reference={photoReference}&key={_apiKey}";
        var response = await _httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
        response.EnsureSuccessStatusCode();

        var contentType = response.Content.Headers.ContentType?.MediaType ?? "image/jpeg";
        var stream = await response.Content.ReadAsStreamAsync();

        return new PhotoDto
        {
            ContentType = contentType,
            Stream = stream,
        };
    }
}