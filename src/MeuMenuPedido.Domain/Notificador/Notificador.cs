using MeuMenuPedido.Domain.Interfaces.Notificador;

namespace MeuMenuPedido.Domain.Notificador;

public class Notificador : INotificador
{
    private ICollection<Notificacao> _notificacoes;

    public Notificador()
    {
        _notificacoes = new List<Notificacao>();
    }

    public bool TemNotificacao()
    {
        return _notificacoes.Any();
    }

    public ICollection<Notificacao> ObterNotificacoes()
    {
        return _notificacoes;
    }

    public void AdicionarNotificacao(Notificacao notificacao)
    {
        _notificacoes.Add(notificacao);
    }

    public void AdicionarNotificacao(ICollection<Notificacao> notificacoes)
    {
        foreach (var notificacao in notificacoes)
            _notificacoes.Add(notificacao);
    }

    public void LimparNotificacoes()
    {
        _notificacoes = new List<Notificacao>();
    }
}