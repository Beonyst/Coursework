#nullable enable
using API.Data.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SupplierController : Controller
{
    private readonly ISupplierService _supplierService;

    public SupplierController(ISupplierService supplierService)
    {
        _supplierService = supplierService;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var suppliers = _supplierService.GetAll();

        return Json(suppliers);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> Get([Required] int id)
    {
        var supplier = await _supplierService.GetAsync(id);

        return Ok(supplier);
    }

    [HttpPut]
    [Route("{id}")]
    public IActionResult UpdateSupplier([Required] int id, [FromBody]string name)
    {
        _supplierService.UpdateSupplier(id, name);

        return NoContent();
    }

    [HttpPost]
    public IActionResult AddSupplier([FromBody]string name)
    {
        var supplier = _supplierService.AddSupplier(name);

        return Json(supplier);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete([Required] int id)
    {
        _supplierService.DeleteSupplier(id);

        return NoContent();
    }
}
