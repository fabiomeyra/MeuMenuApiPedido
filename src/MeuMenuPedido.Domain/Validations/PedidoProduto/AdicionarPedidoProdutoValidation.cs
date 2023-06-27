using FluentValidation;

namespace MeuMenuPedido.Domain.Validations.PedidoProduto;

public class AdicionarPedidoProdutoValidation : AbstractValidator<Models.PedidoProduto>
{
    public AdicionarPedidoProdutoValidation()
    {
        RuleFor(x => x.ProdutoId)
            .Must(x => x != Guid.Empty)
            .WithMessage(RetornaMensagemFormatado(MensagensValidacaoResources.ProdutoObrigatorio));

        RuleFor(x => x.PedidoId)
            .Must(x => x != Guid.Empty)
            .WithMessage(RetornaMensagemFormatado(MensagensValidacaoResources.IdentificadorDoPedidoObrigatorio));

        RuleFor(x => x.ProdutoQuantidade)
            .Must(x => x > 0)
            .WithMessage(RetornaMensagemFormatado(MensagensValidacaoResources.QuantidadeDoProdutoDeveSerSuperiorAZero));
    }

    private string RetornaMensagemFormatado(string mensage) => $"(Produto do pedido): {mensage}";
}