#nullable enable
using System.Net.Http;

namespace Wpf.Infrastructure;

public interface IApiHttpClientFactory
{
    HttpClient GetUnauthorizedClient();
}
