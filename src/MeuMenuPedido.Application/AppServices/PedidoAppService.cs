using AutoMapper;
using MeuMenuPedido.Application.AppServices.Base;
using MeuMenuPedido.Application.Filtros.Pedido;
using MeuMenuPedido.Application.Interfaces;
using MeuMenuPedido.Application.ViewModels;
using MeuMenuPedido.Application.ViewModels.Pedido;
using MeuMenuPedido.Domain.Enum;
using MeuMenuPedido.Domain.Filtros.Pedido;
using MeuMenuPedido.Domain.Interfaces.Services;
using MeuMenuPedido.Domain.Models;
using MeuMenuPedido.Domain.Services.Base;
using MeuMenuPedido.Domain.UoW;
using MeuMenuPedido.Domain.Utils;

namespace MeuMenuPedido.Application.AppServices;

public class PedidoAppService : BaseAppService, IPedidoAppService
{
    private readonly IPedidoService _service;

    public PedidoAppService(
        IUnitOfWork ouw, 
        NegocioService negocioService, 
        IMapper mapper,
        IPedidoService service) 
            : base(ouw, negocioService, mapper)
    {
        _service = service;
    }

    public ICollection<SituacaoPedidoViewModel> BuscarSituacoesPedido()
    {
        var lista = SituacaoPedidoEnum.Enviado.ParaListaDeSelecao();
        return lista.Select(x => new SituacaoPedidoViewModel
        {
            SituacaoPedidoId = x.Valor,
            SituacaoPedidoDescricao = x.Nome
        }).ToList();
    }

    public async Task<PedidoViewModel?> BuscarPorId(Guid id)
    {
        var retorno = await _service.BuscarPorId(id);
        if (retorno is null) return null;
        await _service.AdicionarImagemDescricaoPedidos(retorno.ProdutosDoPedido);
        return Mapper.Map<PedidoViewModel>(retorno);
    }

    public async Task<ICollection<PedidoViewModel>> BuscarPorSituacao(BuscarPedidoPorSituacaoFiltroViewModel filtroVm)
    {
        var filtro = Mapper.Map<BuscarPedidoPorSituacaoFiltro>(filtroVm);
        var retorno = await _service.BuscarPorSituacao(filtro);
        await _service.AdicionarImagemDescricaoPedidos(retorno.SelectMany(x => x.ProdutosDoPedido).ToList());
        return Mapper.Map<ICollection<PedidoViewModel>>(retorno);
    }

    public async Task<PedidoViewModel> Adicionar(AdicionarPedidoViewModel pedidoVm)
    {
        var model = Mapper.Map<Pedido>(pedidoVm);
        model = await _service.Adicionar(model);
        await Commit();
        return Mapper.Map<PedidoViewModel>(model);
    }

    public async Task<PedidoViewModel> AlterarSituacaoPedido(AlterarSituacaoPedidoViewModel pedidoVm)
    {
        var retorno = await _service.AlterarSituacaoPedido(pedidoVm.PedidoId, (SituacaoPedidoEnum) pedidoVm.SituacaoId);
        await Commit();
        return Mapper.Map<PedidoViewModel>(retorno);
    }
}