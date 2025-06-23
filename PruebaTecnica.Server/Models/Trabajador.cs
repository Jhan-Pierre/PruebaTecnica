using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PruebaTecnica.Server.Models
{
    [Table("Trabajadores")]
    public class Trabajador
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El tipo de documento es obligatorio.")]
        [StringLength(3)]
        public string? TipoDocumento { get; set; }

        [Required(ErrorMessage = "El número de documento es obligatorio.")]
        [StringLength(50)]
        public string? NumeroDocumento { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(500)]
        public string? Nombres { get; set; }

        [Required(ErrorMessage = "El sexo es obligatorio.")]
        [StringLength(1)]
        public string? Sexo { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un departamento.")]
        [Range(1, int.MaxValue, ErrorMessage = "El departamento seleccionado no es válido.")]
        public int? IdDepartamento { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una provincia.")]
        [Range(1, int.MaxValue, ErrorMessage = "La provincia seleccionada no es válida.")]
        public int? IdProvincia { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un distrito.")]
        [Range(1, int.MaxValue, ErrorMessage = "El distrito seleccionado no es válido.")]
        public int? IdDistrito { get; set; }

        [ForeignKey("IdDepartamento")]
        public virtual Departamento? Departamento { get; set; }

        [ForeignKey("IdProvincia")]
        public virtual Provincia? Provincia { get; set; }

        [ForeignKey("IdDistrito")]
        public virtual Distrito? Distrito { get; set; }
    }
}
