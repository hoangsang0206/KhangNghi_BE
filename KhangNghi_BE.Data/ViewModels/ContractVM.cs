using System.ComponentModel.DataAnnotations;

namespace KhangNghi_BE.Data.ViewModels
{
    public class ContractVM
    {
        [Required(ErrorMessage = "Mã hợp đồng không được để trống")]
        [MaxLength(150, ErrorMessage = "Mã hợp đồng tối đa 150 ký tự")]
        public string ContractId { get; set; } = null!;

        public DateTime? CreatedDate { get; set; }

        public DateTime? SignedAt { get; set; }

        [Required(ErrorMessage = "Mã khách hàng không được để trống")]
        public string CustomerId { get; set; } = null!;

        public List<ContractDetailVM> ContractDetails { get; set; } = new List<ContractDetailVM>();


        public class ContractDetailVM
        {
            public string ProductId { get; set; } = null!;

            public string ServiceId { get; set; } = null!;

            public int Quantity { get; set; }

            public decimal UnitPrice { get; set; }
        }
    }
}
