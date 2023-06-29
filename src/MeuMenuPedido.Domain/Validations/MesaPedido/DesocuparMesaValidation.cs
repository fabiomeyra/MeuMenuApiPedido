using FluentValidation;
using MeuMenuPedido.Domain.Interfaces.Services;

namespace MeuMenuPedido.Domain.Validations.MesaPedido;

public class DesocuparMesaValidation : AbstractValidator<Models.MesaPedido>
{
    public DesocuparMesaValidation(IMesaPedidoService mesaPedidoService)
    {
        RuleFor(x => x.MesaPedidoNumero)
            .Must(x => x > 0)
            .WithMessage(RetornaMensagemFormatado(MensagensValidacaoResources.MesaObrigatoria));

        RuleFor(x => x.MesaPedidoNumero)
            .MustAsync(async (x, _) =>
            {
                var mesaLivre = await mesaPedidoService.PodeAbrirMesaPedidoNaMesa(x);
                return !mesaLivre;
            })
            .When(x => x.MesaPedidoNumero > 0)
            .WithMessage(RetornaMensagemFormatado(MensagensValidacaoResources.MesaNaoIdentificada));
    }

    private string RetornaMensagemFormatado(string mensage) => $"(Mesa): {mensage}";
}