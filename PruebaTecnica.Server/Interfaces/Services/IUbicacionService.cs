using PruebaTecnica.Server.Models;

namespace PruebaTecnica.Server.Interfaces.Services
{
    public interface IUbicacionService
    {
        Task<IEnumerable<Departamento>> GetDepartamentosAsync();
        Task<IEnumerable<Provincia>> GetProvinciasByDepartamentoAsync(int idDepartamento);
        Task<IEnumerable<Distrito>> GetDistritosByProvinciaAsync(int idProvincia);
    }
}
