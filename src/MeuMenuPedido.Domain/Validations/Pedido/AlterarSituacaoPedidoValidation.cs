using FluentValidation;
using MeuMenuPedido.Domain.Enum;
using MeuMenuPedido.Domain.Services.Utils;

namespace MeuMenuPedido.Domain.Validations.Pedido;

public class AlterarSituacaoPedidoValidation : AbstractValidator<Models.Pedido>
{

    public AlterarSituacaoPedidoValidation(SituacaoPedidoEnum? situacaoAnterior, IUsuarioLogadoService usuarioLogadoService)
    {
        RuleFor(x => x)
            .Must(_ => SituacaoDiferenteDePagoECancelado(situacaoAnterior))
            .WithMessage(RetornaMensagemFormatado(MensagensValidacaoResources.PedidoPagoOuCanceladoNaoPodeMudarSituacao));

        RuleFor(x => x.SituacaoPedidoId)
            .Must(x => x is > 0 && System.Enum.IsDefined(typeof(SituacaoPedidoEnum), x))
            .WithMessage(RetornaMensagemFormatado(MensagensValidacaoResources.PedidoPagoOuCanceladoNaoPodeMudarSituacao));

        RuleFor(x => x.SituacaoPedidoId)
            .Must(x => x is > 0 && System.Enum.IsDefined(typeof(SituacaoPedidoEnum), x))
            .WithMessage(RetornaMensagemFormatado(MensagensValidacaoResources.PedidoPagoOuCanceladoNaoPodeMudarSituacao));

        RuleFor(x => x)
            .Must(x => SituacaoDiferenteDePagoECancelado(x.SituacaoPedidoId) || x.UsuarioPodeFinalizarPedido(usuarioLogadoService))
            .When(x => x.SituacaoPedidoId is > 0 && System.Enum.IsDefined(typeof(SituacaoPedidoEnum), x.SituacaoPedidoId))
            .WithMessage(RetornaMensagemFormatado(MensagensValidacaoResources.UsuarioSemPermissao));
    }

    private bool SituacaoDiferenteDePagoECancelado(SituacaoPedidoEnum? situacaoAtual) =>
        new[] { SituacaoPedidoEnum.Cancelado, SituacaoPedidoEnum.PedidoPago }.All(x => x != situacaoAtual);

    private string RetornaMensagemFormatado(string mensage) => $"(Pedido): {mensage}";
}