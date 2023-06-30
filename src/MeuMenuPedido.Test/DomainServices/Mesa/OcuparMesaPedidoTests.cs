using System.Linq.Expressions;
using MeuMenuPedido.Domain.Interfaces.Notificador;
using MeuMenuPedido.Domain.Interfaces.Repositories;
using MeuMenuPedido.Domain.Interfaces.Services;
using MeuMenuPedido.Domain.Models;
using MeuMenuPedido.Domain.Services.Base;
using MeuMenuPedido.Domain.Validations;
using MeuMenuPedido.Test.Fixtures;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace MeuMenuPedido.Test.DomainServices.Mesa;

public class OcuparMesaPedidoTests : IClassFixture<OcuparMesaPedidoTests.Startup>, IClassFixture<MesaPedidoTestsFixtures>
{
    private readonly Mock<IMesaPedidoRepository> _mesaPedidoRepositoryMock;
    private readonly IMesaPedidoService _mesaPedidoService;
    private readonly INotificador _notificador;
    private readonly NegocioService _negocioService;
    private readonly MesaPedidoTestsFixtures _mesaPedidoTestsFixtures;

    public OcuparMesaPedidoTests(Startup startup, MesaPedidoTestsFixtures mesaPedidoTestsFixtures)
    {
        _mesaPedidoTestsFixtures = mesaPedidoTestsFixtures;

        ServiceProvider serviceProvider = startup.ServiceProvider;
        _mesaPedidoRepositoryMock = Mock.Get(serviceProvider.GetService<IMesaPedidoRepository>()!);
        _mesaPedidoService = serviceProvider.GetService<IMesaPedidoService>()!;
        _notificador = serviceProvider.GetService<INotificador>()!;
        _negocioService = serviceProvider.GetService<NegocioService>()!;
    }

    [Fact(DisplayName = "Ocupar mesa Pedido com Sucesso")]
    [Trait("Categoria", "Ocupar Mesa Pedido Service Test")]
    public async Task MesaPedido_Ocupar_DeveExecutarComSucesso()
    {
        ReiniciarDependencias();

        // Arrange
        var mesaPedido = _mesaPedidoTestsFixtures.RetornaMesaPedidoValido();
        _mesaPedidoRepositoryMock.Setup(x => x.Obter(It.IsAny<Expression<Func<MesaPedido, bool>>>(), y => true, true)).ReturnsAsync(() => false);
        _mesaPedidoRepositoryMock.Setup(x => x.Adicionar(mesaPedido)).ReturnsAsync(() => mesaPedido);

        // Act
        await _mesaPedidoService.Adicionar(mesaPedido);
        await _negocioService.ExecutarValidacao();

        // Assert
        _mesaPedidoRepositoryMock.Verify(mock => mock.Adicionar(mesaPedido), Times.Once);
        Assert.True(_negocioService.EhValido());
    }

    [Fact(DisplayName = "Ocupar mesa Pedido Mesa ocupada")]
    [Trait("Categoria", "Ocupar Mesa Pedido Service Test")]
    public async Task MesaPedido_Ocupar_MesaOcupada()
    {
        ReiniciarDependencias();

        // Arrange
        var mesaPedido = _mesaPedidoTestsFixtures.RetornaMesaPedidoValido();
        _mesaPedidoRepositoryMock.Setup(x => x.Obter(It.IsAny<Expression<Func<MesaPedido, bool>>>(), y => true, true)).ReturnsAsync(() => true);
        _mesaPedidoRepositoryMock.Setup(x => x.Adicionar(mesaPedido)).ReturnsAsync(() => mesaPedido);

        // Act
        await _mesaPedidoService.Adicionar(mesaPedido);
        await _negocioService.ExecutarValidacao();

        // Assert
        _mesaPedidoRepositoryMock.Verify(mock => mock.Adicionar(mesaPedido), Times.Once);
        Assert.False(_negocioService.EhValido());
        Assert.Contains(_notificador.ObterNotificacoes(),
            n => n.Mensagem.Contains(MensagensValidacaoResources.MesaOcupada));
    }

    [Fact(DisplayName = "Ocupar mesa Pedido Mesa não informada")]
    [Trait("Categoria", "Ocupar Mesa Pedido Service Test")]
    public async Task MesaPedido_Ocupar_MesaNaoInformada()
    {
        ReiniciarDependencias();

        // Arrange
        var mesaPedido = new MesaPedido();
        _mesaPedidoRepositoryMock.Setup(x => x.Adicionar(mesaPedido)).ReturnsAsync(() => mesaPedido);

        // Act
        await _mesaPedidoService.Adicionar(mesaPedido);
        await _negocioService.ExecutarValidacao();

        // Assert
        _mesaPedidoRepositoryMock.Verify(mock => mock.Adicionar(mesaPedido), Times.Once);
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
                .AddScoped(_ => Mock.Of<IMesaPedidoRepository>());
        }
    }
}