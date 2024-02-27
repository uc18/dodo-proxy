using System.Net.Http;
using LousBot.Service.Interfaces;

namespace LousBot.Service;

public class HttpService : IHttpService
{
    private readonly HttpClient _httpClient = new();

    public HttpClient ReturnHttpClient()
    {
        return _httpClient;
    }
}