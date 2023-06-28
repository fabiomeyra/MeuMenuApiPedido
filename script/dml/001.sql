USE [MeuMenuDb]
GO

INSERT INTO Pedido.SituacaoPedido
(SituacaoPedidoId, SituacaoPedidoDescricao)
VALUES
(1, 'Enviado'),
(2, 'Em preparo'),
(3, 'Pronto'),
(4, 'Entregue'),
(5, 'Conta solicitada'),
(6, 'Pedido pago'),
(7, 'Cancelado')
GO