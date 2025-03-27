using PetIsland.Models;
using PetIsland.Models.Momo;

namespace PetIslandWeb.Services.Momo;

public interface IMomoService
{
	Task<MomoCreatePaymentResponseModel> CreatePaymentAsync(OrderInfo model);
	MomoExecuteResponseModel PaymentExecuteAsync(IQueryCollection collection);
}
