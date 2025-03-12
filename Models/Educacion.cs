using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace JuanSarmientoApp.Models
{
    public class Educacion
    {
        [Key]
        public int EducacionID { get; set; }

        [ForeignKey("Persona")]
        public int PersonaID { get; set; }

        [Required]
        [StringLength(150)]
        public string Institucion { get; set; }

        [Required]
        [StringLength(100)]
        public string TituloObtenido { get; set; }

        [Required]
        public DateTime FechaGraduacion { get; set; }

        public Persona Persona { get; set; }
    }
}