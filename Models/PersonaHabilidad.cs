using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JuanSarmientoApp.Models
{
    public class PersonaHabilidad
    {
        [Key, Column(Order = 0)]
        [ForeignKey("Persona")]
        public int PersonaID { get; set; }

        [Key, Column(Order = 1)]
        [ForeignKey("Habilidad")]
        public int HabilidadID { get; set; }

        public Persona Persona { get; set; }
        public Habilidad Habilidad { get; set; }
    }
}