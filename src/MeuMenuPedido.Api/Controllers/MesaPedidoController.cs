using MeuMenuPedido.Api.Controllers.Base;
using MeuMenuPedido.Application.Interfaces;
using MeuMenuPedido.Application.ViewModels.MesaPedido;
using MeuMenuPedido.Domain.Interfaces.Notificador;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MeuMenuPedido.Api.Controllers;

[ApiController]
[Authorize("Bearer")]
[Route("api/mesa")]
public class MesaPedidoController : BaseController
{
    private readonly IMesaPedidoAppService _appService;

    public MesaPedidoController(
        INotificador notificador, 
        IMesaPedidoAppService appService
        ) : base(notificador)
    {
        _appService = appService;
    }

    /// <summary>
    /// Rota para ocupar uma mesa
    /// </summary>
    /// <param name="mesa"></param>
    /// <returns>Retorna sucesso ou mensagem(s) de erro(s)</returns>
    [HttpPost("ocupar")]
    public async Task<IActionResult> Ocupar([FromBody] MesaPedidoViewModel mesa)
    {
        await _appService.OcuparMesa(mesa.MesaPedidoNumero);
        return RespostaPadrao();
    }

    /// <summary>
    /// Verifica se pode iniciar um novo pedido na mesa informada
    /// </summary>
    /// <param name="mesa"></param>
    /// <returns>Retorna sucesso ou mensagem(s) de erro(s)</returns>
    [HttpPost("desocupar")]
    public async Task<IActionResult> Desocupar([FromBody] MesaPedidoViewModel mesa)
    {
        await _appService.DesocuparMesa(mesa.MesaPedidoNumero);
        return RespostaPadrao();
    }
}