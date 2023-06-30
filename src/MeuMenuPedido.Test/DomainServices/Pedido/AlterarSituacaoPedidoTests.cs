using MeuMenuPedido.Domain.Enum;
using MeuMenuPedido.Domain.Interfaces.Integracoes;
using MeuMenuPedido.Domain.Interfaces.Notificador;
using MeuMenuPedido.Domain.Interfaces.Repositories;
using MeuMenuPedido.Domain.Interfaces.Services;
using MeuMenuPedido.Domain.Services.Base;
using MeuMenuPedido.Domain.Services.Utils;
using MeuMenuPedido.Domain.Validations;
using MeuMenuPedido.Domain.ValueObjects;
using MeuMenuPedido.Test.Fixtures;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Linq.Expressions;
using Xunit;

namespace MeuMenuPedido.Test.DomainServices.Pedido;

public class AlterarSituacaoTests : IClassFixture<AlterarSituacaoTests.Startup>, IClassFixture<PedidoTestsFixtures>
{
    private readonly Mock<IPedidoRepository> _usuarioRepositoryMock;
    private readonly Mock<IProdutoApiService> _produtoApiServiceMock;
    private readonly Mock<IUsuarioLogadoService> _usuarioLogadoMock;
    private readonly IPedidoService _pedidoService;
    private readonly INotificador _notificador;
    private readonly NegocioService _negocioService;
    private readonly PedidoTestsFixtures _pedidoTestsFixtures;

    public AlterarSituacaoTests(Startup startup, PedidoTestsFixtures pedidoTestsFixtures)
    {
        _pedidoTestsFixtures = pedidoTestsFixtures;

        ServiceProvider serviceProvider = startup.ServiceProvider;
        _usuarioRepositoryMock = Mock.Get(serviceProvider.GetService<IPedidoRepository>()!);
        _produtoApiServiceMock = Mock.Get(serviceProvider.GetService<IProdutoApiService>()!);
        _usuarioLogadoMock = Mock.Get(serviceProvider.GetService<IUsuarioLogadoService>()!);
        _pedidoService = serviceProvider.GetService<IPedidoService>()!;
        _notificador = serviceProvider.GetService<INotificador>()!;
        _negocioService = serviceProvider.GetService<NegocioService>()!;
    }

    [Fact(DisplayName = "Alterar situação Pedido com Sucesso")]
    [Trait("Categoria", "Alterar situação pedido Service Test")]
    public async Task Pedido_AlterarSituacao_DeveExecutarComSucesso()
    {
        ReiniciarDependencias();

        // Arrange
        var pedido = _pedidoTestsFixtures.RetornaPedidoAlterarSituacaoValido();
        _usuarioRepositoryMock.Setup(x => x.Atualizar(pedido)).ReturnsAsync(() => pedido);
        _usuarioRepositoryMock.Setup(x => x.Obter(It.IsAny<Expression<Func<Domain.Models.Pedido, bool>>>(), false)).ReturnsAsync(() => pedido);
        
        // Act
        await _pedidoService.AlterarSituacaoPedido(Guid.NewGuid(), SituacaoPedidoEnum.EmPreparo);
        await _negocioService.ExecutarValidacao();

        // Assert
        _usuarioRepositoryMock.Verify(mock => mock.Atualizar(pedido), Times.Once);
        Assert.True(_negocioService.EhValido());
    }

    [Fact(DisplayName = "Alterar situação Pedido cancelado")]
    [Trait("Categoria", "Alterar situação pedido Service Test")]
    public async Task Pedido_AlterarSituacao_PedidoCancelado()
    {
        ReiniciarDependencias();

        // Arrange
        var pedido = _pedidoTestsFixtures.RetornaPedidoAlterarSituacaoValido();
        _usuarioRepositoryMock.Setup(x => x.Atualizar(It.IsAny<Domain.Models.Pedido>())).ReturnsAsync(() => pedido);
        _usuarioRepositoryMock.Setup(x => x.Obter(It.IsAny<Expression<Func<Domain.Models.Pedido, bool>>>(), false)).ReturnsAsync(() => new Domain.Models.Pedido
        {
            SituacaoPedidoId = SituacaoPedidoEnum.Cancelado
        });

        // Act
        await _pedidoService.AlterarSituacaoPedido(Guid.NewGuid(), SituacaoPedidoEnum.EmPreparo);
        await _negocioService.ExecutarValidacao();

        // Assert
        _usuarioRepositoryMock.Verify(mock => mock.Atualizar(It.IsAny<Domain.Models.Pedido>()), Times.Once);
        Assert.False(_negocioService.EhValido());
        Assert.Contains(_notificador.ObterNotificacoes(),
            n => n.Mensagem.Contains(MensagensValidacaoResources.PedidoPagoOuCanceladoNaoPodeMudarSituacao));
    }

    [Fact(DisplayName = "Alterar situação Pedido pago")]
    [Trait("Categoria", "Alterar situação pedido Service Test")]
    public async Task Pedido_AlterarSituacao_PedidoPago()
    {
        ReiniciarDependencias();

        // Arrange
        var pedido = _pedidoTestsFixtures.RetornaPedidoAlterarSituacaoValido();
        _usuarioRepositoryMock.Setup(x => x.Atualizar(It.IsAny<Domain.Models.Pedido>())).ReturnsAsync(() => pedido);
        _usuarioRepositoryMock.Setup(x => x.Obter(It.IsAny<Expression<Func<Domain.Models.Pedido, bool>>>(), false)).ReturnsAsync(() => new Domain.Models.Pedido
        {
            SituacaoPedidoId = SituacaoPedidoEnum.PedidoPago
        });

        // Act
        await _pedidoService.AlterarSituacaoPedido(Guid.NewGuid(), SituacaoPedidoEnum.EmPreparo);
        await _negocioService.ExecutarValidacao();

        // Assert
        _usuarioRepositoryMock.Verify(mock => mock.Atualizar(It.IsAny<Domain.Models.Pedido>()), Times.Once);
        Assert.False(_negocioService.EhValido());
        Assert.Contains(_notificador.ObterNotificacoes(),
            n => n.Mensagem.Contains(MensagensValidacaoResources.PedidoPagoOuCanceladoNaoPodeMudarSituacao));
    }

    [Fact(DisplayName = "Alterar situação finalizar pedido com sucesso cancelado")]
    [Trait("Categoria", "Alterar situação pedido Service Test")]
    public async Task Pedido_AlterarSituacao_FinalizarPedidoComSucessoCancelado()
    {
        ReiniciarDependencias();

        // Arrange
        var pedido = _pedidoTestsFixtures.RetornaPedidoAlterarSituacaoValido();
        _usuarioRepositoryMock.Setup(x => x.Atualizar(It.IsAny<Domain.Models.Pedido>())).ReturnsAsync(() => pedido);
        _usuarioRepositoryMock.Setup(x => x.Obter(It.IsAny<Expression<Func<Domain.Models.Pedido, bool>>>(), false)).ReturnsAsync(() => pedido);
        _usuarioLogadoMock.Setup(x => x.ObterUsuarioLogado()).Returns(() => new UsuarioLogadoValueObject
        {
            Permissao = "ADMIN"
        });

        // Act
        await _pedidoService.AlterarSituacaoPedido(Guid.NewGuid(), SituacaoPedidoEnum.Cancelado);
        await _negocioService.ExecutarValidacao();

        // Assert
        _usuarioRepositoryMock.Verify(mock => mock.Atualizar(It.IsAny<Domain.Models.Pedido>()), Times.Once);
        Assert.True(_negocioService.EhValido());
    }

    [Fact(DisplayName = "Alterar situação finalizar pedido com sucesso pago")]
    [Trait("Categoria", "Alterar situação pedido Service Test")]
    public async Task Pedido_AlterarSituacao_FinalizarPedidoComSucessoPago()
    {
        ReiniciarDependencias();

        // Arrange
        var pedido = _pedidoTestsFixtures.RetornaPedidoAlterarSituacaoValido();
        _usuarioRepositoryMock.Setup(x => x.Atualizar(It.IsAny<Domain.Models.Pedido>())).ReturnsAsync(() => pedido);
        _usuarioRepositoryMock.Setup(x => x.Obter(It.IsAny<Expression<Func<Domain.Models.Pedido, bool>>>(), false)).ReturnsAsync(() => pedido);
        _usuarioLogadoMock.Setup(x => x.ObterUsuarioLogado()).Returns(() => new UsuarioLogadoValueObject
        {
            Permissao = "ADMIN"
        });

        // Act
        await _pedidoService.AlterarSituacaoPedido(Guid.NewGuid(), SituacaoPedidoEnum.PedidoPago);
        await _negocioService.ExecutarValidacao();

        // Assert
        _usuarioRepositoryMock.Verify(mock => mock.Atualizar(It.IsAny<Domain.Models.Pedido>()), Times.Once);
        Assert.True(_negocioService.EhValido());
    }

    [Fact(DisplayName = "Alterar situação finalizar pedido sem autorização cancelado")]
    [Trait("Categoria", "Alterar situação pedido Service Test")]
    public async Task Pedido_AlterarSituacao_FinalizarPedidoSemAutozicaoCancelado()
    {
        ReiniciarDependencias();

        // Arrange
        var pedido = _pedidoTestsFixtures.RetornaPedidoAlterarSituacaoValido();
        _usuarioRepositoryMock.Setup(x => x.Atualizar(It.IsAny<Domain.Models.Pedido>())).ReturnsAsync(() => pedido);
        _usuarioRepositoryMock.Setup(x => x.Obter(It.IsAny<Expression<Func<Domain.Models.Pedido, bool>>>(), false)).ReturnsAsync(() => pedido);
        _usuarioLogadoMock.Setup(x => x.ObterUsuarioLogado()).Returns(() => new UsuarioLogadoValueObject
        {
            Permissao = "USR"
        });

        // Act
        await _pedidoService.AlterarSituacaoPedido(Guid.NewGuid(), SituacaoPedidoEnum.Cancelado);
        await _negocioService.ExecutarValidacao();

        // Assert
        _usuarioRepositoryMock.Verify(mock => mock.Atualizar(It.IsAny<Domain.Models.Pedido>()), Times.Once);
        Assert.False(_negocioService.EhValido());
        Assert.Contains(_notificador.ObterNotificacoes(),
            n => n.Mensagem.Contains(MensagensValidacaoResources.UsuarioSemPermissao));
    }

    [Fact(DisplayName = "Alterar situação finalizar pedido sem autorização pago")]
    [Trait("Categoria", "Alterar situação pedido Service Test")]
    public async Task Pedido_AlterarSituacao_FinalizarPedidoSemAutozicaoPago()
    {
        ReiniciarDependencias();

        // Arrange
        var pedido = _pedidoTestsFixtures.RetornaPedidoAlterarSituacaoValido();
        _usuarioRepositoryMock.Setup(x => x.Atualizar(It.IsAny<Domain.Models.Pedido>())).ReturnsAsync(() => pedido);
        _usuarioRepositoryMock.Setup(x => x.Obter(It.IsAny<Expression<Func<Domain.Models.Pedido, bool>>>(), false)).ReturnsAsync(() => pedido);
        _usuarioLogadoMock.Setup(x => x.ObterUsuarioLogado()).Returns(() => new UsuarioLogadoValueObject
        {
            Permissao = "USR"
        });

        // Act
        await _pedidoService.AlterarSituacaoPedido(Guid.NewGuid(), SituacaoPedidoEnum.PedidoPago);
        await _negocioService.ExecutarValidacao();

        // Assert
        _usuarioRepositoryMock.Verify(mock => mock.Atualizar(It.IsAny<Domain.Models.Pedido>()), Times.Once);
        Assert.False(_negocioService.EhValido());
        Assert.Contains(_notificador.ObterNotificacoes(),
            n => n.Mensagem.Contains(MensagensValidacaoResources.UsuarioSemPermissao));
    }

    private void ReiniciarDependencias()
    {
        _usuarioRepositoryMock.Reset();
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
                .AddScoped(_ => Mock.Of<IUsuarioLogadoService>())
                .AddScoped(_ => Mock.Of<IProdutoApiService>());
        }
    }
}