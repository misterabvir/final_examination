using AutoMapper;
using Messenger.MessageManager.BusinessLogicalLayer.Services.Base;

namespace Messenger.MessageManager.BusinessLogicalLayer.Services;

public class MapperService : IMapperService
{
    private readonly IMapper _mapper;

    public MapperService(IMapper mapper)
    {
        _mapper = mapper;
    }

    public TTarget Map<TSource, TTarget>(TSource request)
    {
        return _mapper.Map<TSource, TTarget>(request);
    }
}