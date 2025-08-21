using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Wpf.Services;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using Xunit;
using Wpf.Models.ApiRequestModels;
using Wpf.Models.Settings;
using Wpf.Services.Implementations;

namespace Coursework.Tests.Services
{
    public class MedicineServiceWhiteBoxTests
    {
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly HttpClient _httpClient;
        private readonly MedicineService _medicineService;

        public MedicineServiceWhiteBoxTests()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object);

            // Создаем mock для ApiSettings или используем правильный конструктор
            var apiSettings = new ApiSettings { BaseUrl = "https://localhost:7000" };
            _medicineService = new MedicineService(_httpClient, apiSettings);
        }

        //[Fact]
        //public async Task UpdateMedicineAsync_ShouldSendCorrectPutRequest()
        //{
        //    // Arrange
        //    var id = 1;
        //    var parameters = new MedicineParameters
        //    {
        //        Name = "Аспирин",
        //        Price = 150.50, // double вместо decimal
        //        // Уберите свойства которые не существуют в вашем классе
        //        Description = "Обезболивающее"
        //        // Manufacturer и Quantity удалены, если их нет
        //    };

        //    var expectedUrl = $"https://localhost:7000/api/Medicine/{id}";

        //    var response = new HttpResponseMessage(HttpStatusCode.OK);

        //    _httpMessageHandlerMock.Protected()
        //        .Setup<Task<HttpResponseMessage>>(
        //            "SendAsync",
        //            ItExpr.Is<HttpRequestMessage>(req =>
        //                req.Method == HttpMethod.Put &&
        //                req.RequestUri.ToString() == expectedUrl),
        //            ItExpr.IsAny<CancellationToken>())
        //        .ReturnsAsync(response);

        //    // Act
        //    await _medicineService.UpdateMedicineAsync(id, parameters);

        //    // Assert
        //    _httpMessageHandlerMock.Protected().Verify(
        //        "SendAsync",
        //        Times.Once(),
        //        ItExpr.Is<HttpRequestMessage>(req =>
        //            req.Method == HttpMethod.Put &&
        //            req.RequestUri.ToString() == expectedUrl),
        //        ItExpr.IsAny<CancellationToken>());
        //}

        [Fact]
        public async Task UpdateMedicineAsync_ShouldThrowException_WhenResponseIsNotSuccessful()
        {
            // Arrange
            var id = 1;
            var parameters = new MedicineParameters();
            var errorMessage = "Medicine not found";

            var response = new HttpResponseMessage(HttpStatusCode.NotFound)
            {
                Content = new StringContent(errorMessage, Encoding.UTF8, "application/json")
            };

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() =>
                _medicineService.UpdateMedicineAsync(id, parameters));

            Assert.Equal("Ошибка при изменении товара", exception.Message);
        }

     
    }
}