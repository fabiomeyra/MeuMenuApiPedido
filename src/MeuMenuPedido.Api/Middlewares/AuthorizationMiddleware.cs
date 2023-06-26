using MeuMenuPedido.Domain.Services.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;

namespace MeuMenuPedido.Api.Middlewares;

public class AuthorizationMiddleware : IAuthorizationMiddlewareResultHandler
{
    private IUsuarioLogadoService? _usuarioLogadoService;

    private readonly AuthorizationMiddlewareResultHandler _defaultHandler = new();

    public async Task HandleAsync(
        RequestDelegate next,
        HttpContext context,
        AuthorizationPolicy policy,
        PolicyAuthorizationResult authorizeResult)
    {
        //deve - se buscar a dependência do requestServices para que a instância seja a mesma durante o request
        _usuarioLogadoService = context.RequestServices.GetService(typeof(IUsuarioLogadoService)) as IUsuarioLogadoService;

        // definindo usuário logado para usar durante toda a execução da requisição
        var usuario = context.User.Claims.FirstOrDefault(c => c.Type == "Usuario")?.Value;
        _usuarioLogadoService!.DefinirUsuarioLogado(usuario);

        // passa para o próximo middleware
        await _defaultHandler.HandleAsync(next, context, policy, authorizeResult);
    }
}