#nullable enable
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using Wpf.Models;
using Wpf.Models.Settings;
using Wpf.Services.Interfaces;

namespace Wpf.Services.Implementations;

public class SupplierService : ISupplierService
{
    private readonly HttpClient _httpClient;  // HTTP клиент для запросов
    private readonly string _baseUrl;  // Базовый URL для API

    // Конструктор для инициализации HttpClient и базового URL
    public SupplierService(HttpClient httpClient, ApiSettings apiSettings)
    {
        _httpClient = httpClient;
        _baseUrl = apiSettings.BaseUrl;
    }

    // Получение одного поставщика по ID
    public async Task<Supplier> GetAsync(int id)
    {
        var requestUrl = API.Suppliers.Get(_baseUrl, id);  // Формируем URL для запроса

        var response = await _httpClient.GetAsync(requestUrl);  // Отправляем GET-запрос

        if (response.IsSuccessStatusCode)
        {
            var jsonString = await response.Content.ReadAsStringAsync();  // Читаем ответ
            var supplier = JsonConvert.DeserializeObject<Supplier>(jsonString);  // Десериализуем JSON в объект
            return supplier!;  // Возвращаем объект поставщика
        }

        var responseString = response.Content.ReadAsStringAsync().Result;  // Получаем строку ошибки
        throw new Exception($"Ошибка получения поставщика с id {id}");  // Выбрасываем исключение, если запрос не успешен
    }

    // Получение всех поставщиков
    public async Task<Supplier[]> GetAllAsync()
    {
        var requestUrl = API.Suppliers.GetAll(_baseUrl);  // Формируем URL для запроса

        var response = await _httpClient.GetAsync(requestUrl);  // Отправляем GET-запрос

        if (response.IsSuccessStatusCode)
        {
            var jsonString = await response.Content.ReadAsStringAsync();  // Читаем ответ
            var suppliers = JsonConvert.DeserializeObject<Supplier[]>(jsonString);  // Десериализуем JSON в массив поставщиков
            return suppliers!;  // Возвращаем массив поставщиков
        }

        var responseString = await response.Content.ReadAsStringAsync();  // Получаем строку ошибки
        throw new Exception("Ошибка получения списка поставщиков");  // Выбрасываем исключение, если запрос не успешен
    }

    // Добавление нового поставщика
    public async Task<Supplier> AddSupplierAsync(string name)
    {
        var requestUrl = API.Suppliers.AddSupplier(_baseUrl);  // Формируем URL для запроса

        var jsonString = JsonConvert.SerializeObject(name);  // Сериализуем имя поставщика в JSON
        var content = new StringContent(jsonString, Encoding.UTF8, "application/json");  // Создаем контент запроса
        var response = await _httpClient.PostAsync(requestUrl, content);  // Отправляем POST-запрос

        if (response.IsSuccessStatusCode)
        {
            var jsonSupplier = await response.Content.ReadAsStringAsync();  // Читаем ответ
            var supplier = JsonConvert.DeserializeObject<Supplier>(jsonSupplier);  // Десериализуем JSON в объект
            return supplier!;  // Возвращаем объект поставщика
        }

        var responseString = response.Content.ReadAsStringAsync().Result;  // Получаем строку ошибки
        throw new Exception("Ошибка при добавлении поставщика");  // Выбрасываем исключение, если запрос не успешен
    }

    // Обновление данных поставщика
    public async Task UpdateSupplierAsync(Supplier supplier)
    {
        var requestUrl = API.Suppliers.UpdateSupplier(_baseUrl, supplier.Id);  // Формируем URL для запроса

        var jsonString = JsonConvert.SerializeObject(supplier.Name);  // Сериализуем имя поставщика в JSON
        var content = new StringContent(jsonString, Encoding.UTF8, "application/json");  // Создаем контент запроса
        var response = await _httpClient.PutAsync(requestUrl, content);  // Отправляем PUT-запрос

        if (response.IsSuccessStatusCode is false)
        {
            var responseString = response.Content.ReadAsStringAsync().Result;  // Получаем строку ошибки
            throw new Exception("Ошибка при изменении поставщика");  // Выбрасываем исключение, если запрос не успешен
        }
    }

    // Удаление поставщика по ID
    public async Task DeleteSupplier(int id)
    {
        var requestUrl = API.Suppliers.DeleteSupplier(_baseUrl, id);  // Формируем URL для запроса

        var response = await _httpClient.DeleteAsync(requestUrl);  // Отправляем DELETE-запрос

        if (response.IsSuccessStatusCode is false)
        {
            var responseString = response.Content.ReadAsStringAsync().Result;  // Получаем строку ошибки
            throw new Exception("Ошибка при удалении поставщика");  // Выбрасываем исключение, если запрос не успешен
        }
    }
}
