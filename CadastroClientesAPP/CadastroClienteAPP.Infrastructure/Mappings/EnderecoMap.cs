using CadastroClienteAPP.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadastroClienteAPP.Infrastructure.Mappings
{
    public class EnderecoMap : IEntityTypeConfiguration<Endereco>
    {
        public void Configure(EntityTypeBuilder<Endereco> builder)
        {
            builder.ToTable("Enderecos");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Cep)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(e => e.Logradouro)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.Numero)
                .HasMaxLength(100);

            builder.Property(e => e.Bairro)
                .HasMaxLength(50);

            builder.Property(e => e.Cidade)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(e => e.Estado)
                .IsRequired()
                .HasMaxLength(2);

            
            builder.Property<Guid>("ClienteId")
                   .IsRequired();

            builder.HasIndex("ClienteId").IsUnique();
        }
    }
}
