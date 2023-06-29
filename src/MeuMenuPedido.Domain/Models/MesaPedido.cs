using MeuMenuPedido.Domain.Models.Base;

namespace MeuMenuPedido.Domain.Models;

public class MesaPedido : EntidadeValidavelModel<MesaPedido>
{
    public int MesaPedidoNumero { get; set; }
}