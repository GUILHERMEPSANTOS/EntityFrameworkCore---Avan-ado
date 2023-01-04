using EFCoreAvancado.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace EFCoreAvancado.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            const string strConnection = "Data Source=.;Initial Catalog=EFAvancado2;Integrated Security=True;Encrypt=false;pooling=true";

            optionsBuilder
                .UseSqlServer(strConnection)
                 // .LogTo(Console.WriteLine, LogLevel.Information);
                 .LogTo(
                    Console.WriteLine,
                    new[] { CoreEventId.ContextInitialized, RelationalEventId.CommandExecuted },
                    LogLevel.Information,
                    options: DbContextLoggerOptions.LocalTime | DbContextLoggerOptions.Id
                 );

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}