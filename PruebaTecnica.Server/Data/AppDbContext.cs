using Microsoft.EntityFrameworkCore;
using PruebaTecnica.Server.Models;
using PruebaTecnica.Server.DTOs;

namespace PruebaTecnica.Server.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Provincia> Provincias { get; set; }
        public DbSet<Distrito> Distritos { get; set; }
        public DbSet<Trabajador> Trabajadores { get; set; }

        // DbSet para el DTO que mapeará el resultado del Stored Procedure
        public DbSet<TrabajadorDto> TrabajadorDtos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<TrabajadorDto>().HasNoKey();
        }
    }
}
