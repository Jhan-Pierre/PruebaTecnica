using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PruebaTecnica.Server.Models
{
    [Table("Distrito")]
    public class Distrito
    {
        [Key]
        public int Id { get; set; }

        public int? IdProvincia { get; set; }

        public string? NombreDistrito { get; set; }

        [ForeignKey("IdProvincia")]
        public virtual Provincia? Provincia { get; set; }
    }
}
