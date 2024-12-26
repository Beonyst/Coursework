// Этот файл содержит реализацию фабрики для создания экземпляров HttpClient, которые могут использоваться для взаимодействия с API.

#nullable enable
using System.Net.Http;

namespace Wpf.Infrastructure;

// Класс, реализующий интерфейс IApiHttpClientFactory.
// Этот класс отвечает за создание экземпляров HttpClient, включая конфигурацию клиента.
public class ApiHttpClientFactory : IApiHttpClientFactory
{
    // Метод для создания HttpClient без авторизации, с установленным временем ожидания в 5 минут
    public HttpClient GetUnauthorizedClient()
    {
        var httpClient = new HttpClient();

        // Устанавливаем таймаут на 5 минут для запросов
        httpClient.Timeout = TimeSpan.FromMinutes(5);

        return httpClient;
    }
}
