using MeuMenuPedido.Domain.Enum;
using MeuMenuPedido.Domain.Filtros.Pedido;
using MeuMenuPedido.Domain.Interfaces.Services.Base;
using MeuMenuPedido.Domain.Models;

namespace MeuMenuPedido.Domain.Interfaces.Services;

public interface IPedidoService : IBaseService<Pedido>
{
    Task<Pedido?> BuscarPorId(Guid id);
    Task<ICollection<Pedido>> BuscarPorSituacao(BuscarPedidoPorSituacaoFiltro filtro);
    Task AdicionarImagemDescricaoPedidos(ICollection<PedidoProduto> produtosDoPedido);
    Task<Pedido> AlterarSituacaoPedido(Guid pedidoId, SituacaoPedidoEnum situacao);
}