using AutoMapper;
using MeuMenuPedido.Application.ViewModels;
using MeuMenuPedido.Application.ViewModels.Pedido;
using MeuMenuPedido.Application.ViewModels.Produto;
using MeuMenuPedido.Domain.Models;
using MeuMenuPedido.Domain.Utils;
using MeuMenuPedido.Domain.ValueObjects.Integracoes.ApiProdutos;

namespace MeuMenuPedido.Application.AutoMapper;

public class DomainParaViewModelProfile : Profile
{
    public DomainParaViewModelProfile()
    {
        CreateMap<PedidoProduto, PedidoProdutoViewModel>();
        CreateMap<ProdutoDescricaoImagemValueObject, ProdutoDescricaoImagemViewModel>();
        CreateMap<Pedido, PedidoViewModel>()
            .ForMember(x => x.SituacaoPedidoDrescricao, opt => opt.MapFrom(src => src.SituacaoPedidoId == null ? null : src.SituacaoPedidoId.ObterDescricaoEnum()));
    }
}