using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JuanSarmientoApp.Models
{
    public class Habilidad
    {
        [Key]
        public int HabilidadID { get; set; }

        [Required]
        [StringLength(100)]
        public string NombreHabilidad { get; set; }

        //  Relación con Personas
        public ICollection<PersonaHabilidad> PersonaHabilidades { get; set; }
    }
}