using System.Text.Json.Serialization;

namespace MeuMenuPedido.Integracoes.ApiProduto.Models;

public class ApiRetorno<T> where T : class
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public ICollection<string> Errors { get; set; } = new List<string>();
}