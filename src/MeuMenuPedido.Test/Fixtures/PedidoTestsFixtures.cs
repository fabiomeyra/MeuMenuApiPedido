using MeuMenuPedido.Application.ViewModels.Pedido;
using MeuMenuPedido.Application.ViewModels.Produto;
using MeuMenuPedido.Domain.Enum;
using MeuMenuPedido.Domain.Models;
using Xunit;

namespace MeuMenuPedido.Test.Fixtures;

[CollectionDefinition(nameof(PedidoCollectoin))]
public class PedidoCollectoin : ICollectionFixture<PedidoTestsFixtures>
{
    
}

public class PedidoTestsFixtures : IDisposable
{
    public AlterarSituacaoPedidoViewModel RetornaAlterarSituacaoPedidoViewModelValido()
    {
        return new AlterarSituacaoPedidoViewModel
        {
            PedidoId = Guid.NewGuid(),
            SituacaoId = 2
        };
    }

    public AlterarSituacaoPedidoViewModel RetornaAlterarSituacaoPedidoViewModelAtualizarCanceladoValido()
    {
        return new AlterarSituacaoPedidoViewModel
        {
            PedidoId = Guid.NewGuid(),
            SituacaoId = 7
        };
    }

    public AlterarSituacaoPedidoViewModel RetornaAlterarSituacaoPedidoViewModelAtualizarPagoValido()
    {
        return new AlterarSituacaoPedidoViewModel
        {
            PedidoId = Guid.NewGuid(),
            SituacaoId = 6
        };
    }

    public Pedido RetornaPedidoValido()
    {
        return new Pedido
        {
            PedidoMesa = 1,
            ProdutosDoPedido = new List<PedidoProduto>
            {
                new PedidoProduto
                {
                    ProdutoId = Guid.NewGuid(),
                    ProdutoQuantidade = 2
                }
            }
        };
    }

    public AdicionarPedidoViewModel RetornaPedidoValidoViewModel()
    {
        return new AdicionarPedidoViewModel
        {
            PedidoMesa = 1,
            Produtos = new List<ProdutoQuantidadeViewModel>
            {
                new ProdutoQuantidadeViewModel
                {
                    ProdutoId = Guid.NewGuid(),
                    ProdutoQuantidade = 2
                }
            }
        };
    }

    public AdicionarPedidoViewModel RetornaPedidoComProdutoInvalidoViewModel()
    {
        return new AdicionarPedidoViewModel
        {
            PedidoMesa = 1,
            Produtos = new List<ProdutoQuantidadeViewModel>
            {
                new ProdutoQuantidadeViewModel()
            }
        };
    }

    public AdicionarPedidoViewModel RetornaPedidoInvalidoViewModel()
    {
        return new AdicionarPedidoViewModel();
    }

    public Pedido RetornaPedidoAlterarSituacaoValido()
    {
        return new Pedido
        {
            PedidoMesa = 1,
            SituacaoPedidoId = SituacaoPedidoEnum.Enviado,
            ProdutosDoPedido = new List<PedidoProduto>
            {
                new PedidoProduto
                {
                    ProdutoId = Guid.NewGuid(),
                    ProdutoQuantidade = 2
                }
            }
        };
    }

    public Pedido RetornaPedidoInvalido()
    {
        var pedidoId = Guid.NewGuid();

        return new Pedido();
    }

    public Pedido RetornaPedidoComProdutoInvalido()
    {
        var pedidoId = Guid.NewGuid();

        return new Pedido
        {
            ProdutosDoPedido = new List<PedidoProduto>
            {
                new PedidoProduto()
            }
        };
    }

    public void Dispose()
    {
    }
}