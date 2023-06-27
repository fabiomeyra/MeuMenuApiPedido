using AutoMapper;
using MeuMenuPedido.Application.AppServices.Base;
using MeuMenuPedido.Application.Interfaces;
using MeuMenuPedido.Domain.Services.Base;
using MeuMenuPedido.Domain.UoW;

namespace MeuMenuPedido.Application.AppServices;

public class PedidoProdutoAppService : BaseAppService, IPedidoProdutoAppService
{
    public PedidoProdutoAppService(
        IUnitOfWork ouw, 
        NegocioService negocioService, 
        IMapper mapper) 
            : base(ouw, negocioService, mapper)
    {
    }
}