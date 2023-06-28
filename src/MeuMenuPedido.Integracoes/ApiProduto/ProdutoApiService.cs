using MeuMenuPedido.Domain.Interfaces.Integracoes;
using MeuMenuPedido.Domain.ValueObjects.Integracoes.ApiProdutos;
using MeuMenuPedido.Infra.CrossCutting.AppSettings;
using MeuMenuPedido.Integracoes.ApiProduto.Models;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text.Json;

namespace MeuMenuPedido.Integracoes.ApiProduto;

public class ProdutoApiService : IProdutoApiService
{
    private readonly string _urlApiProdutos;

    public ProdutoApiService(IOptions<AppSettings> op)
    {
        _urlApiProdutos = op.Value.Apis?.ApiProduto ?? "";
    }

    public async Task<ProdutoValorValueObject?> BuscarProdutoValor(Guid produtoId)
    {
        using var webClient = new HttpClient();
        if (string.IsNullOrWhiteSpace(_urlApiProdutos)) throw new Exception("Url api produtos não configurada");
        var url = $"{_urlApiProdutos}/api/produto/produto-valor?id={produtoId}";
        var resposta = await webClient.GetAsync(url);
        if (resposta.StatusCode != HttpStatusCode.OK) return null!;
        var resultadoString = await resposta.Content.ReadAsStringAsync();
        var resultado = JsonSerializer.Deserialize<ApiRetorno<ProdutoValorValueObject>>(resultadoString, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        return resultado?.Success ?? false ? resultado.Data : null!;
    }

    public async Task<ICollection<ProdutoDescricaoImagemValueObject>?> BuscarImagemEDescricaoProdutos(ICollection<Guid> listaIds)
    {
        if (!listaIds.Any()) return null;
        using var webClient = new HttpClient();
        if (string.IsNullOrWhiteSpace(_urlApiProdutos)) throw new Exception("Url api produtos não configurada");

        var parametros = listaIds.First().ToString();
        
        for (int i = 1; i < listaIds.Count; i++)
            parametros += $"&ids={listaIds.ElementAt(i)}";

        var url = $"{_urlApiProdutos}/api/produto/buscar-descricao-e-imagem?ids={parametros}";
        var resposta = await webClient.GetAsync(url);
        if (resposta.StatusCode != HttpStatusCode.OK) return null!;
        var resultadoString = await resposta.Content.ReadAsStringAsync();
        var resultado = JsonSerializer.Deserialize<ApiRetorno<ICollection<ProdutoDescricaoImagemValueObject>>>(resultadoString, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        return resultado?.Success ?? false ? resultado.Data : null!;
    }
}