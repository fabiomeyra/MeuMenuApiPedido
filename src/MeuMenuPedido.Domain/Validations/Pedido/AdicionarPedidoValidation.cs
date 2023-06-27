using FluentValidation;
using MeuMenuPedido.Domain.Validations.PedidoProduto;

namespace MeuMenuPedido.Domain.Validations.Pedido;

public class AdicionarPedidoValidation : AbstractValidator<Models.Pedido>
{
    //private readonly IUsuarioService _usuarioService;

    public AdicionarPedidoValidation()
    {
        RuleFor(x => x.PedidoMesa)
            .Must(x => x > 0)
            .WithMessage(RetornaMensagemFormatado(MensagensValidacaoResources.MesaObrigatoria));

        RuleFor(x => x.ProdutosDoPedido)
            .Must(x => x.Any())
            .WithMessage(RetornaMensagemFormatado(MensagensValidacaoResources.DeveInformarProdutoNoPedido));

        RuleForEach(x => x.ProdutosDoPedido)
            .SetValidator(x => new AdicionarPedidoProdutoValidation())
            .When(x => x.ProdutosDoPedido.Any());
    }

    private string RetornaMensagemFormatado(string mensage) => $"(Pedido): {mensage}";
}