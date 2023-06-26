using AutoMapper;
using MeuMenuPedido.Domain.Services.Base;
using MeuMenuPedido.Domain.UoW;

namespace MeuMenuPedido.Application.AppServices.Base;

public abstract class BaseAppService
{
    private readonly IUnitOfWork _ouw;
    private readonly NegocioService _negocioService;
    protected readonly IMapper Mapper;

    protected BaseAppService(IUnitOfWork ouw, NegocioService negocioService, IMapper mapper)
    {
        _ouw = ouw;
        _negocioService = negocioService;
        Mapper = mapper;
    }

    protected async Task<int> Commit()
    {
        await _negocioService.ExecutarValidacao();
        if (!_negocioService.EhValido()) return 0;

        return await _ouw.Commit();
    }
}