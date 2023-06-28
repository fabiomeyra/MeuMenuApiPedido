using MeuMenuPedido.Domain.Enum;
using MeuMenuPedido.Domain.Models.Base;
using MeuMenuPedido.Domain.Services.Utils;

namespace MeuMenuPedido.Domain.Models;

public class Pedido : EntidadeValidavelModel<Pedido>
{
    public Guid PedidoId { get; set; }
    public int PedidoMesa { get; set; }
    public DateTime DataCadastro { get; set; }
    public SituacaoPedidoEnum? SituacaoPedidoId { get; set; }
    public string? PedidoObservacao { get; set; }
    public ICollection<PedidoProduto> ProdutosDoPedido { get; set; } = new List<PedidoProduto>();
    public decimal ValorTotal => ProdutosDoPedido.Select(x => x.ProdutoValor * x.ProdutoQuantidade).Sum();

    public bool UsuarioPodeFinalizarPedido(IUsuarioLogadoService usuarioLogadoService)
    {
        var usuario = usuarioLogadoService.ObterUsuarioLogado();
        return PermissoesParaFinalizarPedido.Any(x => x == usuario?.Permissao);
    }

    private ICollection<string> PermissoesParaFinalizarPedido => new List<string> { "ADMIN", "CAIXA" };

    public Pedido GerarNovoPedido()
    {
        PedidoId = Guid.NewGuid();
        SituacaoPedidoId = SituacaoPedidoEnum.Enviado;
        return this;
    }

    public override Pedido LimparPropriedadesNavegacao()
    {

        return this;
    }
}