using MeuMenuPedido.Api.Controllers.Base;
using MeuMenuPedido.Application.Filtros.Pedido;
using MeuMenuPedido.Application.Interfaces;
using MeuMenuPedido.Application.ViewModels;
using MeuMenuPedido.Application.ViewModels.Pedido;
using MeuMenuPedido.Domain.Interfaces.Notificador;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MeuMenuPedido.Api.Controllers;

[ApiController]
[Authorize("Bearer")]
[Route("api/pedido")]
public class PedidoController : BaseController
{
    private readonly IPedidoAppService _appService;

    public PedidoController(
        INotificador notificador, 
        IPedidoAppService appService) 
        : base(notificador)
    {
        _appService = appService;
    }

    /// <summary>
    /// Busca pedido por id
    /// </summary>
    /// <param name="id">id do pedido</param>
    /// <returns>Retorna pedido filtrado pelo id informado quando existir ou mensagem(s) de erro(s)</returns>
    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> ObterPorId([FromRoute] Guid id)
    {
        var retorno = await _appService.BuscarPorId(id);
        return RespostaPadrao(retorno);
    }

    /// <summary>
    /// Busca pedidos por situação
    /// </summary>
    /// <param name="filtro">Filtros a serem aplicados na busca</param>
    /// <returns>Retorna lista de pedidos filtrados de acordo com os valores informados no parâmetro filto ou mensagem(s) de erro(s)</returns>
    [HttpGet("buscar-por-situacao")]
    [AllowAnonymous]
    public async Task<IActionResult> BuscarPorSituacao([FromQuery] BuscarPedidoPorSituacaoFiltroViewModel filtro)
    {
        var retorno = await _appService.BuscarPorSituacao(filtro);
        return RespostaPadrao(retorno);
    }

    /// <summary>
    /// Retorna situações do pedido
    /// </summary>
    /// <returns>Lista de situações ou mensagem(s) de erro(s)</returns>
    [HttpGet("situacoes")]
    [AllowAnonymous]
    public IActionResult BuscarSituacoes()
    {
        var retorno = _appService.BuscarSituacoesPedido();
        return RespostaPadrao(retorno);
    }
    
    /// <summary>
    /// Rota para cadastro de novo pedido
    /// </summary>
    /// <param name="pedido">Pedido a ser cadastrado</param>
    /// <returns>Pedido acrescido de seu identificador ou mensagem(s) de erro</returns>
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Adicionar([FromBody] AdicionarPedidoViewModel pedido)
    {
        var retorno = await _appService.Adicionar(pedido);
        return RespostaPadrao(retorno);
    }


    /// <summary>
    /// Rota para cadastro de novo pedido
    /// </summary>
    /// <param name="pedido">Pedido a ser cadastrado</param>
    /// <returns>Pedido acrescido de seu identificador ou mensagem(s) de erro</returns>
    [HttpPatch("alterar-situacao")]
    public async Task<IActionResult> AlterarSituacao([FromBody] AlterarSituacaoPedidoViewModel pedido)
    {
        var retorno = await _appService.AlterarSituacaoPedido(pedido);
        return RespostaPadrao(retorno);
    }
}