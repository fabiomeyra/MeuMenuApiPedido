using MeuMenuPedido.Domain.Enum;
using MeuMenuPedido.Domain.Filtros.Pedido;
using MeuMenuPedido.Domain.Interfaces.Integracoes;
using MeuMenuPedido.Domain.Interfaces.Notificador;
using MeuMenuPedido.Domain.Interfaces.Repositories;
using MeuMenuPedido.Domain.Interfaces.Services;
using MeuMenuPedido.Domain.Models;
using MeuMenuPedido.Domain.Notificador;
using MeuMenuPedido.Domain.Services.Base;
using MeuMenuPedido.Domain.Services.Utils;
using MeuMenuPedido.Domain.Utils;
using MeuMenuPedido.Domain.Validations;
using MeuMenuPedido.Domain.Validations.Pedido;

namespace MeuMenuPedido.Domain.Services;

public class PedidoService : BaseService<Pedido>, IPedidoService
{
    private readonly IProdutoApiService _produtoApiService;
    private readonly INotificador _notificador;
    private readonly IPedidoRepository _repository;
    private readonly IUsuarioLogadoService _usuarioLogadoService;

    public PedidoService(
        IPedidoRepository repository, 
        NegocioService negocioService, 
        IProdutoApiService produtoApiService, 
        INotificador notificador, 
        IUsuarioLogadoService usuarioLogadoService
    ) 
        : base(repository, negocioService)
    {
        _repository = repository;
        _produtoApiService = produtoApiService;
        _notificador = notificador;
        _usuarioLogadoService = usuarioLogadoService;
    }

    public Task<Pedido?> BuscarPorId(Guid id) => _repository.BuscarPorId(id);

    public override async Task<Pedido> Adicionar(Pedido objeto)
    {
        objeto.AdicionarValidacaoEntidade(NegocioService, new AdicionarPedidoValidation());
        await NegocioService.ExecutarValidacao();
        if (!NegocioService.EhValido()) return objeto;

        objeto.GerarNovoPedido();

        foreach (var produto in objeto.ProdutosDoPedido)
        {
            var produtoValor = await _produtoApiService.BuscarProdutoValor(produto.ProdutoId);
            if (produtoValor is null)
            {
                _notificador.AdicionarNotificacao(new Notificacao(MensagensValidacaoResources.ProdutoNaoLocalizado));
                return objeto;
            }

            produto.ProdutoValor = produtoValor.ProdutoValor;
            produto.PedidoId = objeto.PedidoId;
        }

        return await base.Adicionar(objeto);
    }

    public Task<ICollection<Pedido>> BuscarPorSituacao(BuscarPedidoPorSituacaoFiltro filtro) =>
        _repository.BuscarPorSituacao(filtro);

    public async Task AdicionarImagemDescricaoPedidos(ICollection<PedidoProduto> produtosDoPedido)
    {
        var idsParaBusca = produtosDoPedido.Select(x => x.ProdutoId).ToList();
        var produtos = await _produtoApiService.BuscarImagemEDescricaoProdutos(idsParaBusca);
        if(produtos is null || !produtos.Any()) return;

        foreach (var x in produtos)
        {
            var produto = produtosDoPedido.First(y => y.ProdutoId == x.ProdutoId);
            produto.Produto = x;
        }
    }

    public async Task<Pedido> AlterarSituacaoPedido(Guid pedidoId, SituacaoPedidoEnum situacao)
    {
        var pedido = await Obter(x => x.PedidoId == pedidoId);
        
        if (pedido is null)
        {
            _notificador.AdicionarNotificacao(new Notificacao(MensagensValidacaoResources.PedidoNaoEncontrado));
            return null!;
        }

        var situacaoAnterior = pedido.SituacaoPedidoId;
        pedido.SituacaoPedidoId = situacao;
        pedido.AdicionarValidacaoEntidade(NegocioService, new AlterarSituacaoPedidoValidation(situacaoAnterior, _usuarioLogadoService));
        await _repository.Atualizar(pedido);
        return pedido;
    }
}