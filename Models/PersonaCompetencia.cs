using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace JuanSarmientoApp.Models
{
    public class PersonaCompetencia
    {
        [Key, Column(Order = 0)]
        [ForeignKey("Persona")]
        public int PersonaID { get; set; }

        [Key, Column(Order = 1)]
        [ForeignKey("Competencia")]
        public int CompetenciaID { get; set; }

        public Persona Persona { get; set; }
        public Competencia Competencia { get; set; }
    }
}