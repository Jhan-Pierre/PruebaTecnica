using PruebaTecnica.Server.DTOs;
using PruebaTecnica.Server.Interfaces.Repositories;
using PruebaTecnica.Server.Interfaces.Services;
using PruebaTecnica.Server.Models;

namespace PruebaTecnica.Server.Services
{
    public class TrabajadorService : ITrabajadorService
    {
        private readonly ITrabajadorRepository _repository;

        public TrabajadorService(ITrabajadorRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<TrabajadorDto>> GetTrabajadoresAsync(string? sexo)
        {
            return await _repository.GetAllAsync(sexo);
        }

        public async Task<Trabajador?> GetTrabajadorByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Trabajador> CreateTrabajadorAsync(Trabajador trabajador)
        {
            // Aquí podrías añadir lógica de negocio, como verificar si el DNI ya existe
            // antes de llamar al repositorio.

            int nuevoId = await _repository.CreateAsync(trabajador);
            trabajador.Id = nuevoId;
            return trabajador;
        }

        public async Task UpdateTrabajadorAsync(int id, Trabajador trabajador)
        {
            if (id != trabajador.Id)
            {
                throw new ArgumentException("El ID del trabajador no coincide.");
            }
            await _repository.UpdateAsync(trabajador);
        }

        public async Task DeleteTrabajadorAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
