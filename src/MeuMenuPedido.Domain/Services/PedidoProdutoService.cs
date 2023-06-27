using MeuMenuPedido.Domain.Interfaces.Repositories;
using MeuMenuPedido.Domain.Interfaces.Services;
using MeuMenuPedido.Domain.Models;
using MeuMenuPedido.Domain.Services.Base;

namespace MeuMenuPedido.Domain.Services;

public class PedidoProdutoService : BaseService<PedidoProduto>, IPedidoProdutoService
{
    public PedidoProdutoService(
        IPedidoProdutoRepository repository, 
        NegocioService negocioService) 
            : base(repository, negocioService)
    { }
}