using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace PirnBackend.Services;

public class HttpService: IHttpService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger _logger;
    public HttpService(ILogger logger)
    {
        _httpClient = new HttpClient();
        _logger = logger;
    }
    
    public async Task<IActionResult> Get(string url, Dictionary<string, string>? headers = null)
    {
        return await ExecuteRequest(url, HttpMethod.Get, headers);
    }

    public async Task<IActionResult> Post(string url, Dictionary<string, string>? headers = null, object? payload = null)
    {
        return await ExecuteRequest(url, HttpMethod.Post, headers, payload);
    }

    private async Task<IActionResult> ExecuteRequest(
        string url, 
        HttpMethod httpMethod, 
        Dictionary<string, string>? headers, 
        object? payload = null) 
    {
        var request = new HttpRequestMessage(httpMethod, url);

        if (headers != null)
        {
            foreach (var header in headers)
            {
                request.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
        }

        request.Headers.TryAddWithoutValidation("Accept", "application/json");
        
        if (payload != null)
        {
            var payloadContent = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            request.Content = payloadContent;
        }
        
        var response = await _httpClient.SendAsync(request);

        return await ConvertToIActionResult(response);
    }
    
    private static async Task<ObjectResult> ConvertToIActionResult(HttpResponseMessage response)
    {
        // Copy the content if needed
        var content = await response.Content.ReadAsStringAsync();
        
        // Use the response status code to determine IActionResult

        switch (response)
        {
            case { StatusCode: HttpStatusCode.OK }:
            {
                var contentObject = JsonSerializer.Deserialize<object>(content);
                return new OkObjectResult(contentObject);
            }
            case { StatusCode: HttpStatusCode.BadRequest }:
                return new BadRequestObjectResult(content);
            case { StatusCode: HttpStatusCode.NotFound }:
                return new NotFoundObjectResult(content);
            default:
                return new ObjectResult(content) { StatusCode = (int)response.StatusCode };
        }
    }
}