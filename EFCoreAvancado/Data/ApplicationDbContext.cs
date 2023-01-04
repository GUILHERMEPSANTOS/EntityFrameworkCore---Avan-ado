using EFCoreAvancado.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace EFCoreAvancado.Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly StreamWriter _writer = new StreamWriter("log_ef_core.txt", append: true);
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            const string strConnection = "Data Source=.;Initial Catalog=EFAvancado2;Integrated Security=True;Encrypt=false;pooling=true";

            optionsBuilder
                .UseSqlServer(strConnection, options => options.MaxBatchSize(20))
                .LogTo(Console.WriteLine, LogLevel.Information);
            //  .LogTo(
            //     Console.WriteLine,
            //     new[] { CoreEventId.ContextInitialized, RelationalEventId.CommandExecuted },
            //     LogLevel.Information,
            //     options: DbContextLoggerOptions.LocalTime | DbContextLoggerOptions.Id
            //  );
            // .LogTo(_writer.WriteLine, LogLevel.Information);
            // .EnableDetailedErrors();

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }

        public override void Dispose()
        {
            base.Dispose();

            _writer.Dispose();
        }
    }
}