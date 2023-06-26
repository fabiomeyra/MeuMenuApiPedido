using System.ComponentModel;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using FluentValidation;
using MeuMenuPedido.Domain.Models.Base;
using MeuMenuPedido.Domain.Services.Base;

namespace MeuMenuPedido.Domain.Utils;

public static class MetodosDeExtensao
{
    /// <summary>
    /// Adiciona uma validação do tipo genérico AbstractValidator de TE na entidade e adiciona essa validação no serviço de negócio da aplicação para que a validação possa ser executada de uma 
    /// só vez pelo controlador da transação, caso esse queira.
    /// </summary>
    /// <typeparam name="TE">Classe a ser validada</typeparam>
    /// <param name="this">Objeto a ser validado</param>
    /// <param name="instanciaNegocioService">Instância do serviço de negócio ao qual a validação deve ser adicionada</param>
    /// <param name="validacao">Validação a ser adicionada para a entidade</param>
    /// <returns>EntidadeValidaVel de TE</returns>
    public static EntidadeValidavelModel<TE> AdicionarValidacaoEntidade<TE>(
        this EntidadeValidavelModel<TE> @this,
        NegocioService instanciaNegocioService,
        AbstractValidator<TE> validacao) where TE : class
    {
        //var entidadeString = JsonSerializer.Serialize(@this as TE, new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.IgnoreCycles, IncludeFields = true });
        //var entidade = JsonSerializer.Deserialize<TE>(entidadeString);

        var entidade = (@this as TE).CopiarObjeto();
        instanciaNegocioService.AdicionarValidacaoEntidade(entidade!, validacao, typeof(TE), @this.ObterGuidEntidade());
        return @this;
    }

    /// <summary>
    /// Copia os valores do Objeto retornando para um novo objeto
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="this"></param>
    /// <returns></returns>
    public static T? CopiarObjeto<T>(this T @this)
    {
        var copiaString = JsonSerializer.Serialize(@this, new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.IgnoreCycles, IncludeFields = true });
        var retorno = JsonSerializer.Deserialize<T>(copiaString, new JsonSerializerOptions { IncludeFields = true });
        return retorno;
    }

    public static ICollection<(int Valor, string? Nome)> ParaListaDeSelecao<TEnum>(this TEnum @this) where TEnum : struct, System.Enum
    {
        var lista = from System.Enum e in System.Enum.GetValues(typeof(TEnum))
            select (Valor: Convert.ToInt32(e), Nome: e.ObterDescricaoEnum());

        return lista.ToList();
    }

    public static string? ObterDescricaoEnum(this System.Enum enumValue)
    {
        var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());

        if (fieldInfo is null) return null;

        var descriptionAttributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

        return descriptionAttributes.Length > 0 ? descriptionAttributes[0].Description : enumValue.ToString();
    }

    public static async Task<object> InvokeAsync(this MethodInfo @this, object obj, params object[] parameters)
    {
        dynamic awaitable = @this.Invoke(obj, parameters)!;
        await awaitable;
        return awaitable.GetAwaiter().GetResult();
    }
}