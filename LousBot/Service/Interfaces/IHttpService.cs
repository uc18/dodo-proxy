using System.Net.Http;

namespace LousBot.Service.Interfaces;

public interface IHttpService
{
    HttpClient ReturnHttpClient();
}