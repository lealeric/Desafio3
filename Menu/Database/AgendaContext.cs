using AgendaConsultorio.Model;
using Microsoft.EntityFrameworkCore;

namespace AgendaConsultorio.Database
{
    public class AgendaContext : DbContext
    {
        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Consulta> Consultas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Consulta>().
                HasOne(consulta => consulta.Paciente).
                WithMany(paciente => paciente.Consultas).
                HasForeignKey(consulta => consulta.PacienteId);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
            optionsBuilder.UseLazyLoadingProxies().
                UseSqlServer("Server=DESKTOP-TIGCP3C\\MSSQLSERVER01;Database=AgendaDB;Trusted_Connection=true;");
            
        
    }
}
