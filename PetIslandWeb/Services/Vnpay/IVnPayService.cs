using PetIsland.Models.Vnpay;

namespace PetIslandWeb.Services.Vnpay;

public interface IVnPayService
{
	string CreatePaymentUrl(PaymentInformationModel model, HttpContext context);
	PaymentResponseModel PaymentExecute(IQueryCollection collections);

}
