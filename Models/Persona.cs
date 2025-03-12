using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JuanSarmientoApp.Models
{ 
public class Persona
{
        [Key]
        public int PersonaID { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [StringLength(255)]
        public string Direccion { get; set; }

        [StringLength(20)]
        public string Telefono { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string CorreoElectronico { get; set; }

        public ICollection<ExperienciaLaboral> ExperienciaLaboral { get; set; }
        public ICollection<Educacion> Educacion { get; set; }
        public ICollection<PersonaHabilidad> PersonaHabilidades { get; set; }
        public ICollection<PersonaCompetencia> PersonaCompetencias { get; set; }
    }
}