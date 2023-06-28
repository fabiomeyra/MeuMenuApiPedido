using MeuMenuPedido.Application.Filtros.Base;

namespace MeuMenuPedido.Application.Filtros.Pedido;

public class BuscarPedidoPorSituacaoFiltroViewModel : BaseFiltroViewModel
{
    public int SituacaoPedido { get; set; }
    public bool OrdenarPorMaisRecentes { get; set; }
}