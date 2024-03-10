using System.Net.Http;
using LousBot.Service.Interfaces;

namespace LousBot.Service;

public class HttpService : IHttpService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public HttpService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public HttpClient ReturnHttpClient()
    {
        return _httpClientFactory.CreateClient();
    }
}