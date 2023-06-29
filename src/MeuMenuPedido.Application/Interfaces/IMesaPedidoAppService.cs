namespace MeuMenuPedido.Application.Interfaces;

public interface IMesaPedidoAppService
{
    Task OcuparMesa(int numeroMesa);
    Task DesocuparMesa(int numeroMesa);
}