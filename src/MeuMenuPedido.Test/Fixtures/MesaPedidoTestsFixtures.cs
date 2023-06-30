using MeuMenuPedido.Domain.Models;
using Xunit;

namespace MeuMenuPedido.Test.Fixtures;

[CollectionDefinition(nameof(MesaPedidoCollectoin))]
public class MesaPedidoCollectoin : ICollectionFixture<MesaPedidoTestsFixtures>
{
    
}

public class MesaPedidoTestsFixtures : IDisposable
{
    public MesaPedido RetornaMesaPedidoValido()
    {
        return new MesaPedido
        {
            MesaPedidoNumero = 1
        };
    }

    public MesaPedido RetornaMesaPedidoInvalido()
    {
        return new MesaPedido();
    }

    public void Dispose()
    {
    }
}