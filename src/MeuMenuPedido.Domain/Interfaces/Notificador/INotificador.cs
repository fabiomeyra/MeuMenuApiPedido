using MeuMenuPedido.Domain.Notificador;

namespace MeuMenuPedido.Domain.Interfaces.Notificador;

public interface INotificador
{
    bool TemNotificacao();
    ICollection<Notificacao> ObterNotificacoes();
    void AdicionarNotificacao(Notificacao notificacao);
    void AdicionarNotificacao(ICollection<Notificacao> notificacoes);
    void LimparNotificacoes();
}