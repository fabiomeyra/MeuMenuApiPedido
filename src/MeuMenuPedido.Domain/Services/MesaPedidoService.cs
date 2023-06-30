using MeuMenuPedido.Domain.Interfaces.Repositories;
using MeuMenuPedido.Domain.Interfaces.Services;
using MeuMenuPedido.Domain.Models;
using MeuMenuPedido.Domain.Services.Base;
using MeuMenuPedido.Domain.Utils;
using MeuMenuPedido.Domain.Validations.MesaPedido;

namespace MeuMenuPedido.Domain.Services;

public class MesaPedidoService : BaseService<MesaPedido>, IMesaPedidoService
{

    public MesaPedidoService(
        IMesaPedidoRepository repository, 
        NegocioService negocioService
    ) 
        : base(repository, negocioService)
    { }

    public async Task<bool> PodeAbrirMesaPedidoNaMesa(int numeroMesa)
    {
        var mesaOcupada = await Obter(x => x.MesaPedidoNumero == numeroMesa, x => true, true);
        return !mesaOcupada;
    }

    public override Task<MesaPedido> Adicionar(MesaPedido objeto)
    {
        objeto.AdicionarValidacaoEntidade(NegocioService, new OcuparMesaValidation(this));
        return base.Adicionar(objeto);
    }

    public override Task Excluir(MesaPedido objeto)
    {
        objeto.AdicionarValidacaoEntidade(NegocioService, new DesocuparMesaValidation(this));
        return base.Excluir(objeto);
    }
}