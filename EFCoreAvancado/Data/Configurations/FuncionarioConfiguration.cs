using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFCoreAvancado.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCoreAvancado.Data.Configurations
{
    public class FuncionarioConfiguration : IEntityTypeConfiguration<Funcionario>
    {
        public void Configure(EntityTypeBuilder<Funcionario> builder)
        {
            builder.ToTable("Funcionarios");

            builder.HasKey(f => f.Id);
            builder.Property(f => f.Nome).HasMaxLength(50).IsRequired();
            builder.Property(f => f.CPF).HasColumnType("CHAR(11)").IsRequired();

            builder
              .HasOne(f => f.Departamento)
              .WithMany(d => d.Funcionarios)
              .HasForeignKey(f => f.DepartamentoId);
        }
    }
}