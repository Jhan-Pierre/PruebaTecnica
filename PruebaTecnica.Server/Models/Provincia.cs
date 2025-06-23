using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PruebaTecnica.Server.Models
{
    [Table("Provincia")]
    public class Provincia
    {
        [Key]
        public int Id { get; set; }

        public int? IdDepartamento { get; set; }

        public string? NombreProvincia { get; set; }

        [ForeignKey("IdDepartamento")]
        public virtual Departamento? Departamento { get; set; }
    }
}
