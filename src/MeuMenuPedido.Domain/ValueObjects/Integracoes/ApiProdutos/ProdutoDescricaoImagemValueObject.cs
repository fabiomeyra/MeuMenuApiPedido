namespace MeuMenuPedido.Domain.ValueObjects.Integracoes.ApiProdutos;


public class ProdutoDescricaoImagemValueObject
{
    public Guid ProdutoId { get; set; }
    public string? ProdutoDescricao { get; set; }
    public string? ProdutoImagem { get; set; }
}