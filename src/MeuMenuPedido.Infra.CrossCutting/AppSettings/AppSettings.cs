namespace MeuMenuPedido.Infra.CrossCutting.AppSettings;

public class AppSettings
{
    public ConnectionStringAppSettings? ConnectionString { get; set; }
    public ApisSettings? Apis { get; set; }
    public JwtAppSettings? Jwt { get; set; }
    public bool EhAmbienteDeProducao { get; set; }
}