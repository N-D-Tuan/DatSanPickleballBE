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
        public IActionResult CreatePayment()
        {
            var vnp_TmnCode = _config["VnPay:TmnCode"];
            var vnp_HashSecret = _config["VnPay:HashSecret"];
            var vnp_Url = _config["VnPay:PaymentUrl"];
            var vnp_ReturnUrl = _config["VnPay:ReturnUrl"];

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
        { "vnp_OrderInfo", "Thanh toan dat san" },
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

        [HttpGet("return")]
        public IActionResult PaymentReturn()
        {
            var vnp_HashSecret = _config["VnPay:HashSecret"];

            // Bỏ vnp_SecureHash và vnp_SecureHashType
            var vnpData = Request.Query
                .Where(kvp => kvp.Key != "vnp_SecureHash" && kvp.Key != "vnp_SecureHashType")
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToString());

            // Sắp xếp theo key tăng dần
            var sortedData = new SortedDictionary<string, string>(vnpData);

            // Ghép chuỗi key=value và encode UTF-8
            var signData = string.Join("&", sortedData.Select(kvp =>
                $"{WebUtility.UrlEncode(kvp.Key)}={WebUtility.UrlEncode(kvp.Value)}"));

            // Tạo hash
            using var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(vnp_HashSecret));
            var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(signData));
            var calculatedHash = BitConverter.ToString(hashBytes).Replace("-", "").ToUpper();

            var vnp_SecureHash = Request.Query["vnp_SecureHash"].ToString();
            var responseCode = Request.Query["vnp_ResponseCode"].ToString();

            if (calculatedHash == vnp_SecureHash)
            {
                return Content("ok"); // => success modal
            }
            else
            {
                return Content("invalid signature");
            }
        }

    }
}
