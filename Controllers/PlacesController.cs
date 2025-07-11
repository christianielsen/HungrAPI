using HungrAPI.Services.PlacesService;
using HungrAPI.Services.PlacesService.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HungrAPI.Controllers;

[AllowAnonymous]
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
}