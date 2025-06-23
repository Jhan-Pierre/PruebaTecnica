using PruebaTecnica.Server.DTOs;
using PruebaTecnica.Server.Models;

namespace PruebaTecnica.Server.Interfaces.Services
{
    public interface ITrabajadorService
    {
        Task<IEnumerable<TrabajadorDto>> GetTrabajadoresAsync(string? sexo);
        Task<Trabajador?> GetTrabajadorByIdAsync(int id);
        Task<Trabajador> CreateTrabajadorAsync(Trabajador trabajador);
        Task UpdateTrabajadorAsync(int id, Trabajador trabajador);
        Task DeleteTrabajadorAsync(int id);
    }
}
