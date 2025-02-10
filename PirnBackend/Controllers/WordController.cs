using System.Net;
using Microsoft.AspNetCore.Mvc;
using PirnBackend.Services;

namespace PirnBackend.Controllers;

[ApiController]
[Route("[controller]")]
public class WordController(ILogger<WordController> logger) : ControllerBase
{
    private readonly ILogger<WordController> _logger = logger;
    private const string BaseUrl = "https://api.sonapi.ee/v2/";
    private readonly HttpService _httpService = new(logger);
    private readonly SonapiService _sonapiService = new();

    [HttpGet(template: "{word}")]
    public async Task<IActionResult> Get(string word)
    {
        try
        {
            var url = $"{BaseUrl}/{word}";
            var result = await _httpService.Get(url);
            var wordObject = _sonapiService.GetWord(result, word);
            return Ok(wordObject);
        }
        catch (HttpServiceFailureException e)
        {
            return e.StatusCode switch
            {
                HttpStatusCode.NotFound => new NotFoundResult(),
                _ => new StatusCodeResult((int)e.StatusCode)
            };
        }
    }
}