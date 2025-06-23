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
            if (string.IsNullOrWhiteSpace(trabajador.TipoDocumento))
            {
                throw new ArgumentException("El tipo de documento no puede ser nulo o vacío.", nameof(trabajador.TipoDocumento));
            }

            if (string.IsNullOrWhiteSpace(trabajador.NumeroDocumento))
            {
                throw new ArgumentException("El número de documento no puede ser nulo o vacío.", nameof(trabajador.NumeroDocumento));
            }

            var existente = await _repository.FindByDocumentoAsync(trabajador.TipoDocumento, trabajador.NumeroDocumento);

            if (existente != null)
            {
                throw new ArgumentException($"Ya existe un trabajador registrado con el documento {trabajador.TipoDocumento} {trabajador.NumeroDocumento}.");
            }

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

            if (string.IsNullOrWhiteSpace(trabajador.TipoDocumento))
            {
                throw new ArgumentException("El tipo de documento no puede ser nulo o vacío.", nameof(trabajador.TipoDocumento));
            }

            if (string.IsNullOrWhiteSpace(trabajador.NumeroDocumento))
            {
                throw new ArgumentException("El número de documento no puede ser nulo o vacío.", nameof(trabajador.NumeroDocumento));
            }

            var existente = await _repository.FindByDocumentoAsync(trabajador.TipoDocumento, trabajador.NumeroDocumento);
            if (existente != null && existente.Id != id)
            {
                throw new ArgumentException($"El documento {trabajador.TipoDocumento} {trabajador.NumeroDocumento} ya está en uso por otro trabajador.");
            }

            await _repository.UpdateAsync(trabajador);
        }

        public async Task DeleteTrabajadorAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
