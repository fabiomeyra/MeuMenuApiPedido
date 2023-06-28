using MeuMenuPedido.Application.ViewModels.Produto;

namespace MeuMenuPedido.Application.ViewModels.Pedido;

public class PedidoProdutoViewModel
{
    public Guid PedidoId { get; set; }
    public Guid ProdutoId { get; set; }
    public decimal ProdutoValor { get; set; }
    public int ProdutoQuantidade { get; set; }
    public virtual PedidoViewModel? Pedido { get; set; }
    public ProdutoDescricaoImagemViewModel? Produto { get; set; }
}