#nullable enable

namespace Wpf.Services.Implementations;

// Класс API содержит статические методы для работы с API поставщиков и медикаментов.
public static class API
{
    // Вложенный класс Suppliers предоставляет методы для работы с поставщиками через API.
    public static class Suppliers
    {
        // Формирует URL для получения информации о поставщике по ID
        public static string Get(string baseUrl, int id) => $"{baseUrl}/api/supplier/{id}";

        // Формирует URL для получения списка всех поставщиков
        public static string GetAll(string baseUrl) => $"{baseUrl}/api/supplier";

        // Формирует URL для обновления информации о поставщике по ID
        public static string UpdateSupplier(string baseUrl, int id) => $"{baseUrl}/api/supplier/{id}";

        // Формирует URL для добавления нового поставщика
        public static string AddSupplier(string baseUrl) => $"{baseUrl}/api/supplier";

        // Формирует URL для удаления поставщика по ID
        public static string DeleteSupplier(string baseUrl, int id) => $"{baseUrl}/api/supplier/{id}";
    }

    // Вложенный класс Medicines предоставляет методы для работы с медикаментами через API.
    public static class Medicines
    {
        // Формирует URL для получения информации о медикаменте по ID
        public static string Get(string baseUrl, int id) => $"{baseUrl}/api/medicine/{id}";

        // Формирует URL для получения списка всех медикаментов
        public static string GetAll(string baseUrl) => $"{baseUrl}/api/medicine";

        // Формирует URL для обновления информации о медикаменте по ID
        public static string UpdateMedicine(string baseUrl, int id) => $"{baseUrl}/api/medicine/{id}";

        // Формирует URL для добавления нового медикамента
        public static string AddMedicine(string baseUrl) => $"{baseUrl}/api/medicine";

        // Формирует URL для удаления медикамента по ID
        public static string DeleteMedicine(string baseUrl, int id) => $"{baseUrl}/api/medicine/{id}";
    }
}
