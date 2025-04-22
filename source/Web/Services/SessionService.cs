namespace Web.Services;

public class SessionService(IHttpContextAccessor httpContextAccessor)
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public void SetString(string key, string value)
    {
        _httpContextAccessor?.HttpContext?.Session.SetString(key, value);
    }

    public string? GetString(string key)
    {
        return _httpContextAccessor?.HttpContext?.Session.GetString(key);
    }

    public void Remove(string key)
    {
        _httpContextAccessor?.HttpContext?.Session.Remove(key);
    }
}
