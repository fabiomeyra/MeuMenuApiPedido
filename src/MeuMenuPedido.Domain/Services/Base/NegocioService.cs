using FluentValidation.Results;
using MeuMenuPedido.Domain.Interfaces.Notificador;
using MeuMenuPedido.Domain.Notificador;
using MeuMenuPedido.Domain.Utils;

namespace MeuMenuPedido.Domain.Services.Base;

public class NegocioService
{
    private readonly INotificador _notificador;

    public NegocioService(INotificador notificador)
    {
        _notificador = notificador;
    }

    public bool EhValido() => !_notificador.TemNotificacao();

    public void LimparNotificacoes() => _notificador.LimparNotificacoes();
    public void LimparValidacoes() => _validacoes = new Dictionary<Guid, EntidadeValidacao>();

    private IDictionary<Guid, EntidadeValidacao> _validacoes = new Dictionary<Guid, EntidadeValidacao>();

    protected void Notificar(ValidationResult resultadoValidacao)
    {
        foreach (var x in resultadoValidacao.Errors)
            _notificador.AdicionarNotificacao(new Notificacao(x.ErrorMessage));
    }

    public void AdicionarValidacaoEntidade(object entidade, object validacao, Type tipoEntidade, Guid guidEntidade)
    {
        _validacoes.TryGetValue(guidEntidade, out var entidadeDicionario);

        entidadeDicionario ??= new EntidadeValidacao(entidade, tipoEntidade);
        entidadeDicionario.AdicionarValidacao(validacao);

        _validacoes[guidEntidade] = entidadeDicionario;
    }

    public async Task ExecutarValidacao()
    {
        foreach (var x in _validacoes)
        {
            foreach (var y in x.Value.Validacoes ?? new List<object>())
            {
                var type = y.GetType();
                var metodo = type.GetMethod("ValidateAsync", new [] {x.Value.TipoEntidade, typeof(CancellationToken) });
                var validador = await metodo?.InvokeAsync(y, new[] { x.Value.Entidade, new CancellationToken() })!;
                if (((ValidationResult)validador).IsValid) continue;
                Notificar((ValidationResult)validador);
            }
        }
    }
}

 internal class EntidadeValidacao
{
    public EntidadeValidacao(object entidade, Type tipoEntidade)
    {
        Entidade = entidade;
        TipoEntidade = tipoEntidade;
    }
    public Type TipoEntidade { get; }
    public object Entidade { get; }
    public ICollection<object>? Validacoes { get; private set; }

    internal EntidadeValidacao AdicionarValidacao(object validacao)
    {
        Validacoes ??= new List<object>();
        Validacoes.Add(validacao);
        return this;
    }
}