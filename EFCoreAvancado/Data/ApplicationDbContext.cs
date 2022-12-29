using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFCoreAvancado.Domain;
using Microsoft.EntityFrameworkCore;

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
                .EnableSensitiveDataLogging()
            .LogTo(Console.WriteLine);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            // .Entity<Departamento>().HasQueryFilter(departamento => !departamento.Excluido);
        }
    }
}