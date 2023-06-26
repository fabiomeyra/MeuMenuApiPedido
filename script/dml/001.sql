USE [MeuMenuDb]
GO

CREATE SCHEMA Pedido;  
GO


CREATE TABLE Pedido.SituacaoPedido(
	SituacaoPedidoId int not null,
	SituacaoPedidoDescricao varchar(100) not null
)
GO

ALTER TABLE Pedido.SituacaoPedido
ADD CONSTRAINT PK_SITUACAO_PEDIDO PRIMARY KEY (SituacaoPedidoId)
GO

CREATE TABLE Pedido.Pedido (
	PedidoId uniqueidentifier NOT NULL,
	PedidoMesa int NOT NULL,
	DataCadastro datetime not null,
	SituacaoPedidoId int NOT NULL,
	PedidoObservacao varchar(500),
)
GO

ALTER TABLE Pedido.Pedido
ADD CONSTRAINT PK_PEDIDO PRIMARY KEY (PedidoId)
GO

ALTER TABLE Pedido.Pedido
ADD CONSTRAINT FK_PEDIDO_SITUACAO FOREIGN KEY(SituacaoPedidoId) REFERENCES Pedido.SituacaoPedido(SituacaoPedidoId)
GO

CREATE TABLE Pedido.PedidoProduto (
	PedidoId uniqueidentifier NOT NULL,
	ProdutoId uniqueidentifier NOT NULL,
	ProdutoValor decimal(19, 2) NOT NULL,
	ProdutoQuantidade int NOT NULL
)
GO

ALTER TABLE Pedido.PedidoProduto
ADD CONSTRAINT PK_PEDIDO_PRODUTO PRIMARY KEY (PedidoId, ProdutoId)
GO

ALTER TABLE Pedido.PedidoProduto
ADD CONSTRAINT FK_PEDIDO_PRODUTO_PEDIDO FOREIGN KEY(PedidoId) REFERENCES Pedido.Pedido(PedidoId)
GO

ALTER TABLE Pedido.PedidoProduto
ADD CONSTRAINT FK_PEDIDO_PRODUTO_PRODUTO FOREIGN KEY(ProdutoId) REFERENCES Produto.Produto(ProdutoId)
GO

