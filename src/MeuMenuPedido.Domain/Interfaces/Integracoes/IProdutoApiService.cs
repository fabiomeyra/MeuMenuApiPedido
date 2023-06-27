using MeuMenuPedido.Domain.ValueObjects.Integracoes.ApiProdutos;

namespace MeuMenuPedido.Domain.Interfaces.Integracoes;

public interface IProdutoApiService
{
    Task<ProdutoValorValueObject> BuscarProdutoValor(Guid produtoId);
}