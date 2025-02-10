using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PirnBackend.Models;

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
    
    public async Task<JsonDocument> Get(string url, Dictionary<string, string>? headers = null)
    {
        return await ExecuteRequest(url, HttpMethod.Get, headers);
    }

    public async Task<JsonDocument> Post(string url, Dictionary<string, string>? headers = null, object? payload = null)
    {
        return await ExecuteRequest(url, HttpMethod.Post, headers, payload);
    }

    private async Task<JsonDocument> ExecuteRequest(
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

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpServiceFailureException(response.StatusCode);
        }
        
        var content = await response.Content.ReadAsStringAsync();
        var json = JsonDocument.Parse(content);
        
        return json;
    }
}

public class HttpServiceFailureException(HttpStatusCode statusCode): Exception
{
    public HttpStatusCode StatusCode { get; } = statusCode;
}