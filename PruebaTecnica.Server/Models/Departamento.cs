using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PruebaTecnica.Server.Models
{
    [Table("Departamento")]
    public class Departamento
    {
        [Key]
        public int Id { get; set; }
        public string? NombreDepartamento { get; set; }
    }
}
