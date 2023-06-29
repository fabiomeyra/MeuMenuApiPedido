using MeuMenuPedido.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MeuMenuPedido.Infra.Data.Context.Mappings;

public class MesaPedidoMapping : IEntityTypeConfiguration<MesaPedido>
{
    public void Configure(EntityTypeBuilder<MesaPedido> builder)
    {
        //  Primary key
        builder.HasKey(x => x.MesaPedidoNumero);

        builder.Property(x => x.MesaPedidoNumero)
            .HasColumnType("int")
            .IsRequired();

        //  Table
        builder.ToTable("MesaPedido", "Pedido");
    }
}