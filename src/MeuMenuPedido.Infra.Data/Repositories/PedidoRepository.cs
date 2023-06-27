using MeuMenuPedido.Domain.Interfaces.Repositories;
using MeuMenuPedido.Domain.Models;
using MeuMenuPedido.Infra.Data.Context;
using MeuMenuPedido.Infra.Data.Repositories.Base;

namespace MeuMenuPedido.Infra.Data.Repositories;

public class PedidoRepository : BaseRepository<Pedido>, IPedidoRepository
{
    public PedidoRepository(MeuMenuPedidoContext contexto) 
        : base(contexto)
    {
    }
}