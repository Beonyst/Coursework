// Интерфейс IApiHttpClientFactory предоставляет метод для получения экземпляра HttpClient, который не авторизован и может быть использован для запросов,
// требующих анонимного доступа (например, для запросов, не требующих авторизации).

#nullable enable
using System.Net.Http;

namespace Wpf.Infrastructure;

// Интерфейс для фабрики, которая создает экземпляры HttpClient.
public interface IApiHttpClientFactory
{
    // Метод для получения HttpClient, который не имеет авторизации.
    HttpClient GetUnauthorizedClient();
}
