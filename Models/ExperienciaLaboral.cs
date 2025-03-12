using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace JuanSarmientoApp.Models
{
    public class ExperienciaLaboral
    {
        [Key]
        public int ExperienciaID { get; set; }

        [ForeignKey("Persona")]
        public int PersonaID { get; set; }

        [Required]
        [StringLength(100)]
        public string Empresa { get; set; }

        [Required]
        [StringLength(100)]
        public string Cargo { get; set; }

        [Required]
        public DateTime FechaInicio { get; set; }

        public DateTime? FechaFin { get; set; }

        public string Responsabilidades { get; set; }

        public Persona Persona { get; set; }
    }
}