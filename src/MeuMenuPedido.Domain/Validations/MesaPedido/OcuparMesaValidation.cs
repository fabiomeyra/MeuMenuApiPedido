using FluentValidation;
using MeuMenuPedido.Domain.Interfaces.Services;

namespace MeuMenuPedido.Domain.Validations.MesaPedido;

public class OcuparMesaValidation : AbstractValidator<Models.MesaPedido>
{
    public OcuparMesaValidation(IMesaPedidoService mesaPedidoService)
    {
        RuleFor(x => x.MesaPedidoNumero)
            .Must(x => x > 0)
            .WithMessage(RetornaMensagemFormatado(MensagensValidacaoResources.MesaObrigatoria));

        RuleFor(x => x.MesaPedidoNumero)
            .MustAsync(async (x, _) => await mesaPedidoService.PodeAbrirMesaPedidoNaMesa(x))
            .When(x => x.MesaPedidoNumero > 0)
            .WithMessage(RetornaMensagemFormatado(MensagensValidacaoResources.MesaOcupada));
    }

    private string RetornaMensagemFormatado(string mensage) => $"(Mesa): {mensage}";
}