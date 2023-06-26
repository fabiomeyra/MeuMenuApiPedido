using System.Linq.Expressions;

namespace MeuMenuPedido.Domain.Interfaces.Repositories.Base;

public interface IBaseRepository<T> : IDisposable where T : class
{
    Task<ICollection<T>> Buscar(Expression<Func<T, bool>> where, bool asNoTracking = false);

    Task<ICollection<TResult>> Buscar<TResult>(Expression<Func<T, bool>> where, Expression<Func<T, TResult>> projecao,
        bool asNoTracking = false);
    Task<T?> Obter(Expression<Func<T, bool>> where, bool asNoTracking = false);

    Task<TResult?> Obter<TResult>(Expression<Func<T, bool>> where, Expression<Func<T, TResult>> projecao,
        bool asNoTracking = false);
    Task<T> Adicionar(T objeto);
    Task<T> Atualizar(T objeto);
    Task Excluir(T objeto);
}