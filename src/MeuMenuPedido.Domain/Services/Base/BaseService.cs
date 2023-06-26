using System.Linq.Expressions;
using MeuMenuPedido.Domain.Interfaces.Repositories.Base;
using MeuMenuPedido.Domain.Interfaces.Services.Base;

namespace MeuMenuPedido.Domain.Services.Base;

public abstract class BaseService<T> : IBaseService<T> where T : class
{
    private readonly IBaseRepository<T> _repository;
    protected readonly NegocioService NegocioService;

    protected BaseService(IBaseRepository<T> repository, NegocioService negocioService)
    {
        _repository = repository;
        NegocioService = negocioService;
    }

    public virtual Task<T?> Obter(Expression<Func<T, bool>> where, bool asNoTracking = false) => _repository.Obter(where, asNoTracking);

    public virtual Task<TResult?> Obter<TResult>(Expression<Func<T, bool>> where, Expression<Func<T, TResult>> projecao,
        bool asNoTracking = false) => _repository.Obter(where, projecao, asNoTracking);

    public virtual Task<ICollection<T>> Buscar(Expression<Func<T, bool>> where, bool asNoTracking = false) => _repository.Buscar(where, asNoTracking);

    public virtual Task<ICollection<TResult>> Buscar<TResult>(Expression<Func<T, bool>> where,
        Expression<Func<T, TResult>> projecao, bool asNoTracking = false) => _repository.Buscar(where, projecao, asNoTracking);

    public virtual async Task<T> Adicionar(T objeto)
    {
        return await _repository.Adicionar(objeto);
    }
    
    public virtual async Task<T> Atualizar(T objeto)
    {
        return await _repository.Atualizar(objeto);
    }

    public virtual async Task Excluir(T objeto)
    {
        await _repository.Excluir(objeto);
    }
}