namespace MeuMenuPedido.Infra.CrossCutting.AppSettings;

public class ApplicationInsightsAppSettings : BaseAppSettings
{
    private string? _connectionString;

    public string? ConnectionString
    {
        get => RetornaValorDescriptografado(_connectionString);
        set => _connectionString = value;
    }
   
}