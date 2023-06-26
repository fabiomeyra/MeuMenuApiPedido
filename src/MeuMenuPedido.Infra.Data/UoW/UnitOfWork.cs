using MeuMenuPedido.Domain.UoW;
using MeuMenuPedido.Infra.Data.Context;

namespace MeuMenuPedido.Infra.Data.UoW;

public class UnitOfWork : IUnitOfWork
{
    private readonly MeuMenuPedidoContext _context;

    public UnitOfWork(MeuMenuPedidoContext context)
    {
        _context = context;
    }

    public async Task<int> Commit()
    {
        return await _context.SaveChangesAsync();
    }
}