using MeuMenuPedido.Domain.Enum;
using MeuMenuPedido.Domain.Filtros.Pedido;
using MeuMenuPedido.Domain.Interfaces.Repositories;
using MeuMenuPedido.Domain.Models;
using MeuMenuPedido.Infra.Data.Context;
using MeuMenuPedido.Infra.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace MeuMenuPedido.Infra.Data.Repositories;

public class PedidoRepository : BaseRepository<Pedido>, IPedidoRepository
{
    public PedidoRepository(MeuMenuPedidoContext contexto) 
        : base(contexto)
    {
    }

    public async Task<Pedido?> BuscarPorId(Guid id)
    {
        var retorno = await Entidades
            .Include(x => x.ProdutosDoPedido)
            .FirstOrDefaultAsync(x => x.PedidoId == id);

        return retorno;
    }

    public async Task<ICollection<Pedido>> BuscarPorSituacao(BuscarPedidoPorSituacaoFiltro filtro)
    {
        var retorno = Entidades
            .Where(x => x.DataCadastro.Date >= DateTime.Now.AddDays(-1).Date)
            .Where(x => x.SituacaoPedidoId == filtro.SituacaoPedido)
            .Include(x => x.ProdutosDoPedido)
            .Skip(filtro.Pagina - 1)
            .Take(filtro.QuantidadePorPagina);

        var ordenado = filtro.OrdenarPorMaisRecentes
            ? retorno.OrderByDescending(x => x.DataCadastro)
            : retorno.OrderBy(x => x.DataCadastro);

        return await ordenado.ToListAsync();
    }
}