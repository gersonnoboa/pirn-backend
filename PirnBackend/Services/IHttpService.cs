using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using PirnBackend.Models;

namespace PirnBackend.Services;

public interface IHttpService
{
    Task<JsonDocument> Get(string url, Dictionary<string, string>? headers = null);
    Task<JsonDocument> Post(string url, Dictionary<string, string>? headers = null, object? payload = null);
}