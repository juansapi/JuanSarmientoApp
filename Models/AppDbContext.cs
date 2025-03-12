using Microsoft.EntityFrameworkCore;
namespace JuanSarmientoApp.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Persona> Persona { get; set; }
        public DbSet<ExperienciaLaboral> ExperienciaLaboral { get; set; }
        public DbSet<Educacion> Educacion { get; set; }
        public DbSet<Habilidad> Habilidad { get; set; }
        public DbSet<Competencia> Competencia { get; set; }
        public DbSet<PersonaHabilidad> PersonaHabilidad { get; set; }
        public DbSet<PersonaCompetencia> PersonaCompetencia { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //  Configurar clave primaria compuesta en la tabla intermedia
            modelBuilder.Entity<PersonaCompetencia>()
                .HasKey(pc => new { pc.PersonaID, pc.CompetenciaID });

            //  Configurar relaciones
            modelBuilder.Entity<PersonaCompetencia>()
                .HasOne(pc => pc.Persona)
                .WithMany(p => p.PersonaCompetencias)
                .HasForeignKey(pc => pc.PersonaID);

            modelBuilder.Entity<PersonaCompetencia>()
                .HasOne(pc => pc.Competencia)
                .WithMany(c => c.PersonaCompetencias)
                .HasForeignKey(pc => pc.CompetenciaID);

            //  Clave primaria compuesta para PersonaHabilidad
            modelBuilder.Entity<PersonaHabilidad>()
                .HasKey(ph => new { ph.PersonaID, ph.HabilidadID });

            //  Configurar relaciones de PersonaHabilidad
            modelBuilder.Entity<PersonaHabilidad>()
                .HasOne(ph => ph.Persona)
                .WithMany(p => p.PersonaHabilidades)
                .HasForeignKey(ph => ph.PersonaID);

            modelBuilder.Entity<PersonaHabilidad>()
                .HasOne(ph => ph.Habilidad)
                .WithMany(h => h.PersonaHabilidades)
                .HasForeignKey(ph => ph.HabilidadID);
        }
    }
}