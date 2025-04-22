namespace Web.Services;

public class AuthenticateService(SessionService sessionService, APIService apiService)
{
    private readonly SessionService SessionService = sessionService;
    private readonly APIService APIService = apiService;

    public bool IsRegularUser => SessionService.GetString("type-user") == "regular";
    public bool IsManagerUser => SessionService.GetString("type-user") == "manager";

    public async Task AuthenticateRegularAsync()
    {
        CleanSession();

        var response = await APIService.GetAsync("/account/login/regular");

        if (string.IsNullOrEmpty(response))
            return;

        SessionService.SetString("token", response);
        SessionService.SetString("type-user", "regular");
    }

    public async Task AuthenticateManagerAsync()
    {
        CleanSession();

        var response = await APIService.GetAsync("/account/login/manager");

        if (string.IsNullOrEmpty(response))
            return;

        SessionService.SetString("token", response);
        SessionService.SetString("type-user", "manager");
    }

    public void Logout()
    {
        SessionService.Remove("token");
    }

    private void CleanSession()
    {
        SessionService.Remove("token");
        SessionService.Remove("type-user");
    }
}