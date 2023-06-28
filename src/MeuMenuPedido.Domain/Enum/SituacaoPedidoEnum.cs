using System.ComponentModel;

namespace MeuMenuPedido.Domain.Enum;

public enum SituacaoPedidoEnum
{
    Enviado = 1,
    [Description("Em preparo")]
    EmPreparo,
    Pronto,
    Entregue,
    [Description("Conta solicitada")]
    ContaSolicitada,
    [Description("Pedido pago")]
    PedidoPago,
    Cancelado
}