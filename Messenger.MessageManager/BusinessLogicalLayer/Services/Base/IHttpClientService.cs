namespace Messenger.MessageManager.BusinessLogicalLayer.Services.Base;

public interface IHttpClientService
{
    Task<TResponse?> Post<TRequest, TResponse>(string url, TRequest request);
}
