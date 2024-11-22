namespace KhangNghi_BE.Data.ViewModels
{
    public class PromotionVM
    {
        public string PromotionName { get; set; } = null!;

        public string PromotionType { get; set; } = null!;

        public decimal? DiscountAmount { get; set; }

        public decimal? MaxDiscountAmount { get; set; }

        public DateOnly StartDate { get; set; }

        public DateOnly EndDate { get; set; }

        //List product id
        public string[]? PromotionProducts { get; set; }
    }
}
