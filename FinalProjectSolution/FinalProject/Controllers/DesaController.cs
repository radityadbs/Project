using Microsoft.AspNetCore.Mvc;
using FinalProject.Services;
using FinalProject.Dtos;

namespace FinalProject.Controllers;

[ApiController]
[Route("api/desa")]
public class DesaController : ControllerBase
{
    private readonly DesaService _desaService;

    public DesaController(DesaService desaService)
    {
        _desaService = desaService;
    }

    // [HttpGet]
    // public async Task<IActionResult> Get()
    // {
    //     var desa = await _desaService.LoadAllDesaAsync();
    //     return Ok(desa);
    // }

    [HttpGet]
    public async Task<IActionResult> Search([FromQuery] string q)
    {
        if (string.IsNullOrWhiteSpace(q) || q.Length < 3)
            return Ok(Array.Empty<DesaDto>()); // ⛔ STOP DI SINI

        var result = await _desaService.SearchAsync(q);
        return Ok(result);
    }


}
