namespace KhangNghi_BE.Payment.PaymentModels
{
    public class VNPayPaymentInformationModel
    {
        public string OrderId { get; set; } = null!;
        public string OrderType { get; set; } = null!;
        public double Amount { get; set; }
        public string OrderDescription { get; set; } = null!;
        public string Name { get; set; } = null!;
    }
}
