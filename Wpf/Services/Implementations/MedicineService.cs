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
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public MedicineService(HttpClient httpClient, ApiSettings apiSettings)
    {
        _httpClient = httpClient;
        _baseUrl = apiSettings.BaseUrl;
    }

    public async Task<Medicine> GetAsync(int id)
    {
        var requestUrl = API.Medicines.Get(_baseUrl, id);

        var response = await _httpClient.GetAsync(requestUrl);

        if (response.IsSuccessStatusCode)
        {
            var jsonString = await response.Content.ReadAsStringAsync();
            var medicine = JsonConvert.DeserializeObject<Medicine>(jsonString);
            return medicine!;
        }

        var responseString = response.Content.ReadAsStringAsync().Result;
        throw new Exception($"Ошибка получения товара с id {id}");
    }

    public async Task<Medicine[]> GetAllAsync()
    {
        var requestUrl = API.Medicines.GetAll(_baseUrl);

        var response = await _httpClient.GetAsync(requestUrl);

        if (response.IsSuccessStatusCode)
        {
            var jsonString = await response.Content.ReadAsStringAsync();
            var medicines = JsonConvert.DeserializeObject<Medicine[]>(jsonString);
            return medicines!;
        }

        var responseString = await response.Content.ReadAsStringAsync();
        throw new Exception("Ошибка получения списка товаров"); 
    }

    public async Task<Medicine> AddMedicineAsync(MedicineParameters parameters)
    {
        var requestUrl = API.Medicines.AddMedicine(_baseUrl);

        var jsonString = JsonConvert.SerializeObject(parameters);
        var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(requestUrl, content);

        if (response.IsSuccessStatusCode)
        {
            var jsonMedicine = await response.Content.ReadAsStringAsync();
            var addedMedicine = JsonConvert.DeserializeObject<Medicine>(jsonMedicine);
            return addedMedicine!;
        }

        var responseString = response.Content.ReadAsStringAsync().Result;
        throw new Exception("Ошибка при добавлении товара");
    }

    public async Task UpdateMedicineAsync(int id, MedicineParameters parameters)
    {
        var requestUrl = API.Medicines.UpdateMedicine(_baseUrl, id);

        var jsonString = JsonConvert.SerializeObject(parameters);
        var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync(requestUrl, content);

        if (response.IsSuccessStatusCode is false)
        {
            var responseString = response.Content.ReadAsStringAsync().Result;
            throw new Exception("Ошибка при изменении товара");
        }
    }

    public async Task DeleteMedicine(int id)
    {
        var requestUrl = API.Medicines.DeleteMedicine(_baseUrl, id);

        var response = await _httpClient.DeleteAsync(requestUrl);

        if (response.IsSuccessStatusCode is false)
        {
            var responseString = response.Content.ReadAsStringAsync().Result;
            throw new Exception("Ошибка при удалении товара");
        }
    }
}
