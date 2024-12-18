﻿using KhangNghi_BE.Contants;
using KhangNghi_BE.Data;
using KhangNghi_BE.Data.Models;
using KhangNghi_BE.Data.ViewModels;
using KhangNghi_BE.Filters;
using KhangNghi_BE.Payment.PaymentServices;
using KhangNghi_BE.Services;
using Microsoft.AspNetCore.Mvc;
using KhangNghi_BE.Payment.PaymentModels;

namespace KhangNghi_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoicesController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IVNPayService _vnpayService;
        private readonly IContractService _contractService;
        private readonly IPromotionService _promotionService;
        private readonly int _pageSize = 20;

        public InvoicesController(IInvoiceService invoiceService, 
            IContractService contractService,
            IPromotionService promotionService,
            IVNPayService vNPayService)
        {
            _invoiceService = invoiceService;
            _contractService = contractService;
            _promotionService = promotionService;
            _vnpayService = vNPayService;
        }

        [HttpGet]
        [AdminAuthorize(Code = Functions.ViewInvoices)]
        public async Task<IActionResult> GetInvoices(string? sortBy, string? filterBy, int page = 1)
        {
            PagedList<Invoice> invoices = await _invoiceService.GetAsync(filterBy, sortBy, page, _pageSize);

            return Ok(new ApiResponse
            {
                Success = true,
                Data = invoices
            });
        }

        [HttpGet("by-contract")]
        [AdminAuthorize(Code = Functions.ViewInvoices)]
        public async Task<IActionResult> GetInvoiceByContractId(string cId)
        {
            Invoice? invoice = await _invoiceService.GetByContractId(cId);

            if (invoice == null)
            {
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Message = "Không tìm thấy hóa đơn cho hợp đồng này"
                });
            }

            return Ok(new ApiResponse
            {
                Success = true,
                Data = invoice
            });
        }

        [HttpGet("by-id")]
        [AdminAuthorize(Code = Functions.ViewInvoices)]
        public async Task<IActionResult> GetInvoiceById(string id)
        {
            Invoice? invoice = await _invoiceService.GetByIdAsync(id);

            if (invoice == null)
            {
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Message = "Không tìm thấy hóa đơn"
                });
            }

            return Ok(new ApiResponse
            {
                Success = true,
                Data = invoice
            });
        }

        [HttpPost("create")]
        [AdminAuthorize(Code = Functions.CreateInvoice)]
        public async Task<IActionResult> CreateInvoice([FromBody] InvoiceVM invoice)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Dữ liệu không hợp lệ",
                    Data = ModelState
                });
            }

            Contract? contract = await _contractService.GetByIdAsync(invoice.ContractId);

            if (contract == null)
            {
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Message = "Không tìm thấy hợp đồng"
                });
            }

            //Check promotions
            if (invoice.PromotionIds != null)
            {
                List<string> inActiveIds = new List<string>();
                foreach (string id in invoice.PromotionIds)
                {
                    if (!await _promotionService.CheckIsActive(id))
                    {
                        inActiveIds.Add(id);
                    }
                }

                if (inActiveIds.Count > 0)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Message = $"Không thể tạo hóa đơn với các khuyến mãi không còn hiệu lực",
                        Data = new
                        {
                            expiredPromotionIds = inActiveIds
                        }
                    });
                }
            }


            string invoiceId = $"HOADON-{DateTime.Now.ToString("yyyyMMddHHmm")}";
            if (await _invoiceService.CreateAsync(invoice, invoiceId))
            {
                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Tạo hóa đơn thành công",
                    Data = await _invoiceService.GetByIdAsync(invoiceId)
                });
            }

            return BadRequest(new ApiResponse
            {
                Success = false,
                Message = "Tạo hóa đơn thất bại"
            });
        }

        [HttpPost("create-payment")]
        [AdminAuthorize(Code = Functions.CreateInvoice)]
        public async Task<IActionResult> CreatePayment([FromQuery] string invoiceId)
        {
            Invoice? invoice = await _invoiceService.GetByIdAsync(invoiceId);
            if (invoice == null)
            {
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Message = "Không tìm thấy hóa đơn"
                });
            }

            var model = new VNPayPaymentInformationModel
            {
                OrderId = invoice.InvoiceId,
                Amount = (double)invoice.TotalAmout,
                Name = "Hóa đơn " + invoice.InvoiceId,
                OrderDescription = "Thanh toan hoa don: " + invoice.InvoiceId,
                OrderType = "other"
            };

            var url = _vnpayService.CreatePaymentUrl(model, HttpContext);
            
            return Ok(new ApiResponse
            {
                Success = true,
                Data = new 
                {
                    PaymentUrl = url
                }
            });
        }

        [HttpPut("paid")]
        [AdminAuthorize(Code = Functions.UpdateInvoice)]
        public async Task<IActionResult> SetPaid(string id)
        {
            Invoice? invoice = await _invoiceService.GetByIdAsync(id);

            if (invoice == null)
            {
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Message = "Không tìm thấy hóa đơn"
                });
            }

            if (await _invoiceService.PaidAsync(id))
            {
                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Đã xác nhận thanh toán cho hóa đơnn này"
                });
            }

            return BadRequest(new ApiResponse
            {
                Success = false,
                Message = "Xác nhận thanh toán hóa đơn thất bại"
            });
        }

        [HttpDelete]
        [AdminAuthorize(Code = Functions.DeleteInvoice)]
        public async Task<IActionResult> DeleteInvoice(string id)
        {
            Invoice? invoice = await _invoiceService.GetByIdAsync(id);

            if (invoice == null)
            {
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Message = "Không tìm thấy hóa đơn"
                });
            }

            if(invoice.PaidDate != null)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Không thể xóa hóa đơn đã được thanh toán"
                });
            }

            if (await _invoiceService.DeleteAsync(id))
            {
                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Xóa hóa đơn thành công"
                });
            }

            return BadRequest(new ApiResponse
            {
                Success = false,
                Message = "Xóa hóa đơn thất bại"
            });
        }
    }
}
