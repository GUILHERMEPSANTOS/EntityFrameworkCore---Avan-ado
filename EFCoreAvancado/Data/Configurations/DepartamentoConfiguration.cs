using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

            builder.HasKey(d => d.Id);
            builder.Property(d => d.Descricao).HasMaxLength(512);
            builder.Property(d => d.Ativo);

            builder
                .HasMany(d => d.Funcionarios)
                .WithOne(f => f.Departamento);
        }
    }
}