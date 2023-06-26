using MeuMenuPedido.Domain.Interfaces.Notificador;
using MeuMenuPedido.Domain.Notificador;
using Microsoft.AspNetCore.Mvc;

namespace MeuMenuPedido.Api.Controllers.Base;

public class BaseController : ControllerBase
{
    private readonly INotificador _notificador;

    public BaseController(INotificador notificador)
    {
        _notificador = notificador;
    }

    protected bool OperacaoValida() => !_notificador.TemNotificacao();

    protected IActionResult RespostaPadrao(object? result = null)
    {
        return OperacaoValida()
            ? Ok(new
            {
                success = true,
                data = result
            })
            : BadRequest(new
            {
                success = false,
                errors = _notificador.ObterNotificacoes().Select(x => x.Mensagem).Distinct()
            });
    }

    protected void NotificarErro(string mensagem)
    {
        _notificador.AdicionarNotificacao(new Notificacao(mensagem));
    }

    protected IActionResult RetornaMensagemErro(string mensagem)
    {
        _notificador.LimparNotificacoes();
        NotificarErro(mensagem);
        return RespostaPadrao();
    }
}