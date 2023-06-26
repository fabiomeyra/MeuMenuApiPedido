using System.Text.Json;
using MeuMenuPedido.Domain.Services.Utils;
using MeuMenuPedido.Domain.ValueObjects;

namespace MeuMenuPedido.Infra.CrossCutting;

public class UsuarioLogadoService : IUsuarioLogadoService
{
    private UsuarioLogadoValueObject? _usuarioLogado;

    public UsuarioLogadoValueObject? ObterUsuarioLogado() => _usuarioLogado;

    public void DefinirUsuarioLogado(string? usuarioString)
    {
        _usuarioLogado = usuarioString == null ? null : JsonSerializer.Deserialize<UsuarioLogadoValueObject>(usuarioString);
    }
}