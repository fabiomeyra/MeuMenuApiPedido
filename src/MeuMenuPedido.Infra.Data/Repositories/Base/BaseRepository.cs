using System.Linq.Expressions;
using MeuMenuPedido.Domain.Interfaces.Repositories.Base;
using MeuMenuPedido.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace MeuMenuPedido.Infra.Data.Repositories.Base;

public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
{
    protected MeuMenuPedidoContext Contexto;
    protected DbSet<T> Entidades;

    protected BaseRepository(MeuMenuPedidoContext contexto)
    {
        Contexto = contexto;
        Entidades = contexto.Set<T>();
    }

    public virtual async Task<T> Adicionar(T objeto)
    {
        EntityEntry<T> objRetorno = await Entidades.AddAsync(objeto);
        return objRetorno.Entity;
    }

    public virtual async Task<T> Atualizar(T objeto)
    {
        EntityEntry<T> objetoRetorno = await Task.Run(() => Entidades.Update(objeto));
        return objetoRetorno.Entity;
    }

    public async Task<ICollection<T>> Buscar(Expression<Func<T, bool>> where, bool asNoTracking = false)
    {
        if (asNoTracking) return await Entidades.AsNoTracking().Where(where).ToListAsync();
        return await Entidades.Where(where).ToListAsync();
    }

    public async Task<ICollection<TResult>> Buscar<TResult>(Expression<Func<T, bool>> where, Expression<Func<T, TResult>> projecao, bool asNoTracking = false)
    {
        if (asNoTracking) return await Entidades.AsNoTracking().Where(where).Select(projecao).ToListAsync();
        return await Entidades.Where(where).Select(projecao).ToListAsync();
    }

    public async Task Excluir(T objeto)
    {
        await Task.Run(() => Entidades.Remove(objeto));
    }

    public virtual async Task<T?> Obter(Expression<Func<T, bool>> where, bool asNoTracking = false)
    {
        if (asNoTracking) return await Entidades.AsNoTracking().Where(where).FirstOrDefaultAsync();
        return await Entidades.Where(where).FirstOrDefaultAsync();
    }
    
    public virtual async Task<TResult?> Obter<TResult>(Expression<Func<T, bool>> where, Expression<Func<T, TResult>> projecao, bool asNoTracking = false)
    {
        if (asNoTracking) return await Entidades.AsNoTracking().Where(where).Select(projecao).FirstOrDefaultAsync();
        return await Entidades.Where(where).Select(projecao).FirstOrDefaultAsync();
    }

    public void Dispose()
    {
        Contexto.Dispose();
    }
}