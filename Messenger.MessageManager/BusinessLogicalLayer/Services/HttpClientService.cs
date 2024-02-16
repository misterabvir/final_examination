using System.Text.Json;
using Messenger.MessageManager.BusinessLogicalLayer.Services.Base;

namespace Messenger.MessageManager.BusinessLogicalLayer.Services;

public class HttpClientService : IHttpClientService
{
    private readonly HttpClient _httpClient;

    public HttpClientService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>
    /// Posts data to the specified URL and returns the deserialized response.
    /// </summary>
    /// <typeparam name="T">The type of the request data</typeparam>
    /// <typeparam name="E">The type of the response data</typeparam>
    /// <param name="url">The URL to post the data to</param>
    /// <param name="request">The data to be posted</param>
    /// <returns>The deserialized response data</returns>
    public async Task<E?> Post<T, E>(string url, T request)
    {
        var response = await _httpClient.PostAsJsonAsync(url, request);
        return await Response<E>(response);
    }

    /// <summary>
    /// Parses the content of the HTTP response message and deserializes it to the specified type.
    /// </summary>
    /// <typeparam name="T">The type to deserialize the content to.</typeparam>
    /// <param name="response">The HTTP response message to parse and deserialize.</param>
    /// <returns>The deserialized content of the HTTP response message.</returns>
    private static async Task<T?> Response<T>(HttpResponseMessage response)
    {
        var json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<T>(json);
        return result;
    }
}