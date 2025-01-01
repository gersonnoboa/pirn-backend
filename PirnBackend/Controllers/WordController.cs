using Microsoft.AspNetCore.Mvc;
using PirnBackend.Services;

namespace PirnBackend.Controllers;

[ApiController]
[Route("[controller]")]
public class WordController(ILogger<WordController> logger) : ControllerBase
{
    private readonly ILogger<WordController> _logger = logger;
    private const string BaseUrl = "https://api.sonapi.ee/v2/";
    private readonly IHttpService _httpService = new HttpService(logger);
    
    [HttpGet(template: "{word}")]
    public async Task<IActionResult> Get(string word)
    {
        var url = $"{BaseUrl}/{word}";
        return await _httpService.Get(url);
    }
}