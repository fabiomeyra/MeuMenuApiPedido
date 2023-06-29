using MeuMenuPedido.Domain.Interfaces.Services.Base;
using MeuMenuPedido.Domain.Models;

namespace MeuMenuPedido.Domain.Interfaces.Services;

public interface IMesaPedidoService : IBaseService<MesaPedido>
{
    Task<bool> PodeAbrirMesaPedidoNaMesa(int numeroMesa);
}