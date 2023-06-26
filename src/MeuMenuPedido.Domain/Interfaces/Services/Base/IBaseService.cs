using System.Linq.Expressions;

namespace MeuMenuPedido.Domain.Interfaces.Services.Base;

public interface IBaseService<T> where T : class
{
    Task<T?> Obter(Expression<Func<T, bool>> where, bool asNoTracking = false);
    Task<TResult?> Obter<TResult>(Expression<Func<T, bool>> where,
        Expression<Func<T, TResult>> projecao, bool asNoTracking = false);
    Task<ICollection<T>> Buscar(Expression<Func<T, bool>> where, bool asNoTracking = false);
    Task<ICollection<TResult>> Buscar<TResult>(Expression<Func<T, bool>> where,
        Expression<Func<T, TResult>> projecao, bool asNoTracking = false);
    Task<T> Adicionar(T obj);
    Task<T> Atualizar(T obj);
    Task Excluir(T obj);
}