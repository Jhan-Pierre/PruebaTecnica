using Microsoft.AspNetCore.Mvc;
using PruebaTecnica.Server.Interfaces.Services;

namespace PruebaTecnica.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UbicacionController : ControllerBase // Cambiado a ControllerBase
    {
        private readonly IUbicacionService _ubicacionService;

        public UbicacionController(IUbicacionService ubicacionService)
        {
            _ubicacionService = ubicacionService;
        }

        [HttpGet("departamentos")]
        public async Task<IActionResult> GetDepartamentos()
        {
            var departamentos = await _ubicacionService.GetDepartamentosAsync();
            return Ok(departamentos);
        }

        [HttpGet("provincias/{idDepartamento}")]
        public async Task<IActionResult> GetProvincias(int idDepartamento)
        {
            var provincias = await _ubicacionService.GetProvinciasByDepartamentoAsync(idDepartamento);
            return Ok(provincias);
        }

        [HttpGet("distritos/{idProvincia}")]
        public async Task<IActionResult> GetDistritos(int idProvincia)
        {
            var distritos = await _ubicacionService.GetDistritosByProvinciaAsync(idProvincia);
            return Ok(distritos);
        }
    }
}
