using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace MeuMenuPedido.Infra.CrossCutting.AppSettings;

public class JwtAppSettings
{
    private string? _secretKey;
    private SymmetricSecurityKey? _key;

    /// <summary>
    /// Audience
    /// </summary>
    public string? Audience { get; set; }
    /// <summary>
    /// Issuer
    /// </summary>
    public string? Issuer { get; set; }
    /// <summary>
    /// Time Expire
    /// </summary>
    public int ExpiresIn { get; set; }
    public TimeSpan ExpiresSpan { get; } = TimeSpan.Zero;

    /// <summary>
    /// Secret Key
    /// </summary>
    public string? SecretKey
    {
        get => _secretKey == null ? null : Convert.ToBase64String(Encoding.UTF8.GetBytes(_secretKey));
        set => _secretKey = value;
    }

    /// <summary>
    /// Token Type
    /// </summary>
    public string? TokenType { get; set; }

    /// <summary>
    /// Secret Key
    /// </summary>
    public SymmetricSecurityKey RetornaKey()
    {
       _key ??= new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey!));
       return _key;
    }

    /// <summary>
    /// Signing Credentials 
    /// </summary>
    public SigningCredentials ObterSigningCredentials()
    {
        return new SigningCredentials(RetornaKey(), SecurityAlgorithms.HmacSha256);
    }

    /// <summary>
    /// Retorna hora da expiração do token
    /// </summary>
    /// <returns></returns>
    public DateTime RetornaHoraExpiracaoToken() => DateTime.Now.AddHours(ExpiresIn);
}