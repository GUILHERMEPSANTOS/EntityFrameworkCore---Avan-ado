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
                .EnableSensitiveDataLogging()
                .LogTo(
                    Console.WriteLine,
                    new[] { CoreEventId.ContextInitialized, RelationalEventId.CommandExecuted },
                    LogLevel.Information,
                    DbContextLoggerOptions.LocalTime
                 )
                .UseSqlServer(strConnection)
                .EnableDetailedErrors();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            /* 

            --Collactions
            builder.UseCollation("SQL_Latin1_General_CP1_CI_AI");

            -- Sequencias
            builder
                .HasSequence<int>("MinhaSequencia", "sequencia")
                .StartsAt(1)
                .IncrementsBy(2)
                .HasMin(1)
                .HasMax(10)
                .IsCyclic();
             */

             builder.Entity<Departamento>()
                .HasIndex(p => new { p.Descricao, p.Ativo });
        }
    }
}