namespace MeuMenuPedido.Infra.CrossCutting.AppSettings;

public class ConnectionStringAppSettings : BaseAppSettings
{
    private string? _meuMenudb;
    public string? MeuMenuDb
    {
        get
        {
            var valor = RetornaValorDescriptografado(_meuMenudb);
            return $"{valor}Persist Security Info=True;";
        }
        set => _meuMenudb = value;
    }
}