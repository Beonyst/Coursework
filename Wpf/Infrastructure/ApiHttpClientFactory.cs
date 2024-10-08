#nullable enable
using System.Net.Http;

namespace Wpf.Infrastructure;

public class ApiHttpClientFactory : IApiHttpClientFactory
{
    public HttpClient GetUnauthorizedClient()
    {
        var httpClient = new HttpClient();

        httpClient.Timeout = TimeSpan.FromMinutes(5);

        return httpClient;
    }
}
