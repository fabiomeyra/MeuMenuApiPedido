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

public class DesocuparMesaPedidoTests : IClassFixture<DesocuparMesaPedidoTests.Startup>, IClassFixture<MesaPedidoTestsFixtures>
{
    private readonly Mock<IMesaPedidoRepository> _mesaPedidoRepositoryMock;
    private readonly IMesaPedidoAppService _mesaPedidoAppService;
    private readonly INotificador _notificador;
    private readonly Mock<IUnitOfWork> _uowMock;
    private readonly NegocioService _negocioService;
    private readonly MesaPedidoTestsFixtures _mesaPedidoTestsFixtures;

    public DesocuparMesaPedidoTests(Startup startup, MesaPedidoTestsFixtures mesaPedidoTestsFixtures)
    {
        _mesaPedidoTestsFixtures = mesaPedidoTestsFixtures;

        ServiceProvider serviceProvider = startup.ServiceProvider;
        _mesaPedidoRepositoryMock = Mock.Get(serviceProvider.GetService<IMesaPedidoRepository>()!);
        _uowMock = Mock.Get(serviceProvider.GetService<IUnitOfWork>()!);
        _mesaPedidoAppService = serviceProvider.GetService<IMesaPedidoAppService>()!;
        _notificador = serviceProvider.GetService<INotificador>()!;
        _negocioService = serviceProvider.GetService<NegocioService>()!;
    }

    [Fact(DisplayName = "Desocupar mesa Pedido com Sucesso")]
    [Trait("Categoria", "Desocupar Mesa Pedido App Service Test")]
    public async Task MesaPedido_Desocupar_DeveExecutarComSucesso()
    {
        ReiniciarDependencias();

        // Arrange
        _mesaPedidoRepositoryMock.Setup(x => x.Obter(It.IsAny<Expression<Func<MesaPedido, bool>>>(), y => true, true)).ReturnsAsync(() => true);
        _uowMock.Setup(x => x.Commit()).ReturnsAsync(() => 1);

        // Act
        await _mesaPedidoAppService.DesocuparMesa(1);
        await _negocioService.ExecutarValidacao();

        // Assert
        _mesaPedidoRepositoryMock.Verify(mock => mock.Excluir(It.IsAny<MesaPedido>()), Times.Once);
        _uowMock.Verify(mock => mock.Commit(), Times.Once);
        Assert.True(_negocioService.EhValido());
    }

    [Fact(DisplayName = "Desocupar mesa Pedido Mesa não ocupada")]
    [Trait("Categoria", "Desocupar Mesa Pedido App Service Test")]
    public async Task MesaPedido_Desocupar_MesaNaoOcupada()
    {
        ReiniciarDependencias();

        // Arrange
        _mesaPedidoRepositoryMock.Setup(x => x.Obter(It.IsAny<Expression<Func<MesaPedido, bool>>>(), y => true, true)).ReturnsAsync(() => false);
        _uowMock.Setup(x => x.Commit()).ReturnsAsync(() => 0);

        // Act
        await _mesaPedidoAppService.DesocuparMesa(1);
        await _negocioService.ExecutarValidacao();

        // Assert
        Assert.False(_negocioService.EhValido());
        _mesaPedidoRepositoryMock.Verify(mock => mock.Excluir(It.IsAny<MesaPedido>()), Times.Once);
        _uowMock.Verify(mock => mock.Commit(), Times.Never);
        Assert.Contains(_notificador.ObterNotificacoes(),
            n => n.Mensagem.Contains(MensagensValidacaoResources.MesaNaoIdentificada));
    }

    [Fact(DisplayName = "Desocupar mesa Pedido Mesa não informada")]
    [Trait("Categoria", "Desocupar Mesa Pedido App Service Test")]
    public async Task MesaPedido_Desocupar_MesaNaoInformada()
    {
        ReiniciarDependencias();

        // Act
        await _mesaPedidoAppService.DesocuparMesa(0);
        await _negocioService.ExecutarValidacao();

        // Assert
        _mesaPedidoRepositoryMock.Verify(mock => mock.Excluir(It.IsAny<MesaPedido>()), Times.Once);
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