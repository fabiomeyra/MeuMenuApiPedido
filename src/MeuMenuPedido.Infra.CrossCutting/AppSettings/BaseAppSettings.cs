namespace MeuMenuPedido.Infra.CrossCutting.AppSettings;

public abstract class BaseAppSettings
{
    protected string? RetornaValorDescriptografado(string? valor)
    {
        if (valor == null) return null;
        try
        {
            return ServicoDeCriptografia.Descriptografar(valor);
        }
        catch
        {
            return valor;
        }
    }
}