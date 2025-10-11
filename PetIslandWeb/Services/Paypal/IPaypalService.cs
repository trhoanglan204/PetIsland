using PetIsland.Models.Paypal;

namespace PetIslandWeb.Services.Paypal;

public interface IPaypalService
{
    string? GetClientId();
    Task<AuthResponseModel?> Authenticate();

    Task<CreateOrderResponse?> CreateOrder(string value, string currentcy, string reference);

    Task<CaptureOrderResponse?> CaptureOrder(string orderId);
}
