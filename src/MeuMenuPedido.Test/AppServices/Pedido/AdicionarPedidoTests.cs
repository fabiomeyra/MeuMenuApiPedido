using MeuMenuPedido.Application.Interfaces;
using MeuMenuPedido.Domain.Interfaces.Integracoes;
using MeuMenuPedido.Domain.Interfaces.Notificador;
using MeuMenuPedido.Domain.Interfaces.Repositories;
using MeuMenuPedido.Domain.Services.Base;
using MeuMenuPedido.Domain.UoW;
using MeuMenuPedido.Domain.Validations;
using MeuMenuPedido.Domain.ValueObjects.Integracoes.ApiProdutos;
using MeuMenuPedido.Test.Fixtures;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace MeuMenuPedido.Test.AppServices.Pedido;

public class AdicionarPedidoTests : IClassFixture<AdicionarPedidoTests.Startup>, IClassFixture<PedidoTestsFixtures>
{
    private readonly Mock<IPedidoRepository> _pedidoRepositoryMock;
    private readonly Mock<IProdutoApiService> _produtoApiServiceMock;
    private readonly Mock<IUnitOfWork> _uowMock;
    private readonly IPedidoAppService _pedidoAppService;
    private readonly INotificador _notificador;
    private readonly NegocioService _negocioService;
    private readonly PedidoTestsFixtures _pedidoTestsFixtures;

    public AdicionarPedidoTests(Startup startup, PedidoTestsFixtures pedidoTestsFixtures)
    {
        _pedidoTestsFixtures = pedidoTestsFixtures;

        ServiceProvider serviceProvider = startup.ServiceProvider;
        _pedidoRepositoryMock = Mock.Get(serviceProvider.GetService<IPedidoRepository>()!);
        _produtoApiServiceMock = Mock.Get(serviceProvider.GetService<IProdutoApiService>()!);
        _uowMock = Mock.Get(serviceProvider.GetService<IUnitOfWork>()!);
        _pedidoAppService = serviceProvider.GetService<IPedidoAppService>()!;
        _notificador = serviceProvider.GetService<INotificador>()!;
        _negocioService = serviceProvider.GetService<NegocioService>()!;
    }

    [Fact(DisplayName = "Adicionar Pedido com Sucesso")]
    [Trait("Categoria", "Adicionar App Service Test")]
    public async Task Pedido_Adicionar_DeveExecutarComSucesso()
    {
        ReiniciarDependencias();

        // Arrange
        var pedidoVm = _pedidoTestsFixtures.RetornaPedidoValidoViewModel();
        _pedidoRepositoryMock.Setup(x => x.Adicionar(It.IsAny<Domain.Models.Pedido>())).ReturnsAsync(() => new Domain.Models.Pedido());
        _uowMock.Setup(x => x.Commit()).ReturnsAsync(() => 1);
        _produtoApiServiceMock.Setup(x => x.BuscarProdutoValor(It.IsAny<Guid>())).ReturnsAsync(() => new ProdutoValorValueObject
        {
            ProdutoValor = (decimal)12.99
        });

        // Act
        await _pedidoAppService.Adicionar(pedidoVm);
        await _negocioService.ExecutarValidacao();

        // Assert
        _pedidoRepositoryMock.Verify(mock => mock.Adicionar(It.IsAny<Domain.Models.Pedido>()), Times.Once);
        Assert.True(_negocioService.EhValido());
    }

    [Fact(DisplayName = "Adicionar Pedido deve adicionar ao menos um produto")]
    [Trait("Categoria", "Adicionar App Service Test")]
    public async Task Pedido_Adicionar_DeveAdicionarAoMenosUmProduto()
    {
        ReiniciarDependencias();

        // Arrange
        var pedidoVm = _pedidoTestsFixtures.RetornaPedidoInvalidoViewModel();
        _pedidoRepositoryMock.Setup(x => x.Adicionar(It.IsAny<Domain.Models.Pedido>())).ReturnsAsync(() => new Domain.Models.Pedido());
        _uowMock.Setup(x => x.Commit()).ReturnsAsync(() => 0);

        // Act
        await _pedidoAppService.Adicionar(pedidoVm);
        await _negocioService.ExecutarValidacao();

        // Assert
        Assert.False(_negocioService.EhValido());
        _pedidoRepositoryMock.Verify(mock => mock.Adicionar(It.IsAny<Domain.Models.Pedido>()), Times.Never);
        Assert.Contains(_notificador.ObterNotificacoes(),
            n => n.Mensagem.Contains(MensagensValidacaoResources.DeveInformarProdutoNoPedido));
    }

    [Fact(DisplayName = "Adicionar Pedido mesa não informada")]
    [Trait("Categoria", "Adicionar App Service Test")]
    public async Task Pedido_Adicionar_MesaNaoInformada()
    {
        ReiniciarDependencias();

        // Arrange
        var pedidoVm = _pedidoTestsFixtures.RetornaPedidoInvalidoViewModel();
        _pedidoRepositoryMock.Setup(x => x.Adicionar(It.IsAny<Domain.Models.Pedido>())).ReturnsAsync(() => new Domain.Models.Pedido());
        _uowMock.Setup(x => x.Commit()).ReturnsAsync(() => 0);

        // Act
        await _pedidoAppService.Adicionar(pedidoVm);
        await _negocioService.ExecutarValidacao();

        // Assert
        Assert.False(_negocioService.EhValido());
        _pedidoRepositoryMock.Verify(mock => mock.Adicionar(It.IsAny<Domain.Models.Pedido>()), Times.Never);
        Assert.Contains(_notificador.ObterNotificacoes(),
            n => n.Mensagem.Contains(MensagensValidacaoResources.MesaObrigatoria));
    }

    [Fact(DisplayName = "Adicionar Pedido produto sem id informado")]
    [Trait("Categoria", "Adicionar App Service Test")]
    public async Task Pedido_Adicionar_ProdutoSemIdInformado()
    {
        ReiniciarDependencias();

        // Arrange
        var pedidoVm = _pedidoTestsFixtures.RetornaPedidoComProdutoInvalidoViewModel();
        _pedidoRepositoryMock.Setup(x => x.Adicionar(It.IsAny<Domain.Models.Pedido>())).ReturnsAsync(() => new Domain.Models.Pedido());
        _uowMock.Setup(x => x.Commit()).ReturnsAsync(() => 0);

        // Act
        await _pedidoAppService.Adicionar(pedidoVm);
        await _negocioService.ExecutarValidacao();

        // Assert
        Assert.False(_negocioService.EhValido());
        _pedidoRepositoryMock.Verify(mock => mock.Adicionar(It.IsAny<Domain.Models.Pedido>()), Times.Never);
        Assert.Contains(_notificador.ObterNotificacoes(),
            n => n.Mensagem.Contains(MensagensValidacaoResources.ProdutoObrigatorio));
    }

    [Fact(DisplayName = "Adicionar Pedido produto quantidade informada")]
    [Trait("Categoria", "Adicionar App Service Test")]
    public async Task Pedido_Adicionar_ProdutoSemQuantidadeInformada()
    {
        ReiniciarDependencias();

        // Arrange
        var pedidoVm = _pedidoTestsFixtures.RetornaPedidoComProdutoInvalidoViewModel();
        _pedidoRepositoryMock.Setup(x => x.Adicionar(It.IsAny<Domain.Models.Pedido>())).ReturnsAsync(() => new Domain.Models.Pedido());
        _uowMock.Setup(x => x.Commit()).ReturnsAsync(() => 0);

        // Act
        await _pedidoAppService.Adicionar(pedidoVm);
        await _negocioService.ExecutarValidacao();

        // Assert
        Assert.False(_negocioService.EhValido());
        _pedidoRepositoryMock.Verify(mock => mock.Adicionar(It.IsAny<Domain.Models.Pedido>()), Times.Never);
        Assert.Contains(_notificador.ObterNotificacoes(),
            n => n.Mensagem.Contains(MensagensValidacaoResources.QuantidadeDoProdutoDeveSerSuperiorAZero));
    }

    [Fact(DisplayName = "Adicionar Pedido produto não identificado")]
    [Trait("Categoria", "Adicionar App Service Test")]
    public async Task Pedido_Adicionar_ProdutoNaoIdentificadoApiProdutos()
    {
        ReiniciarDependencias();

        // Arrange
        var pedidoVm = _pedidoTestsFixtures.RetornaPedidoValidoViewModel();
        _pedidoRepositoryMock.Setup(x => x.Adicionar(It.IsAny<Domain.Models.Pedido>())).ReturnsAsync(() => new Domain.Models.Pedido());
        _uowMock.Setup(x => x.Commit()).ReturnsAsync(() => 0);
        _produtoApiServiceMock.Setup(x => x.BuscarProdutoValor(It.IsAny<Guid>())).ReturnsAsync(() => null);

        // Act
        await _pedidoAppService.Adicionar(pedidoVm);
        await _negocioService.ExecutarValidacao();

        // Assert
        Assert.False(_negocioService.EhValido());
        _pedidoRepositoryMock.Verify(mock => mock.Adicionar(It.IsAny<Domain.Models.Pedido>()), Times.Never);
        Assert.Contains(_notificador.ObterNotificacoes(),
            n => n.Mensagem.Contains(MensagensValidacaoResources.ProdutoNaoLocalizado));
    }

    private void ReiniciarDependencias()
    {
        _pedidoRepositoryMock.Reset();
        _produtoApiServiceMock.Reset();
        _uowMock.Reset();
        _negocioService.LimparNotificacoes();
        _negocioService.LimparValidacoes();
    }

    public class Startup : BaseStartup
    {
        public override IServiceCollection OnConfigureServices(IServiceCollection services)
        {
            return services
                .AddScoped(_ => Mock.Of<IPedidoRepository>())
                .AddScoped(_ => Mock.Of<IUnitOfWork>())
                .AddScoped(_ => Mock.Of<IProdutoApiService>());
        }
    }
}