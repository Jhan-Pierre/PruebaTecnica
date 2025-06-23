using PruebaTecnica.Server.Interfaces.Repositories;
using PruebaTecnica.Server.Interfaces.Services;
using PruebaTecnica.Server.Models;

namespace PruebaTecnica.Server.Services
{
    public class UbicacionService : IUbicacionService
    {
        private readonly IUbicacionRepository _repository;

        public UbicacionService(IUbicacionRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Departamento>> GetDepartamentosAsync()
        {
            return await _repository.GetDepartamentosAsync();
        }

        public async Task<IEnumerable<Provincia>> GetProvinciasByDepartamentoAsync(int idDepartamento)
        {
            return await _repository.GetProvinciasByDepartamentoAsync(idDepartamento);
        }

        public async Task<IEnumerable<Distrito>> GetDistritosByProvinciaAsync(int idProvincia)
        {
            return await _repository.GetDistritosByProvinciaAsync(idProvincia);
        }
    }
}
