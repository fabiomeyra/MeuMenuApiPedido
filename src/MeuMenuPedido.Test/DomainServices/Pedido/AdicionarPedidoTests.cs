using MeuMenuPedido.Domain.Interfaces.Integracoes;
using MeuMenuPedido.Domain.Interfaces.Notificador;
using MeuMenuPedido.Domain.Interfaces.Repositories;
using MeuMenuPedido.Domain.Interfaces.Services;
using MeuMenuPedido.Domain.Services.Base;
using MeuMenuPedido.Domain.Validations;
using MeuMenuPedido.Domain.ValueObjects.Integracoes.ApiProdutos;
using MeuMenuPedido.Test.Fixtures;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace MeuMenuPedido.Test.DomainServices.Pedido;

public class AdicionarPedidoTests : IClassFixture<AdicionarPedidoTests.Startup>, IClassFixture<PedidoTestsFixtures>
{
    private readonly Mock<IPedidoRepository> _pedidoRepositoryMock;
    private readonly Mock<IProdutoApiService> _produtoApiServiceMock;
    private readonly IPedidoService _pedidoService;
    private readonly INotificador _notificador;
    private readonly NegocioService _negocioService;
    private readonly PedidoTestsFixtures _pedidoTestsFixtures;

    public AdicionarPedidoTests(Startup startup, PedidoTestsFixtures pedidoTestsFixtures)
    {
        _pedidoTestsFixtures = pedidoTestsFixtures;

        ServiceProvider serviceProvider = startup.ServiceProvider;
        _pedidoRepositoryMock = Mock.Get(serviceProvider.GetService<IPedidoRepository>()!);
        _produtoApiServiceMock = Mock.Get(serviceProvider.GetService<IProdutoApiService>()!);
        _pedidoService = serviceProvider.GetService<IPedidoService>()!;
        _notificador = serviceProvider.GetService<INotificador>()!;
        _negocioService = serviceProvider.GetService<NegocioService>()!;
    }

    [Fact(DisplayName = "Adicionar Pedido com Sucesso")]
    [Trait("Categoria", "Adicionar Service Test")]
    public async Task Pedido_Adicionar_DeveExecutarComSucesso()
    {
        ReiniciarDependencias();

        // Arrange
        var pedido = _pedidoTestsFixtures.RetornaPedidoValido();
        _pedidoRepositoryMock.Setup(x => x.Adicionar(pedido)).ReturnsAsync(() => pedido);
        _produtoApiServiceMock.Setup(x => x.BuscarProdutoValor(It.IsAny<Guid>())).ReturnsAsync(() => new ProdutoValorValueObject
        {
            ProdutoValor = (decimal)12.99
        });

        // Act
        await _pedidoService.Adicionar(pedido);
        await _negocioService.ExecutarValidacao();

        // Assert
        _pedidoRepositoryMock.Verify(mock => mock.Adicionar(pedido), Times.Once);
        Assert.True(_negocioService.EhValido());
    }

    [Fact(DisplayName = "Adicionar Pedido deve adicionar ao menos um produto")]
    [Trait("Categoria", "Adicionar Service Test")]
    public async Task Pedido_Adicionar_DeveAdicionarAoMenosUmProduto()
    {
        ReiniciarDependencias();

        // Arrange
        var pedido = _pedidoTestsFixtures.RetornaPedidoInvalido();
        _pedidoRepositoryMock.Setup(x => x.Adicionar(pedido)).ReturnsAsync(() => pedido);

        // Act
        await _pedidoService.Adicionar(pedido);
        await _negocioService.ExecutarValidacao();

        // Assert
        Assert.False(_negocioService.EhValido());
        _pedidoRepositoryMock.Verify(mock => mock.Adicionar(pedido), Times.Never);
        Assert.Contains(_notificador.ObterNotificacoes(),
            n => n.Mensagem.Contains(MensagensValidacaoResources.DeveInformarProdutoNoPedido));
    }

    [Fact(DisplayName = "Adicionar Pedido mesa não informada")]
    [Trait("Categoria", "Adicionar Service Test")]
    public async Task Pedido_Adicionar_MesaNaoInformada()
    {
        ReiniciarDependencias();

        // Arrange
        var pedido = _pedidoTestsFixtures.RetornaPedidoInvalido();
        _pedidoRepositoryMock.Setup(x => x.Adicionar(pedido)).ReturnsAsync(() => pedido);

        // Act
        await _pedidoService.Adicionar(pedido);
        await _negocioService.ExecutarValidacao();

        // Assert
        Assert.False(_negocioService.EhValido());
        _pedidoRepositoryMock.Verify(mock => mock.Adicionar(pedido), Times.Never);
        Assert.Contains(_notificador.ObterNotificacoes(),
            n => n.Mensagem.Contains(MensagensValidacaoResources.MesaObrigatoria));
    }

    [Fact(DisplayName = "Adicionar Pedido produto sem id informado")]
    [Trait("Categoria", "Adicionar Service Test")]
    public async Task Pedido_Adicionar_ProdutoSemIdInformado()
    {
        ReiniciarDependencias();

        // Arrange
        var pedido = _pedidoTestsFixtures.RetornaPedidoComProdutoInvalido();
        _pedidoRepositoryMock.Setup(x => x.Adicionar(pedido)).ReturnsAsync(() => pedido);

        // Act
        await _pedidoService.Adicionar(pedido);
        await _negocioService.ExecutarValidacao();

        // Assert
        Assert.False(_negocioService.EhValido());
        _pedidoRepositoryMock.Verify(mock => mock.Adicionar(pedido), Times.Never);
        Assert.Contains(_notificador.ObterNotificacoes(),
            n => n.Mensagem.Contains(MensagensValidacaoResources.ProdutoObrigatorio));
    }

    [Fact(DisplayName = "Adicionar Pedido produto quantidade informada")]
    [Trait("Categoria", "Adicionar Service Test")]
    public async Task Pedido_Adicionar_ProdutoSemQuantidadeInformada()
    {
        ReiniciarDependencias();

        // Arrange
        var pedido = _pedidoTestsFixtures.RetornaPedidoComProdutoInvalido();
        _pedidoRepositoryMock.Setup(x => x.Adicionar(pedido)).ReturnsAsync(() => pedido);

        // Act
        await _pedidoService.Adicionar(pedido);
        await _negocioService.ExecutarValidacao();

        // Assert
        Assert.False(_negocioService.EhValido());
        _pedidoRepositoryMock.Verify(mock => mock.Adicionar(pedido), Times.Never);
        Assert.Contains(_notificador.ObterNotificacoes(),
            n => n.Mensagem.Contains(MensagensValidacaoResources.QuantidadeDoProdutoDeveSerSuperiorAZero));
    }

    [Fact(DisplayName = "Adicionar Pedido produto não identificado")]
    [Trait("Categoria", "Adicionar Service Test")]
    public async Task Pedido_Adicionar_ProdutoNaoIdentificadoApiProdutos()
    {
        ReiniciarDependencias();

        // Arrange
        var pedido = _pedidoTestsFixtures.RetornaPedidoValido();
        _pedidoRepositoryMock.Setup(x => x.Adicionar(pedido)).ReturnsAsync(() => pedido);
        _produtoApiServiceMock.Setup(x => x.BuscarProdutoValor(It.IsAny<Guid>())).ReturnsAsync(() => null);

        // Act
        await _pedidoService.Adicionar(pedido);
        await _negocioService.ExecutarValidacao();

        // Assert
        Assert.False(_negocioService.EhValido());
        _pedidoRepositoryMock.Verify(mock => mock.Adicionar(pedido), Times.Never);
        Assert.Contains(_notificador.ObterNotificacoes(),
            n => n.Mensagem.Contains(MensagensValidacaoResources.ProdutoNaoLocalizado));
    }

    private void ReiniciarDependencias()
    {
        _pedidoRepositoryMock.Reset();
        _produtoApiServiceMock.Reset();
        _negocioService.LimparNotificacoes();
        _negocioService.LimparValidacoes();
    }

    public class Startup : BaseStartup
    {
        public override IServiceCollection OnConfigureServices(IServiceCollection services)
        {
            return services
                .AddScoped(_ => Mock.Of<IPedidoRepository>())
                .AddScoped(_ => Mock.Of<IProdutoApiService>());
        }
    }
}