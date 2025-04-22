using System.Net.Http.Headers;

namespace Web.Services;

public class APIService(HttpClient httpClient, SessionService sessionService)
{
    private readonly HttpClient HttpClient = httpClient;
    private readonly SessionService SessionService = sessionService;

    public const string accountEndpointBase = "account/";

    private bool AddAuthorizationHeader()
    {
        var token = SessionService.GetString("token");
        if (!string.IsNullOrEmpty(token))
        {
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return true;
        }

        return false;
    }

    public async Task<T?> GetAsync<T>(string endpoint)
    {
        var existsHeader = AddAuthorizationHeader();

        if (endpoint.Contains(accountEndpointBase))
            return await HttpClient.GetFromJsonAsync<T>(endpoint);

        if (existsHeader)
            return await HttpClient.GetFromJsonAsync<T>(endpoint);

        return default;
    }

    public async Task<string> GetAsync(string endpoint)
    {
        var existsHeader = AddAuthorizationHeader();

        if (endpoint.Contains(accountEndpointBase))
            return await HttpClient.GetStringAsync(endpoint);

        if (existsHeader)
            return await HttpClient.GetStringAsync(endpoint);

        return string.Empty;
    }

    public async Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest data)
    {
        AddAuthorizationHeader();
        var response = await HttpClient.PostAsJsonAsync(endpoint, data);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TResponse>();
    }

    public async Task PutAsync<T>(string endpoint, T data)
    {
        var existsHeader = AddAuthorizationHeader();
        if (!existsHeader)
            return;

        var response = await HttpClient.PutAsJsonAsync(endpoint, data);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(string endpoint)
    {
        var existsHeader = AddAuthorizationHeader();
        if (!existsHeader)
            return;

        var response = await HttpClient.DeleteAsync(endpoint);
        response.EnsureSuccessStatusCode();
    }
}