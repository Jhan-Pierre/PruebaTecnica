using Microsoft.EntityFrameworkCore;
using PruebaTecnica.Server.Data;
using PruebaTecnica.Server.Interfaces.Repositories;
using PruebaTecnica.Server.Models;

namespace PruebaTecnica.Server.Repositories
{
    public class UbicacionRepository : IUbicacionRepository
    {
        private readonly AppDbContext _context;

        public UbicacionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Departamento>> GetDepartamentosAsync()
        {
            return await _context.Departamentos
                .OrderBy(d => d.NombreDepartamento)
                .ToListAsync();
        }

        public async Task<IEnumerable<Provincia>> GetProvinciasByDepartamentoAsync(int idDepartamento)
        {
            return await _context.Provincias
                .Where(p => p.IdDepartamento == idDepartamento)
                .OrderBy(p => p.NombreProvincia)
                .ToListAsync();
        }

        public async Task<IEnumerable<Distrito>> GetDistritosByProvinciaAsync(int idProvincia)
        {
            return await _context.Distritos
                .Where(d => d.IdProvincia == idProvincia)
                .OrderBy(d => d.NombreDistrito)
                .ToListAsync();
        }
    }
}
