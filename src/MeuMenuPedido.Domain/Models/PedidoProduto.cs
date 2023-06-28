using MeuMenuPedido.Domain.Models.Base;
using MeuMenuPedido.Domain.ValueObjects.Integracoes.ApiProdutos;

namespace MeuMenuPedido.Domain.Models;

public class PedidoProduto : EntidadeValidavelModel<PedidoProduto>
{
    public Guid PedidoId { get; set; }
    public Guid ProdutoId { get; set; }
    public decimal ProdutoValor { get; set; }
    public int ProdutoQuantidade { get; set; }
    public ProdutoDescricaoImagemValueObject? Produto { get; set; }
    public virtual Pedido? Pedido { get; set; }

    public override PedidoProduto LimparPropriedadesNavegacao()
    {
        Pedido = null;
        return this;
    }
}