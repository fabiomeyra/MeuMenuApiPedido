using MeuMenuPedido.Domain.ValueObjects;

namespace MeuMenuPedido.Domain.Services.Utils;

public interface IUsuarioLogadoService
{
    UsuarioLogadoValueObject? ObterUsuarioLogado();
    void DefinirUsuarioLogado(string? usuarioString);
}