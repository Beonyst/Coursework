#nullable enable
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using Wpf.Models;
using Wpf.Models.ApiRequestModels;
using Wpf.Models.Settings;
using Wpf.Services.Interfaces;

namespace Wpf.Services.Implementations;

public class MedicineService : IMedicineService
{
    private readonly HttpClient _httpClient;  // HTTP клиент для запросов
    private readonly string _baseUrl;  // Базовый URL для API

    // Конструктор для инициализации HttpClient и базового URL
    public MedicineService(HttpClient httpClient, ApiSettings apiSettings)
    {
        _httpClient = httpClient;
        _baseUrl = apiSettings.BaseUrl;
    }

    // Получение одного лекарства по ID
    public async Task<Medicine> GetAsync(int id)
    {
        var requestUrl = API.Medicines.Get(_baseUrl, id);  // Формируем URL для запроса

        var response = await _httpClient.GetAsync(requestUrl);  // Отправляем GET-запрос

        if (response.IsSuccessStatusCode)
        {
            var jsonString = await response.Content.ReadAsStringAsync();  // Читаем ответ
            var medicine = JsonConvert.DeserializeObject<Medicine>(jsonString);  // Десериализуем JSON в объект
            return medicine!;  // Возвращаем объект лекарства
        }

        var responseString = response.Content.ReadAsStringAsync().Result;  // Получаем строку ошибки
        throw new Exception($"Ошибка получения товара с id {id}");  // Выбрасываем исключение, если запрос не успешен
    }

    // Получение всех лекарств
    public async Task<Medicine[]> GetAllAsync()
    {
        var requestUrl = API.Medicines.GetAll(_baseUrl);  // Формируем URL для запроса

        var response = await _httpClient.GetAsync(requestUrl);  // Отправляем GET-запрос

        if (response.IsSuccessStatusCode)
        {
            var jsonString = await response.Content.ReadAsStringAsync();  // Читаем ответ
            var medicines = JsonConvert.DeserializeObject<Medicine[]>(jsonString);  // Десериализуем JSON в массив лекарств
            return medicines!;  // Возвращаем массив лекарств
        }

        var responseString = await response.Content.ReadAsStringAsync();  // Получаем строку ошибки
        throw new Exception("Ошибка получения списка товаров");  // Выбрасываем исключение, если запрос не успешен
    }

    // Добавление нового лекарства
    public async Task<Medicine> AddMedicineAsync(MedicineParameters parameters)
    {
        var requestUrl = API.Medicines.AddMedicine(_baseUrl);  // Формируем URL для запроса

        var jsonString = JsonConvert.SerializeObject(parameters);  // Сериализуем параметры лекарства в JSON
        var content = new StringContent(jsonString, Encoding.UTF8, "application/json");  // Создаем контент запроса
        var response = await _httpClient.PostAsync(requestUrl, content);  // Отправляем POST-запрос

        if (response.IsSuccessStatusCode)
        {
            var jsonMedicine = await response.Content.ReadAsStringAsync();  // Читаем ответ
            var addedMedicine = JsonConvert.DeserializeObject<Medicine>(jsonMedicine);  // Десериализуем JSON в объект
            return addedMedicine!;  // Возвращаем добавленное лекарство
        }

        var responseString = response.Content.ReadAsStringAsync().Result;  // Получаем строку ошибки
        throw new Exception("Ошибка при добавлении товара");  // Выбрасываем исключение, если запрос не успешен
    }

    // Обновление данных лекарства
    public async Task UpdateMedicineAsync(int id, MedicineParameters parameters)
    {
        var requestUrl = API.Medicines.UpdateMedicine(_baseUrl, id);  // Формируем URL для запроса

        var jsonString = JsonConvert.SerializeObject(parameters);  // Сериализуем параметры лекарства в JSON
        var content = new StringContent(jsonString, Encoding.UTF8, "application/json");  // Создаем контент запроса
        var response = await _httpClient.PutAsync(requestUrl, content);  // Отправляем PUT-запрос

        if (response.IsSuccessStatusCode is false)
        {
            var responseString = response.Content.ReadAsStringAsync().Result;  // Получаем строку ошибки
            throw new Exception("Ошибка при изменении товара");  // Выбрасываем исключение, если запрос не успешен
        }
    }

    // Удаление лекарства по ID
    public async Task DeleteMedicine(int id)
    {
        var requestUrl = API.Medicines.DeleteMedicine(_baseUrl, id);  // Формируем URL для запроса

        var response = await _httpClient.DeleteAsync(requestUrl);  // Отправляем DELETE-запрос

        if (response.IsSuccessStatusCode is false)
        {
            var responseString = response.Content.ReadAsStringAsync().Result;  // Получаем строку ошибки
            throw new Exception("Ошибка при удалении товара");  // Выбрасываем исключение, если запрос не успешен
        }
    }
}
