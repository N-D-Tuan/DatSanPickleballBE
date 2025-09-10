using DatSanPickleballBE.ModelDto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace DatSanPickleballBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VNPayController : ControllerBase
    {
        private readonly IConfiguration _config;

        public VNPayController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet("create-payment")]
        public IActionResult CreatePayment(string type = "booking")
        {
            var vnp_TmnCode = _config["VnPay:TmnCode"];
            var vnp_HashSecret = _config["VnPay:HashSecret"];
            var vnp_Url = _config["VnPay:PaymentUrl"];
            var baseReturnUrl = _config["VnPay:ReturnUrl"];

            //// phân biệt returnUrl theo type
            //string vnp_ReturnUrl = type switch
            //{
            //    "checkout" => $"{baseReturnUrl}/checkout?payment=success",
            //    _ => $"{baseReturnUrl}/booking?payment=success"
            //};

            string vnp_ReturnUrl = $"{baseReturnUrl}/api/VNPay/payment-return?type={type}";


            var vnpParams = new SortedDictionary<string, string>
    {
        { "vnp_Version", "2.1.0" },
        { "vnp_Command", "pay" },
        { "vnp_TmnCode", vnp_TmnCode },
        { "vnp_Amount", (100000 * 100).ToString() }, // 100k VND
        { "vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss") },
        { "vnp_CurrCode", "VND" },
        { "vnp_IpAddr", HttpContext.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1" },
        { "vnp_Locale", "vn" },
        { "vnp_OrderInfo", "Thanh toan" },
        { "vnp_OrderType", "other" },
        { "vnp_ReturnUrl", vnp_ReturnUrl },
        { "vnp_TxnRef", DateTime.Now.Ticks.ToString() }
    };

            // Build data for signature (URL-encode cả key và value)
            var signData = string.Join("&", vnpParams.Select(kvp =>
                $"{WebUtility.UrlEncode(kvp.Key)}={WebUtility.UrlEncode(kvp.Value)}"));

            // Tạo HMAC SHA512 từ HashSecret
            using var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(vnp_HashSecret));
            string secureHash = BitConverter.ToString(hmac.ComputeHash(Encoding.UTF8.GetBytes(signData)))
                .Replace("-", "").ToUpper();

            // Build query string cho URL gọi VNPay
            var queryString = signData + $"&vnp_SecureHash={secureHash}";

            string paymentUrl = $"{vnp_Url}?{queryString}";
            return Ok(new { paymentUrl });
        }

        [HttpGet("payment-return")]
        public IActionResult PaymentReturn(string type, [FromQuery] string vnp_ResponseCode, [FromQuery] string vnp_TxnRef)
        {
            if (vnp_ResponseCode == "00")
            {
                if (type == "booking")
                {
                    // Redirect về trang index của frontend
                    return Redirect("https://localhost:7279/?payment=success&type=booking");
                }
                else if (type == "checkout")
                {
                    // Redirect về trang shop của frontend
                    return Redirect("https://localhost:7279/Shop/CheckoutIndex?payment=success&type=checkout");
                }
            }
            else
            {
                if (type == "booking")
                {
                    // Redirect về trang index của frontend
                    return Redirect("https://localhost:7279");
                }
                else if (type == "checkout")
                {
                    // Redirect về trang shop của frontend
                    return Redirect("https://localhost:7279/Shop/CheckoutIndex");
                }
            }
            return BadRequest(new { Message = $"Thanh toán {type} thất bại", vnp_TxnRef });
        }

    }
}
