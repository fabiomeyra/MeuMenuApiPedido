using MeuMenuPedido.Application.Filtros.Pedido;
using MeuMenuPedido.Application.ViewModels;
using MeuMenuPedido.Application.ViewModels.Pedido;

namespace MeuMenuPedido.Application.Interfaces;

public interface IPedidoAppService
{
    ICollection<SituacaoPedidoViewModel> BuscarSituacoesPedido();
    Task<PedidoViewModel?> BuscarPorId(Guid id);
    Task<ICollection<PedidoViewModel>> BuscarPorSituacao(BuscarPedidoPorSituacaoFiltroViewModel filtroVm);
    Task<PedidoViewModel> Adicionar(AdicionarPedidoViewModel pedidoVm);
    Task<PedidoViewModel> AlterarSituacaoPedido(AlterarSituacaoPedidoViewModel pedidoVm);
}