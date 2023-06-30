using System.Diagnostics.CodeAnalysis;
using MeuMenuPedido.Infra.CrossCutting.AppSettings;
using MeuMenuPedido.Infra.IoC;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MeuMenuPedido.Test;

public class BaseStartup : IDisposable
{
    public BaseStartup()
    {
        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);
        ServiceProvider = serviceCollection.BuildServiceProvider();
    }

    public IConfigurationRoot? Configuration { get; private set; }
    public ServiceProvider ServiceProvider { get;}

    public void ConfigureServices(IServiceCollection services)
    {
        IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", true, false);
        Configuration = builder.Build();
        services.AddSingleton(Configuration);

        var appSettingsSection = Configuration.GetSection("AppSettings");
        services.Configure<AppSettings>(appSettingsSection);

        services.ResolveDependencias();

        _ = OnConfigureServices(services);
    }

    public void Dispose()
    {
        ServiceProvider.Dispose();
    }

    [ExcludeFromCodeCoverage]
    public virtual IServiceCollection OnConfigureServices(IServiceCollection services)
    {
        return services;
    }
}