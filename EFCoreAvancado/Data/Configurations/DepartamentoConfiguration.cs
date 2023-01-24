using EFCoreAvancado.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCoreAvancado.Data.Configurations
{
    public class DepartamentoConfiguration : IEntityTypeConfiguration<Departamento>

    {
        public void Configure(EntityTypeBuilder<Departamento> builder)
        {
            builder.ToTable("Departamentos");

            builder.Property(d => d.Id); // -- Sequencia .HasDefaultValueSql("NEXT VALUE FOR sequencia.MinhaSequencia");
            builder.Property(d => d.Descricao).HasMaxLength(512); // -- Collactions .UseCollation("SQL_Latin1_General_CP1_CI_AI");
            builder.Property(d => d.Ativo);

            builder
                .HasMany(d => d.Funcionarios)
                .WithOne(f => f.Departamento);
        }
    }
}