#nullable enable
using API.Data.Models.ApiRequestModels;
using API.Data.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace API.Controllers;

// Контроллер для управления данными о лекарствах, использует сервис для обработки логики.
[ApiController]
[Route("api/[controller]")]
public class MedicineController : Controller
{
    private readonly IMedicineService _medicineService;

    // Конструктор контроллера, принимает сервис для работы с лекарствами.
    public MedicineController(IMedicineService medicineService)
    {
        _medicineService = medicineService;
    }

    // Метод для получения всех лекарств.
    [HttpGet]
    public IActionResult GetAll()
    {
        var medicines = _medicineService.GetAll();  // Получение всех лекарств через сервис.

        return Json(medicines);  // Возвращение данных в формате JSON.
    }

    // Метод для получения информации о лекарстве по ID.
    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> Get([Required] int id)
    {
        var medicine = await _medicineService.GetAsync(id);  // Асинхронное получение лекарства по ID.

        return Ok(medicine);  // Возвращение лекарства с кодом 200 (OK).
    }

    // Метод для обновления информации о лекарстве.
    [HttpPut]
    [Route("{id}")]
    public IActionResult UpdateMedicine([Required]int id, [FromBody] MedicineParameters parameters)
    {
        _medicineService.UpdateMedicine(id, parameters.Name, parameters.Description, parameters.Price, parameters.SupplierId);  // Обновление данных о лекарствах через сервис.

        return NoContent();  // Возвращение ответа без содержимого (код 204).
    }

    // Метод для добавления нового лекарства.
    [HttpPost]
    public IActionResult AddMedicine([FromBody]MedicineParameters parameters)
    {
        var medicine = _medicineService.AddMedicine(parameters.Name, parameters.Description, parameters.Price, parameters.SupplierId);  // Добавление лекарства через сервис.

        return Json(medicine);  // Возвращение добавленного лекарства в формате JSON.
    }

    // Метод для удаления лекарства по ID.
    [HttpDelete("{id}")]
    public IActionResult Delete([Required] int id)
    {
        _medicineService.DeleteMedicine(id);  // Удаление лекарства через сервис.

        return NoContent();  // Возвращение ответа без содержимого (код 204).
    }
}
