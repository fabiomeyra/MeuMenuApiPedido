using MeuMenuPedido.Domain.Enum;
using MeuMenuPedido.Domain.Models.Base;

namespace MeuMenuPedido.Domain.Models;

public class Pedido : EntidadeValidavelModel<Pedido>
{
    public Guid PedidoId { get; set; }
    public int PedidoMesa { get; set; }
    public DateTime DataCadastro { get; set; }
    public SituacaoPedidoEnum? SituacaoPedidoId { get; set; }
    public string? PedidoObservacao { get; set; }
    public ICollection<PedidoProduto> ProdutosDoPedido { get; set; } = new List<PedidoProduto>();

    public override Pedido LimparPropriedadesNavegacao()
    {

        return this;
    }
}