using HungrAPI.Services.PlacesService;
using HungrAPI.Services.PlacesService.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HungrAPI.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class PlacesController(IPlacesService placesService) : ControllerBase
{
    private readonly IPlacesService _placesService = placesService;

    [HttpPost]
    public async Task<IActionResult> GetPlaces([FromBody] Location location)
    {
        var places = await _placesService.GetRestaurantsAsync(location);

        return Ok(places);
    }

    [HttpGet("photo/{photoReference}")]
    public async Task<IActionResult> GetPlacePhoto(string photoReference)
    {
        var photo = await _placesService.GetPlacePhotoAsync(photoReference);

        return File(photo.Stream, photo.ContentType);
    }
}