using DatSanPickleballBE.ModelDto;
using DatSanPickleballBE.ModelFromDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace DatSanPickleballBE.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        QuanLyDatSanPickleBall qly;
        public UserController(QuanLyDatSanPickleBall qly)
        {
            this.qly = qly;
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
                    matKhau = u.MatKhau,
                    role = u.Role
                })
                .ToList();

            if (users == null || users.Count == 0)
                return NotFound("Không tìm thấy người dùng phù hợp.");

            return Ok(users);
        }
        [HttpGet]
        [Route("/User/TenNguoiDung/{email}")]
        public IActionResult GetUserByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return BadRequest("Vui lòng nhập email cần tìm.");

            var users = qly.Users
                .Where(u => EF.Functions.Like(u.Email.ToLower(), $"%{email.ToLower()}%"))
                .Select(u => new UserDto
                {
                    maNguoiDung = u.MaNguoiDung,
                    tenNguoiDung = u.TenNguoiDung,
                    email = u.Email,
                    soDienThoai = u.SoDienThoai,
                    matKhau = u.MatKhau,
                    role = u.Role
                })
                .ToList().FirstOrDefault();

            if (users == null)
                return NotFound("Không tìm thấy người dùng phù hợp với email.");

            return Ok(users);
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
                    new SqlParameter("@matKhau", userDto.matKhau),
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
        public IActionResult UpdateUserByMaNguoiDung(int maNguoiDung, [FromBody] CreateUserDto updatedUser)
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
                user.MatKhau = updatedUser.matKhau ?? user.MatKhau;
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
        [HttpPut("UpdatePasswordByEmail/{email}")]
        public IActionResult UpdatePasswordByEmail(string email, [FromBody] string newPassword)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(newPassword))
                {
                    return BadRequest("Email và mật khẩu mới không được để trống.");
                }

                var user = qly.Users.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());

                if (user == null)
                {
                    return NotFound("Không tìm thấy người dùng với email đã cho.");
                }

                // Cập nhật mật khẩu
                user.MatKhau = newPassword;
                qly.SaveChanges();

                return Ok("Cập nhật mật khẩu thành công.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi: {ex.Message}");
            }
        }

    }
}
