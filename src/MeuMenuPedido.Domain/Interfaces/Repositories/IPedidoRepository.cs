using MeuMenuPedido.Domain.Filtros.Pedido;
using MeuMenuPedido.Domain.Interfaces.Repositories.Base;
using MeuMenuPedido.Domain.Models;

namespace MeuMenuPedido.Domain.Interfaces.Repositories;

public interface IPedidoRepository : IBaseRepository<Pedido>
{
    Task<Pedido?> BuscarPorId(Guid id);
    Task<ICollection<Pedido>> BuscarPorSituacao(BuscarPedidoPorSituacaoFiltro filtro);
}