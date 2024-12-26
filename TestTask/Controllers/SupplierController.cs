#nullable enable
using API.Data.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace API.Controllers;

// Контроллер для управления данными о поставщиках, использует сервис для обработки логики.
[ApiController]
[Route("api/[controller]")]
public class SupplierController : Controller
{
    private readonly ISupplierService _supplierService;

    // Конструктор контроллера, принимает сервис для работы с поставщиками.
    public SupplierController(ISupplierService supplierService)
    {
        _supplierService = supplierService;
    }

    // Метод для получения всех поставщиков.
    [HttpGet]
    public IActionResult GetAll()
    {
        var suppliers = _supplierService.GetAll();  // Получение всех поставщиков через сервис.

        return Json(suppliers);  // Возвращение данных в формате JSON.
    }

    // Метод для получения информации о поставщике по ID.
    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> Get([Required] int id)
    {
        var supplier = await _supplierService.GetAsync(id);  // Асинхронное получение поставщика по ID.

        return Ok(supplier);  // Возвращение поставщика с кодом 200 (OK).
    }

    // Метод для обновления данных о поставщике.
    [HttpPut]
    [Route("{id}")]
    public IActionResult UpdateSupplier([Required] int id, [FromBody]string name)
    {
        _supplierService.UpdateSupplier(id, name);  // Обновление данных поставщика через сервис.

        return NoContent();  // Возвращение ответа без содержимого (код 204).
    }

    // Метод для добавления нового поставщика.
    [HttpPost]
    public IActionResult AddSupplier([FromBody]string name)
    {
        var supplier = _supplierService.AddSupplier(name);  // Добавление нового поставщика через сервис.

        return Json(supplier);  // Возвращение добавленного поставщика в формате JSON.
    }

    // Метод для удаления поставщика по ID.
    [HttpDelete("{id}")]
    public IActionResult Delete([Required] int id)
    {
        _supplierService.DeleteSupplier(id);  // Удаление поставщика через сервис.

        return NoContent();  // Возвращение ответа без содержимого (код 204).
    }
}
