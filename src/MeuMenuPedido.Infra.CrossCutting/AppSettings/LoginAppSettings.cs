namespace MeuMenuPedido.Infra.CrossCutting.AppSettings;

public class LoginAppSettings : BaseAppSettings
{
    private string? _secretKey;

    public string? SecretKey
    {
        get => RetornaValorDescriptografado(_secretKey);
        set => _secretKey = value;
    }
}