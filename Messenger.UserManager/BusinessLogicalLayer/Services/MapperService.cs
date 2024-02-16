using AutoMapper;

namespace Messenger.UserManager.BusinessLogicalLayer.Services.Base;

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