using Xunit;
using Wpf.Services;
using Wpf.Models;
using System.Net.Http;
using Moq;
using Wpf.Models.Settings;
using Coursework.Tests.Services;
using Wpf.Services.Implementations;
using Wpf.Models.ApiRequestModels;

namespace Coursework.Tests.Services
{
    public class BasicStructureTest
    {
        [Fact]
        public void MedicineService_ShouldBeCreated()
        {
            // Arrange
            var httpClient = new HttpClient();
            var apiSettings = new ApiSettings { BaseUrl = "https://localhost:7000" };

            // Act
            var medicineService = new MedicineService(httpClient, apiSettings);

            // Assert
            Assert.NotNull(medicineService);
        }

        [Fact]
        public void MedicineParameters_ShouldHaveBasicProperties()
        {
            // Arrange & Act
            var parameters = new MedicineParameters
            {
                Name = "Test",
                Price = 100.50
            };

            // Assert
            Assert.Equal("Test", parameters.Name);
            Assert.Equal(100.50, parameters.Price);
        }
    }
}