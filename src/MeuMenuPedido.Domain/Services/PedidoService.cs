using MeuMenuPedido.Domain.Interfaces.Repositories;
using MeuMenuPedido.Domain.Interfaces.Services;
using MeuMenuPedido.Domain.Models;
using MeuMenuPedido.Domain.Services.Base;

namespace MeuMenuPedido.Domain.Services;

public class PedidoService : BaseService<Pedido>, IPedidoService
{
    public PedidoService(
        IPedidoRepository repository, 
        NegocioService negocioService) 
            : base(repository, negocioService)
    { }
}