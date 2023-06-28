using MeuMenuPedido.Application.ViewModels.Produto;

namespace MeuMenuPedido.Application.ViewModels.Pedido;

public class AdicionarPedidoViewModel
{
    public string? PedidoObservacao { get; set; }
    public int PedidoMesa { get; set; }
    public ICollection<ProdutoQuantidadeViewModel> Produtos { get; set; } = new List<ProdutoQuantidadeViewModel>();
}