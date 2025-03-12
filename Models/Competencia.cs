using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JuanSarmientoApp.Models
{
    public class Competencia
    {
        [Key]
        public int CompetenciaID { get; set; }

        [Required]
        [StringLength(100)]
        public string NombreCompetencia { get; set; }

        // Relación Muchos a Muchos con Persona
        public ICollection<PersonaCompetencia> PersonaCompetencias { get; set; }
    }
}