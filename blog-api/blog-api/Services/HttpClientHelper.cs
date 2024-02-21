
using Newtonsoft.Json;

namespace blog_api;

public class HttpClientHelper : IHttpClientHelper
{
    private readonly IHttpClientFactory httpClient;

    public HttpClientHelper(IHttpClientFactory httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<T?> GetAsync<T>(string baseUri, string endpoint)
    {
        var client = httpClient.CreateClient();

        try
        {
            var response = await client.GetAsync($"{baseUri}/{endpoint}");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<T>(json);
            }
        }
        catch
        {
            throw new Exception("Please, check your internet connection");
        }

        return default;
    }
}
