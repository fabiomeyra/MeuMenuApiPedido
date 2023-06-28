using MeuMenuPedido.Domain.Enum;
using MeuMenuPedido.Domain.Filtros.Base;

namespace MeuMenuPedido.Domain.Filtros.Pedido;

public class BuscarPedidoPorSituacaoFiltro : BaseFiltro
{
    public SituacaoPedidoEnum SituacaoPedido { get; set; }
    public bool OrdenarPorMaisRecentes { get; set; }
}