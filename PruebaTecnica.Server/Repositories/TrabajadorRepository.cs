using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PruebaTecnica.Server.Data;
using PruebaTecnica.Server.DTOs;
using PruebaTecnica.Server.Interfaces.Repositories;
using PruebaTecnica.Server.Models;
using System.Data;

namespace PruebaTecnica.Server.Repositories
{
    public class TrabajadorRepository : ITrabajadorRepository
    {
        private readonly AppDbContext _context;

        public TrabajadorRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TrabajadorDto>> GetAllAsync(string? sexo)
        {
            var sexoParam = new SqlParameter("@SexoFiltro", string.IsNullOrEmpty(sexo) ? DBNull.Value : sexo);
            return await _context.TrabajadorDtos
                .FromSqlRaw("EXEC sp_ListarTrabajadores @SexoFiltro", sexoParam)
                .ToListAsync();
        }

        public async Task<Trabajador?> GetByIdAsync(int id)
        {
            var idParam = new SqlParameter("@Id", id);
            var resultados = await _context.Trabajadores
                .FromSqlRaw("EXEC sp_ObtenerTrabajadorPorId @Id", idParam)
                .ToListAsync();
            return resultados.FirstOrDefault();
        }

        public async Task<int> CreateAsync(Trabajador trabajador)
        {
            var nuevoIdParam = new SqlParameter
            {
                ParameterName = "@NuevoId",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output,
            };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_CrearTrabajador @TipoDocumento, @NumeroDocumento, @Nombres, @Sexo, @IdDepartamento, @IdProvincia, @IdDistrito, @NuevoId OUTPUT",
                new SqlParameter("@TipoDocumento", trabajador.TipoDocumento),
                new SqlParameter("@NumeroDocumento", trabajador.NumeroDocumento),
                new SqlParameter("@Nombres", trabajador.Nombres),
                new SqlParameter("@Sexo", trabajador.Sexo),
                new SqlParameter("@IdDepartamento", trabajador.IdDepartamento as object ?? DBNull.Value),
                new SqlParameter("@IdProvincia", trabajador.IdProvincia as object ?? DBNull.Value),
                new SqlParameter("@IdDistrito", trabajador.IdDistrito as object ?? DBNull.Value),
                nuevoIdParam
            );

            return (int)nuevoIdParam.Value;
        }

        public async Task UpdateAsync(Trabajador trabajador)
        {
            var parameters = new[]
            {
                new SqlParameter("@Id", trabajador.Id),
                new SqlParameter("@TipoDocumento", trabajador.TipoDocumento),
                new SqlParameter("@NumeroDocumento", trabajador.NumeroDocumento),
                new SqlParameter("@Nombres", trabajador.Nombres),
                new SqlParameter("@Sexo", trabajador.Sexo),
                new SqlParameter("@IdDepartamento", trabajador.IdDepartamento as object ?? DBNull.Value),
                new SqlParameter("@IdProvincia", trabajador.IdProvincia as object ?? DBNull.Value),
                new SqlParameter("@IdDistrito", trabajador.IdDistrito as object ?? DBNull.Value)
            };

            await _context.Database.ExecuteSqlRawAsync("EXEC sp_ActualizarTrabajador @Id, @TipoDocumento, @NumeroDocumento, @Nombres, @Sexo, @IdDepartamento, @IdProvincia, @IdDistrito", parameters);
        }

        public async Task DeleteAsync(int id)
        {
            var idParam = new SqlParameter("@Id", id);
            await _context.Database.ExecuteSqlRawAsync("EXEC sp_EliminarTrabajador @Id", idParam);
        }

        public async Task<Trabajador?> FindByDocumentoAsync(string tipoDocumento, string numeroDocumento)
        {
            return await _context.Trabajadores
                .FirstOrDefaultAsync(t => t.TipoDocumento == tipoDocumento && t.NumeroDocumento == numeroDocumento);
        }
    }
}
