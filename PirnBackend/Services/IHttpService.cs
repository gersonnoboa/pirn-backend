using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace PirnBackend.Services;

public interface IHttpService
{
    Task<IActionResult> Get(string url, Dictionary<string, string>? headers = null);
    Task<IActionResult> Post(string url, Dictionary<string, string>? headers = null, object? payload = null);
}