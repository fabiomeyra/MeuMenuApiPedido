using MeuMenuPedido.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MeuMenuPedido.Infra.Data.Context.Mappings;

public class PedidoProdutoMapping : IEntityTypeConfiguration<PedidoProduto>
{
    public void Configure(EntityTypeBuilder<PedidoProduto> builder)
    {
        //  Primary key
        builder.HasKey(x => new { x.PedidoId, x.ProdutoId });

        builder.Property(x => x.PedidoId)
            .HasColumnType("uniqueidentifier")
            .IsRequired();

        builder.Property(x => x.ProdutoId)
            .HasColumnType("uniqueidentifier")
            .IsRequired();

        builder.Property(x => x.ProdutoValor)
            .HasColumnType("decimal(19,2)")
            .IsRequired();

        builder.Property(x => x.ProdutoQuantidade)
            .HasColumnType("int")
            .IsRequired();

        // Relacionamento
        builder.HasOne(x => x.Pedido)
            .WithMany(x => x.ProdutosDoPedido)
            .HasForeignKey(x => x.PedidoId);

        //  Table
        builder.ToTable("Pedido", "PedidoProduto");
    }
}