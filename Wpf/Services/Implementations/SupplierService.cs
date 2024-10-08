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
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public SupplierService(HttpClient httpClient, ApiSettings apiSettings)
    {
        _httpClient = httpClient;
        _baseUrl = apiSettings.BaseUrl;
    }

    public async Task<Supplier> GetAsync(int id)
    {
        var requestUrl = API.Suppliers.Get(_baseUrl, id);

        var response = await _httpClient.GetAsync(requestUrl);

        if (response.IsSuccessStatusCode)
        {
            var jsonString = await response.Content.ReadAsStringAsync();
            var supplier = JsonConvert.DeserializeObject<Supplier>(jsonString);
            return supplier!;
        }

        var responseString = response.Content.ReadAsStringAsync().Result;
        throw new Exception($"Ошибка получения поставщика с id {id}");
    }

    public async Task<Supplier[]> GetAllAsync()
    {
        var requestUrl = API.Suppliers.GetAll(_baseUrl);

        var response = await _httpClient.GetAsync(requestUrl);

        if (response.IsSuccessStatusCode)
        {
            var jsonString = await response.Content.ReadAsStringAsync();
            var suppliers = JsonConvert.DeserializeObject<Supplier[]>(jsonString);
            return suppliers!;
        }

        var responseString = await response.Content.ReadAsStringAsync();
        throw new Exception("Ошибка получения списка поставщиков");
    }

    public async Task<Supplier> AddSupplierAsync(string name)
    {
        var requestUrl = API.Suppliers.AddSupplier(_baseUrl);

        var jsonString = JsonConvert.SerializeObject(name);
        var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(requestUrl, content);

        if (response.IsSuccessStatusCode)
        {
            var jsonSupplier = await response.Content.ReadAsStringAsync();
            var supplier = JsonConvert.DeserializeObject<Supplier>(jsonSupplier);
            return supplier!;
        }

        var responseString = response.Content.ReadAsStringAsync().Result;
        throw new Exception("Ошибка при добавлении поставщика");
    }

    public async Task UpdateSupplierAsync(Supplier supplier)
    {
        var requestUrl = API.Suppliers.UpdateSupplier(_baseUrl, supplier.Id);

        var jsonString = JsonConvert.SerializeObject(supplier.Name);
        var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync(requestUrl, content);

        if (response.IsSuccessStatusCode is false)
        {
            var responseString = response.Content.ReadAsStringAsync().Result;
            throw new Exception("Ошибка при изменении поставщика");
        }
    }

    public async Task DeleteSupplier(int id)
    {
        var requestUrl = API.Suppliers.DeleteSupplier(_baseUrl, id);

        var response = await _httpClient.DeleteAsync(requestUrl);

        if (response.IsSuccessStatusCode is false)
        {
            var responseString = response.Content.ReadAsStringAsync().Result;
            throw new Exception("Ошибка при удалении поставщика");
        }
    }
}
