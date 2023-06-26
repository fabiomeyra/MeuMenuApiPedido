namespace MeuMenuPedido.Domain.UoW;

public interface IUnitOfWork
{
    Task<int> Commit();
}