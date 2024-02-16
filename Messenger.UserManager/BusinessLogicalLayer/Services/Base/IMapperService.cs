namespace Messenger.UserManager.BusinessLogicalLayer.Services.Base;

public interface IMapperService
{
    TTarget Map<TSource, TTarget>(TSource request);
}
