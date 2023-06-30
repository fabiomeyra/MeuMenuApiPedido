using MeuMenuPedido.Application.Interfaces;
using MeuMenuPedido.Domain.Interfaces.Notificador;
using MeuMenuPedido.Domain.Interfaces.Repositories;
using MeuMenuPedido.Domain.Models;
using MeuMenuPedido.Domain.Services.Base;
using MeuMenuPedido.Domain.UoW;
using MeuMenuPedido.Domain.Validations;
using MeuMenuPedido.Test.Fixtures;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Linq.Expressions;
using Xunit;

namespace MeuMenuPedido.Test.AppServices.Mesa;

public class OcuparMesaPedidoTests : IClassFixture<OcuparMesaPedidoTests.Startup>, IClassFixture<MesaPedidoTestsFixtures>
{
    private readonly Mock<IMesaPedidoRepository> _mesaPedidoRepositoryMock;
    private readonly IMesaPedidoAppService _mesaPedidoAppService;
    private readonly INotificador _notificador;
    private readonly Mock<IUnitOfWork> _uowMock;
    private readonly NegocioService _negocioService;
    private readonly MesaPedidoTestsFixtures _mesaPedidoTestsFixtures;

    public OcuparMesaPedidoTests(Startup startup, MesaPedidoTestsFixtures mesaPedidoTestsFixtures)
    {
        _mesaPedidoTestsFixtures = mesaPedidoTestsFixtures;

        ServiceProvider serviceProvider = startup.ServiceProvider;
        _mesaPedidoRepositoryMock = Mock.Get(serviceProvider.GetService<IMesaPedidoRepository>()!);
        _uowMock = Mock.Get(serviceProvider.GetService<IUnitOfWork>()!);
        _mesaPedidoAppService = serviceProvider.GetService<IMesaPedidoAppService>()!;
        _notificador = serviceProvider.GetService<INotificador>()!;
        _negocioService = serviceProvider.GetService<NegocioService>()!;
    }

    [Fact(DisplayName = "Ocupar mesa Pedido com Sucesso")]
    [Trait("Categoria", "Ocupar Mesa Pedido App Service Test")]
    public async Task MesaPedido_Ocupar_DeveExecutarComSucesso()
    {
        ReiniciarDependencias();

        // Arrange
        var mesaPedido = _mesaPedidoTestsFixtures.RetornaMesaPedidoValido();
        _mesaPedidoRepositoryMock.Setup(x => x.Obter(It.IsAny<Expression<Func<MesaPedido, bool>>>(), y => true, true)).ReturnsAsync(() => false);
        _mesaPedidoRepositoryMock.Setup(x => x.Adicionar(mesaPedido)).ReturnsAsync(() => mesaPedido);
        _uowMock.Setup(x => x.Commit()).ReturnsAsync(() => 1);

        // Act
        await _mesaPedidoAppService.OcuparMesa(1);
        await _negocioService.ExecutarValidacao();

        // Assert
        _mesaPedidoRepositoryMock.Verify(mock => mock.Adicionar(It.IsAny<MesaPedido>()), Times.Once);
        _uowMock.Verify(mock => mock.Commit(), Times.Once);
        Assert.True(_negocioService.EhValido());
    }

    [Fact(DisplayName = "Ocupar mesa Pedido Mesa ocupada")]
    [Trait("Categoria", "Ocupar Mesa Pedido App Service Test")]
    public async Task MesaPedido_Ocupar_MesaOcupada()
    {
        ReiniciarDependencias();

        // Arrange
        var mesaPedido = _mesaPedidoTestsFixtures.RetornaMesaPedidoValido();
        _mesaPedidoRepositoryMock.Setup(x => x.Obter(It.IsAny<Expression<Func<MesaPedido, bool>>>(), y => true, true)).ReturnsAsync(() => true);
        _mesaPedidoRepositoryMock.Setup(x => x.Adicionar(mesaPedido)).ReturnsAsync(() => mesaPedido);
        _uowMock.Setup(x => x.Commit()).ReturnsAsync(() => 1);

        // Act
        await _mesaPedidoAppService.OcuparMesa(1);
        await _negocioService.ExecutarValidacao();

        // Assert
        Assert.False(_negocioService.EhValido());
        _mesaPedidoRepositoryMock.Verify(mock => mock.Adicionar(It.IsAny<MesaPedido>()), Times.Once);
        _uowMock.Verify(mock => mock.Commit(), Times.Never);
        Assert.Contains(_notificador.ObterNotificacoes(),
            n => n.Mensagem.Contains(MensagensValidacaoResources.MesaOcupada));
    }

    [Fact(DisplayName = "Ocupar mesa Pedido Mesa não informada")]
    [Trait("Categoria", "Ocupar Mesa Pedido App Service Test")]
    public async Task MesaPedido_Ocupar_MesaNaoInformada()
    {
        ReiniciarDependencias();

        // Act
        await _mesaPedidoAppService.OcuparMesa(0);
        await _negocioService.ExecutarValidacao();

        // Assert
        _mesaPedidoRepositoryMock.Verify(mock => mock.Adicionar(It.IsAny<MesaPedido>()), Times.Once);
        _uowMock.Verify(mock => mock.Commit(), Times.Never);
        Assert.False(_negocioService.EhValido());
        Assert.Contains(_notificador.ObterNotificacoes(),
            n => n.Mensagem.Contains(MensagensValidacaoResources.MesaObrigatoria));
    }

    private void ReiniciarDependencias()
    {
        _mesaPedidoRepositoryMock.Reset();
        _negocioService.LimparNotificacoes();
        _negocioService.LimparValidacoes();
    }

    public class Startup : BaseStartup
    {
        public override IServiceCollection OnConfigureServices(IServiceCollection services)
        {
            return services
                .AddScoped(_ => Mock.Of<IUnitOfWork>())
                .AddScoped(_ => Mock.Of<IMesaPedidoRepository>());
        }
    }
}