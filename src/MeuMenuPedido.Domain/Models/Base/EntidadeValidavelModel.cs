namespace MeuMenuPedido.Domain.Models.Base;

public abstract class EntidadeValidavelModel<TE> where TE : class
{
    private readonly Guid _entidadeValidavelId;

    protected EntidadeValidavelModel()
    {
        _entidadeValidavelId = Guid.NewGuid();
    }

    public Guid ObterGuidEntidade() => _entidadeValidavelId;

    public virtual TE LimparPropriedadesNavegacao()
    {
        return null!;
    }
}