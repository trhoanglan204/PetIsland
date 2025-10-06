using PetIsland.Models.Momo;
using PetIsland.Models.Paypal;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace PetIslandWeb.Services.Paypal;

public sealed class PaypalService : IPaypalService
{
    public string? Mode { get; }
    public string? ClientId { get; }
    public string? ClientSecret { get; }
    public string? BaseUrl { get; }

    public PaypalService(IConfiguration configuration)
    {
        Mode = configuration["PaypalSettings:Mode"];
        ClientId = configuration["PaypalSettings:ClientId"];
        ClientSecret = configuration["PaypalSettings:ClientSecret"];
        BaseUrl = Mode == "live"
            ? configuration["PaypalSettings:UrlLive"]
            : configuration["PaypalSettings:UrlSandBox"];
    }

    public string? GetClientId()
    {
        return ClientId;
    }

    public async Task<AuthResponseModel?> Authenticate()
    {
        var auth = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{ClientId}:{ClientSecret}"));

        var content = new List<KeyValuePair<string, string>>
        {
            new("grant_type","client_credentials")
        };

        var request = new HttpRequestMessage 
        { 
            Method = HttpMethod.Post, 
            RequestUri = new Uri($"{BaseUrl}/v1/oauth2/token"),
            Headers = 
            {
                {"Authorization", $"Basic {auth}" }
            },
            Content = new FormUrlEncodedContent(content)
        };

        var httpClient = new HttpClient();
        var httpResponse = await httpClient.SendAsync(request);
        var jsonResponse = await httpResponse.Content.ReadAsStringAsync();
        var response = JsonSerializer.Deserialize<AuthResponseModel>(jsonResponse);

        return response;
    }

    public async Task<CreateOrderResponse?> CreateOrder(string value, string currentcy, string reference)
    {
        var auth = await Authenticate();

        var request = new CreateOrderRequest
        { 
            intent = "CAPTURE",
            purchase_units =
            [
                new()
                {
                    reference_id = reference,
                    amount = new Amount
                    {
                        currency_code = currentcy,
                        value = value
                    }
                }
            ]
        };

        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"Bearer {auth.access_token}");

        var httpResponse = await httpClient.PostAsJsonAsync($"{BaseUrl}/v2/checkout/orders", request);
        var jsonResponse = await httpResponse.Content.ReadAsStringAsync();
        var response = JsonSerializer.Deserialize<CreateOrderResponse>(jsonResponse);

        return response;
    }

    public async Task<CaptureOrderResponse?> CaptureOrder(string orderId)
    {
        var auth = await Authenticate();

        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"Bearer {auth.access_token}");

        var httpContent = new StringContent("", Encoding.Default, "application/json");
        var httpResponse = await httpClient.PostAsync($"{BaseUrl}/v2/checkout/orders/{orderId}/capture", httpContent);
        var jsonResponse = await httpResponse.Content.ReadAsStringAsync();
        var response = JsonSerializer.Deserialize<CaptureOrderResponse>(jsonResponse);

        return response;
    }
}
