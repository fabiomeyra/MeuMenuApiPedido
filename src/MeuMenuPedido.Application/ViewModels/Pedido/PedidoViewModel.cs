namespace MeuMenuPedido.Application.ViewModels.Pedido;

public class PedidoViewModel
{
    public Guid PedidoId { get; set; }
    public int PedidoMesa { get; set; }
    public DateTime DataCadastro { get; set; }
    public int? SituacaoPedidoId { get; set; }
    public string? SituacaoPedidoDrescricao { get; set; }
    public string? PedidoObservacao { get; set; }
    public decimal ValorTotal { get; set; }
    public ICollection<PedidoProdutoViewModel> ProdutosDoPedido { get; set; } = new List<PedidoProdutoViewModel>();
}