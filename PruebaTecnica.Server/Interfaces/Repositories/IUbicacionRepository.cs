using PruebaTecnica.Server.Models;

namespace PruebaTecnica.Server.Interfaces.Repositories
{
    public interface IUbicacionRepository
    {
        Task<IEnumerable<Departamento>> GetDepartamentosAsync();
        Task<IEnumerable<Provincia>> GetProvinciasByDepartamentoAsync(int idDepartamento);
        Task<IEnumerable<Distrito>> GetDistritosByProvinciaAsync(int idProvincia);
    }
}
