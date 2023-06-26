using AutoMapper;
using MeuMenuPedido.Application.ViewModels;
using MeuMenuPedido.Domain.Models;
using MeuMenuPedido.Domain.Utils;

namespace MeuMenuPedido.Application.AutoMapper;

public class DomainParaViewModelProfile : Profile
{
    public DomainParaViewModelProfile()
    {
        CreateMap<PedidoProduto, PedidoProdutoViewModel>();
        CreateMap<Pedido, PedidoViewModel>()
            .ForMember(x => x.SituacaoPedidoDrescricao, opt => opt.MapFrom(src => src.SituacaoPedidoId == null ? null : src.SituacaoPedidoId.ObterDescricaoEnum()));
    }
}