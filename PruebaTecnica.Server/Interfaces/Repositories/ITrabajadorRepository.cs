using PruebaTecnica.Server.DTOs;
using PruebaTecnica.Server.Models;

namespace PruebaTecnica.Server.Interfaces.Repositories
{
    public interface ITrabajadorRepository
    {
        Task<IEnumerable<TrabajadorDto>> GetAllAsync(string? sexo);
        Task<Trabajador?> GetByIdAsync(int id);
        Task<int> CreateAsync(Trabajador trabajador);
        Task UpdateAsync(Trabajador trabajador);
        Task DeleteAsync(int id);
        Task<Trabajador?> FindByDocumentoAsync(string tipoDocumento, string numeroDocumento);
    }
}
