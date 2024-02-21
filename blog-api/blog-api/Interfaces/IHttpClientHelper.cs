
namespace blog_api;

public interface IHttpClientHelper
{
    Task<T?> GetAsync<T>(string baseUri, string endpoint);
}