using DatSanPickleballBE.ModelDto;
using DatSanPickleballBE.ModelFromDB;
using DatSanPickleballBE.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Generators;

namespace DatSanPickleballBE.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        QuanLyDatSanPickleBall qly;
        private readonly EmailService _emailService;
        public UserController(QuanLyDatSanPickleBall qly, EmailService emailService)
        {
            this.qly = qly;
            _emailService = emailService;
        }

        [HttpGet]
        [Route("/All")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
        {
            var users = await qly.Users
                .Select(u => new UserDto
                {
                    maNguoiDung = u.MaNguoiDung,
                    tenNguoiDung = u.TenNguoiDung,
                    email = u.Email,
                    soDienThoai = u.SoDienThoai,
                    matKhau = u.MatKhau,
                    role = u.Role
                }).ToListAsync();

            return Ok(users);
        }

        [HttpGet]
        [Route("/User/{tenNguoiDung}")]
        public IActionResult GetUserByTenNguoiDung(string tenNguoiDung)
        {
            if (string.IsNullOrEmpty(tenNguoiDung))
                return BadRequest("Vui lòng nhập tên người dùng cần tìm.");

            var users = qly.Users
                .Where(u => EF.Functions.Like(u.TenNguoiDung.ToLower(), $"%{tenNguoiDung.ToLower()}%"))
                .Select(u => new UserDto
                {
                    maNguoiDung = u.MaNguoiDung,
                    tenNguoiDung = u.TenNguoiDung,
                    email = u.Email,
                    soDienThoai = u.SoDienThoai,
                    role = u.Role
                })
                .ToList();

            if (users == null || users.Count == 0)
                return NotFound("Không tìm thấy người dùng phù hợp.");

            return Ok(users);
        }
        //[HttpGet]
        //[Route("/User/TenNguoiDung/{email}")]
        //public IActionResult GetUserByEmail(string email)
        //{
        //    if (string.IsNullOrEmpty(email))
        //        return BadRequest("Vui lòng nhập email cần tìm.");

        //    var users = qly.Users
        //        .Where(u => EF.Functions.Like(u.Email.ToLower(), $"%{email.ToLower()}%"))
        //        .Select(u => new UserDto
        //        {
        //            maNguoiDung = u.MaNguoiDung,
        //            tenNguoiDung = u.TenNguoiDung,
        //            email = u.Email,
        //            soDienThoai = u.SoDienThoai,
        //            matKhau = u.MatKhau,
        //            role = u.Role
        //        })
        //        .ToList().FirstOrDefault();

        //    if (users == null)
        //        return NotFound("Không tìm thấy người dùng phù hợp với email.");

        //    return Ok(users);
        //}

        [HttpPost]
        [Route("/User/Login")]
        public IActionResult Login([FromBody] LoginDto model)
        {
            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
                return BadRequest("Vui lòng nhập email và mật khẩu.");

            var user = qly.Users.FirstOrDefault(u => u.Email.ToLower() == model.Email.ToLower());
            if (user == null)
                return NotFound("Email không tồn tại.");

            // Kiểm tra password với hash
            bool isValid = PasswordHasher.Verify(model.Password, user.MatKhau);
            if (!isValid)
                return Unauthorized("Sai mật khẩu.");

            // Trả về user DTO (không trả mật khẩu gốc)
            var dto = new UserDto
            {
                maNguoiDung = user.MaNguoiDung,
                tenNguoiDung = user.TenNguoiDung,
                email = user.Email,
                soDienThoai = user.SoDienThoai,
                role = user.Role
            };

            return Ok(dto);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto userDto)
        {
            try
            {
                // Kiểm tra đầu vào
                if (userDto == null || string.IsNullOrWhiteSpace(userDto.tenNguoiDung) ||
                    string.IsNullOrWhiteSpace(userDto.email) ||
                    string.IsNullOrWhiteSpace(userDto.soDienThoai) ||
                    string.IsNullOrWhiteSpace(userDto.matKhau))
                {
                    return BadRequest("Thông tin người dùng không hợp lệ.");
                }

                // Gán mặc định nếu role không có hoặc để trống
                var role = string.IsNullOrWhiteSpace(userDto.role) ? "NguoiDung" : userDto.role;

                // Hash mật khẩu trước khi lưu
                string hashedPassword = PasswordHasher.HashPassword(userDto.matKhau);

                // Tạo câu truy vấn SQL
                string sql = @"
            INSERT INTO [User] (tenNguoiDung, email, soDienThoai, matKhau, role)
            VALUES (@tenNguoiDung, @email, @soDienThoai, @matKhau, @role)
        ";

                // Thực thi câu lệnh
                int rows = await qly.Database.ExecuteSqlRawAsync(sql,
                    new SqlParameter("@tenNguoiDung", userDto.tenNguoiDung),
                    new SqlParameter("@email", userDto.email),
                    new SqlParameter("@soDienThoai", userDto.soDienThoai),
                    new SqlParameter("@matKhau", hashedPassword),
                    new SqlParameter("@role", role)
                );

                // Kiểm tra kết quả
                if (rows > 0)
                    return Ok("Thêm người dùng thành công.");
                else
                    return StatusCode(500, "Thêm người dùng thất bại.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }
        [HttpPut("{maNguoiDung}")]
        public IActionResult UpdateUserByMaNguoiDung(int maNguoiDung, [FromBody] UpdateUserDto updatedUser)
        {
            try
            {
                var user = qly.Users.FirstOrDefault(u => u.MaNguoiDung == maNguoiDung);

                if (user == null)
                {
                    return NotFound("Không tìm thấy người dùng.");
                }

                // Cập nhật thông tin người dùng
                user.TenNguoiDung = updatedUser.tenNguoiDung ?? user.TenNguoiDung;
                user.Email = updatedUser.email ?? user.Email;
                user.SoDienThoai = updatedUser.soDienThoai ?? user.SoDienThoai;
                user.Role = string.IsNullOrWhiteSpace(updatedUser.role) ? "NguoiDung" : updatedUser.role;

                qly.SaveChanges();

                return Ok("Cập nhật người dùng thành công.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi: {ex.Message}");
            }
        }

        [HttpDelete("{maNguoiDung}")]
        public IActionResult DeleteUserByMaNguoiDung(int maNguoiDung)
        {
            try
            {
                var user = qly.Users.FirstOrDefault(u => u.MaNguoiDung == maNguoiDung);

                if (user == null)
                {
                    return NotFound("Không tìm thấy người dùng.");
                }

                qly.Users.Remove(user);
                qly.SaveChanges();

                return Ok("Xóa người dùng thành công.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi: {ex.Message}");
            }
        }

        //[HttpPut("UpdatePasswordByEmail/{email}")]
        //public IActionResult UpdatePasswordByEmail(string email, [FromBody] string newPassword)
        //{
        //    try
        //    {
        //        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(newPassword))
        //        {
        //            return BadRequest("Email và mật khẩu mới không được để trống.");
        //        }

        //        var user = qly.Users.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());

        //        if (user == null)
        //        {
        //            return NotFound("Không tìm thấy người dùng với email đã cho.");
        //        }

        //        // Cập nhật mật khẩu
        //        user.MatKhau = newPassword;
        //        qly.SaveChanges();

        //        return Ok("Cập nhật mật khẩu thành công.");
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Lỗi: {ex.Message}");
        //    }
        //}

        [HttpPost("reset-password")]
        public async Task<IActionResult> RequestPasswordReset([FromBody] string email)
        {
            if (string.IsNullOrEmpty(email))
                return BadRequest("Email không được để trống");

            var user = await qly.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                return NotFound("Không tìm thấy người dùng với email này");

            // Tạo mã OTP ngẫu nhiên 6 số
            var random = new Random();
            var otp = random.Next(100000, 999999).ToString();

            // Lưu OTP và thời hạn hết hạn
            user.ResetOtp = otp;
            user.ResetOtpExpiry = DateTime.Now.AddMinutes(10); // OTP hết hạn sau 10 phút
            await qly.SaveChangesAsync();

            // Gửi email chứa OTP
            await _emailService.SendEmailAsync(email, "Mã OTP đặt lại mật khẩu",
                $"Mã OTP của bạn là: {otp} (có hiệu lực trong 10 phút)");

            return Ok("Mã OTP đã được gửi đến email của bạn");
        }


        [HttpPut("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto request)
        {
            if (string.IsNullOrEmpty(request.Otp) || string.IsNullOrEmpty(request.matKhau))
                return BadRequest("Thiếu OTP hoặc mật khẩu mới");

            // Kiểm tra OTP
            var user = await qly.Users
                .FirstOrDefaultAsync(u => u.ResetOtp == request.Otp && u.ResetOtpExpiry > DateTime.Now);

            if (user == null)
                return BadRequest("OTP không hợp lệ hoặc đã hết hạn");

            // Hash mật khẩu mới
            user.MatKhau = PasswordHasher.HashPassword(request.matKhau);

            // Xóa OTP sau khi dùng
            user.ResetOtp = null;
            user.ResetOtpExpiry = null;

            // Lưu thay đổi vào DB
            await qly.SaveChangesAsync();

            return Ok("Đổi mật khẩu thành công");
        }     
    }
}
