using KhangNghi_BE.Payment.PaymentModels;

namespace KhangNghi_BE.Payment.PaymentServices
{
    public interface IVNPayService
    {
        string CreatePaymentUrl(VNPayPaymentInformationModel model, HttpContext context);
        VNPayPaymentResponseModel PaymentExecute(IQueryCollection collections);
    }
}
