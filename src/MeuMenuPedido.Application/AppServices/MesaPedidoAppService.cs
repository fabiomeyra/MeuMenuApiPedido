using AutoMapper;
using MeuMenuPedido.Application.AppServices.Base;
using MeuMenuPedido.Application.Interfaces;
using MeuMenuPedido.Domain.Interfaces.Services;
using MeuMenuPedido.Domain.Models;
using MeuMenuPedido.Domain.Services.Base;
using MeuMenuPedido.Domain.UoW;

namespace MeuMenuPedido.Application.AppServices;

public class MesaPedidoAppService : BaseAppService, IMesaPedidoAppService
{
    private readonly IMesaPedidoService _service;

    public MesaPedidoAppService(
        IUnitOfWork ouw, 
        NegocioService negocioService, 
        IMapper mapper,
        IMesaPedidoService service) 
            : base(ouw, negocioService, mapper)
    {
        _service = service;
    }

    public async Task OcuparMesa(int numeroMesa)
    {
        var mesa = new MesaPedido { MesaPedidoNumero = numeroMesa };
        await _service.Adicionar(mesa);
        await Commit();
    }

    public async Task DesocuparMesa(int numeroMesa)
    {
        var mesa = new MesaPedido { MesaPedidoNumero = numeroMesa };
        await _service.Excluir(mesa);
        await Commit();
    }
}