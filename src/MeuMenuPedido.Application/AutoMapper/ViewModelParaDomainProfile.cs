using AutoMapper;
using MeuMenuPedido.Application.ViewModels;
using MeuMenuPedido.Domain.Models;

namespace MeuMenuPedido.Application.AutoMapper;

public class ViewModelParaDomainProfile : Profile
{
    public ViewModelParaDomainProfile()
    {
        CreateMap<PedidoViewModel, Pedido>();
        CreateMap<PedidoProdutoViewModel, PedidoProduto>();
    }
}