using EFCoreAvancado.Domain;
using Microsoft.EntityFrameworkCore;

namespace EFCoreAvancado.Data
{
    public class ApplicationDbContextCidade: DbContext
    {
        public DbSet<Cidade> Cidades { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            const string strConnection = "Data Source=.;Initial Catalog=EFAvancado;Integrated Security=True;Encrypt=false;pooling=true";

            optionsBuilder
                .EnableSensitiveDataLogging()
                .LogTo(Console.WriteLine)
                .UseSqlServer(strConnection);
        }
    }
}