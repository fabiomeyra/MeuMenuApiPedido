using AutoMapper;
using MeuMenuPedido.Application.AppServices.Base;
using MeuMenuPedido.Application.Interfaces;
using MeuMenuPedido.Domain.Services.Base;
using MeuMenuPedido.Domain.UoW;

namespace MeuMenuPedido.Application.AppServices;

public class PedidoAppService : BaseAppService, IPedidoAppService
{
    public PedidoAppService(
        IUnitOfWork ouw, 
        NegocioService negocioService, 
        IMapper mapper) 
            : base(ouw, negocioService, mapper)
    {
    }
}