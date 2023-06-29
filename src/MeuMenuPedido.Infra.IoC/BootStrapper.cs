using MeuMenuPedido.Application.AppServices;
using MeuMenuPedido.Application.Interfaces;
using MeuMenuPedido.Domain.Interfaces.Context;
using MeuMenuPedido.Domain.Interfaces.Integracoes;
using MeuMenuPedido.Domain.Interfaces.Notificador;
using MeuMenuPedido.Domain.Interfaces.Repositories;
using MeuMenuPedido.Domain.Interfaces.Services;
using MeuMenuPedido.Domain.Notificador;
using MeuMenuPedido.Domain.Services;
using MeuMenuPedido.Domain.Services.Base;
using MeuMenuPedido.Domain.Services.Utils;
using MeuMenuPedido.Domain.UoW;
using MeuMenuPedido.Infra.CrossCutting;
using MeuMenuPedido.Infra.Data.Context;
using MeuMenuPedido.Infra.Data.Repositories;
using MeuMenuPedido.Infra.Data.UoW;
using MeuMenuPedido.Integracoes.ApiProduto;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace MeuMenuPedido.Infra.IoC;

public static class BootStrapper
{
    public static IServiceCollection ResolveDependencias(this IServiceCollection services)
    {
        services.AddScoped<INotificador, Notificador>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<NegocioService>();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        // Serviço com informações do usuário logado
        services.AddScoped<IUsuarioLogadoService, UsuarioLogadoService>();

        // Automapper
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


        services.AddScoped(provider =>
        {
            // pega a mesma instância configurada para a injeção da interface do contexto,
            // assim é possível injetar direto via classe para usar na camada de dados e via interface nas demais camadas
            var service = provider.GetService<IMeuMenuPedidoContext>() as MeuMenuPedidoContext;
            return service!;
        });
        services.AddScoped<IMeuMenuPedidoContext, MeuMenuPedidoContext>();

        // appServices
        services.AddScoped<IPedidoAppService, PedidoAppService>();
        services.AddScoped<IPedidoProdutoAppService, PedidoProdutoAppService>();
        services.AddScoped<IMesaPedidoAppService, MesaPedidoAppService>();

        // services
        services.AddScoped<IPedidoService, PedidoService>();
        services.AddScoped<IPedidoProdutoService, PedidoProdutoService>();
        services.AddScoped<IMesaPedidoService, MesaPedidoService>();

        // repositories
        services.AddScoped<IPedidoRepository, PedidoRepository>();
        services.AddScoped<IPedidoProdutoRepository, PedidoProdutoRepository>();
        services.AddScoped<IMesaPedidoRepository, MesaPedidoRepository>();

        // integrações
        services.AddScoped<IProdutoApiService, ProdutoApiService>();

        return services;
    }
}