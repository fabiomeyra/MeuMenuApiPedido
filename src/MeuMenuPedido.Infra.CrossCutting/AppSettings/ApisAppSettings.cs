namespace MeuMenuPedido.Infra.CrossCutting.AppSettings;

public class ApisSettings : BaseAppSettings
{
    private string? _apiProduto;

    public string? ApiProduto
    {
        get => RetornaValorDescriptografado(_apiProduto);
        set => _apiProduto = value;
    }
}