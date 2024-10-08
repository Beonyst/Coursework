#nullable enable

namespace Wpf.Services.Implementations;

public static class API
{
    public static class Suppliers
    {
        public static string Get(string baseUrl, int id) => $"{baseUrl}/api/supplier/{id}";

        public static string GetAll(string baseUrl) => $"{baseUrl}/api/supplier";

        public static string UpdateSupplier(string baseUrl, int id) => $"{baseUrl}/api/supplier/{id}";

        public static string AddSupplier(string baseUrl) => $"{baseUrl}/api/supplier";

        public static string DeleteSupplier(string baseUrl, int id) => $"{baseUrl}/api/supplier/{id}";
    }

    public static class Medicines
    {
        public static string Get(string baseUrl, int id) => $"{baseUrl}/api/medicine/{id}";

        public static string GetAll(string baseUrl) => $"{baseUrl}/api/medicine";

        public static string UpdateMedicine(string baseUrl, int id) => $"{baseUrl}/api/medicine/{id}";

        public static string AddMedicine(string baseUrl) => $"{baseUrl}/api/medicine";

        public static string DeleteMedicine(string baseUrl, int id) => $"{baseUrl}/api/medicine/{id}";
    }
}
