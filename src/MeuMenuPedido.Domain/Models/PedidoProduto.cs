using MeuMenuPedido.Domain.Models.Base;

namespace MeuMenuPedido.Domain.Models;

public class PedidoProduto : EntidadeValidavelModel<PedidoProduto>
{
    public Guid PedidoId { get; set; }
    public Guid ProdutoId { get; set; }
    public decimal ProdutoValor { get; set; }
    public int ProdutoQuantidade { get; set; }
    public virtual Pedido? Pedido { get; set; }

    public override PedidoProduto LimparPropriedadesNavegacao()
    {
        Pedido = null;
        return this;
    }
}