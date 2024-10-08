#nullable enable
using API.Data.Models.ApiRequestModels;
using API.Data.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MedicineController : Controller
{
    private readonly IMedicineService _medicineService;

    public MedicineController(IMedicineService medicineService)
    {
        _medicineService = medicineService;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var medicines = _medicineService.GetAll();

        return Json(medicines);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> Get([Required] int id)
    {
        var medicine = await _medicineService.GetAsync(id);

        return Ok(medicine);
    }

    [HttpPut]
    [Route("{id}")]
    public IActionResult UpdateMedicine([Required]int id, [FromBody] MedicineParameters parameters)
    {
        _medicineService.UpdateMedicine(id, parameters.Name, parameters.Description, parameters.Price, parameters.SupplierId);

        return NoContent();
    }

    [HttpPost]
    public IActionResult AddMedicine([FromBody]MedicineParameters parameters)
    {
        var medicine = _medicineService.AddMedicine(parameters.Name, parameters.Description, parameters.Price, parameters.SupplierId);

        return Json(medicine);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete([Required] int id)
    {
        _medicineService.DeleteMedicine(id);

        return NoContent();
    }
}
