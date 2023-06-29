USE [MeuMenuDb]
GO

CREATE TABLE Pedido.MesaPedido (
	MesaPedidoNumero int not null,
)
GO

ALTER TABLE Pedido.MesaPedido
ADD CONSTRAINT PK_MESAPEDIDO PRIMARY KEY (MesaPedidoNumero)
GO