using MeuMenuPedido.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MeuMenuPedido.Infra.Data.Context.Mappings;

public class PedidoMapping : IEntityTypeConfiguration<Pedido>
{
    public void Configure(EntityTypeBuilder<Pedido> builder)
    {
        //  Primary key
        builder.HasKey(x => x.PedidoId);

        builder.Property(x => x.PedidoMesa)
            .HasColumnType("int")
            .IsRequired();

        builder.Property(x => x.DataCadastro)
            .HasColumnType("datetime")
            .IsRequired();

        builder.Property(x => x.SituacaoPedidoId)
            .HasColumnType("int")
            .IsRequired();

        builder.Property(x => x.PedidoObservacao)
            .HasColumnType("varchar(2000)");

        builder.Ignore(x => x.ValorTotal);

        //  Table
        builder.ToTable("Pedido", "Pedido");
    }
}