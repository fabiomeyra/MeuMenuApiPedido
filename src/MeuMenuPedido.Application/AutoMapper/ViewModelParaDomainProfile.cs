using AutoMapper;
using MeuMenuPedido.Application.Filtros.Base;
using MeuMenuPedido.Application.Filtros.Pedido;
using MeuMenuPedido.Application.ViewModels;
using MeuMenuPedido.Application.ViewModels.Pedido;
using MeuMenuPedido.Application.ViewModels.Produto;
using MeuMenuPedido.Domain.Filtros.Base;
using MeuMenuPedido.Domain.Filtros.Pedido;
using MeuMenuPedido.Domain.Models;

namespace MeuMenuPedido.Application.AutoMapper;

public class ViewModelParaDomainProfile : Profile
{
    public ViewModelParaDomainProfile()
    {
        CreateMap<PedidoViewModel, Pedido>();
        CreateMap<PedidoProdutoViewModel, PedidoProduto>();

        CreateMap<AdicionarPedidoViewModel, Pedido>()
            .ForMember(x => x.PedidoId, opt => opt.Ignore())
            .ForMember(x => x.DataCadastro, opt => opt.Ignore())
            .ForMember(x => x.ProdutosDoPedido, opt => opt.MapFrom(src => src.Produtos))
            .ForMember(x => x.SituacaoPedidoId, opt => opt.Ignore());

        CreateMap<ProdutoQuantidadeViewModel, PedidoProduto>()
            .ForMember(x => x.ProdutoValor, opt => opt.Ignore())
            .ForMember(x => x.Pedido, opt => opt.Ignore())
            .ForMember(x => x.PedidoId, opt => opt.Ignore());

        // Mapeamento de filtros
        MapearFiltros();
    }

    private void MapearFiltros()
    {
        CreateMap<BaseFiltroViewModel, BaseFiltro>();
        CreateMap<BuscarPedidoPorSituacaoFiltroViewModel, BuscarPedidoPorSituacaoFiltro>();
    }
}