using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using CadastroClienteAPP.Domain.Entities;

namespace CadastroClienteAPP.Infrastructure.Mappings
{
    public class ClienteMap : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.ToTable("Clientes");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Nome)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(c => c.Documento)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(c => c.TipoPessoa)
                .IsRequired()
                .HasMaxLength(1); // 'F' ou 'J'

            builder.Property(c => c.DataNascimento)
                .IsRequired();

            builder.Property(c => c.Telefone)
                .HasMaxLength(20);

            builder.Property(c => c.Email)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.InscricaoEstadual)
                .HasMaxLength(50);

            builder.Property(c => c.Isento)
                .IsRequired();

            // Relacionamento 1:1 com Endereco
            builder.HasOne(c => c.Endereco)
                .WithOne()
                .HasForeignKey<Endereco>(e => e.ClienteId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
