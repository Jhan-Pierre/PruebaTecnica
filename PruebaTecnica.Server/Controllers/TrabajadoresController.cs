using Microsoft.AspNetCore.Mvc;
using PruebaTecnica.Server.Interfaces.Services;
using PruebaTecnica.Server.Models;

namespace PruebaTecnica.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrabajadoresController : ControllerBase
    {
        private readonly ITrabajadorService _service;

        public TrabajadoresController(ITrabajadorService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetTrabajadores([FromQuery] string? sexo = null)
        {
            var trabajadores = await _service.GetTrabajadoresAsync(sexo);
            return Ok(trabajadores);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTrabajador(int id)
        {
            var trabajador = await _service.GetTrabajadorByIdAsync(id);
            if (trabajador == null)
            {
                return NotFound();
            }
            return Ok(trabajador);
        }

        [HttpPost]
        public async Task<IActionResult> PostTrabajador([FromBody] Trabajador trabajador)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var nuevoTrabajador = await _service.CreateTrabajadorAsync(trabajador);
            return CreatedAtAction(nameof(GetTrabajador), new { id = nuevoTrabajador.Id }, nuevoTrabajador);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTrabajador(int id, [FromBody] Trabajador trabajador)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _service.UpdateTrabajadorAsync(id, trabajador);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrabajador(int id)
        {
            await _service.DeleteTrabajadorAsync(id);
            return NoContent();
        }
    }
}
