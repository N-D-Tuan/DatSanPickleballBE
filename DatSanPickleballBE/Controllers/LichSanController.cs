using DatSanPickleballBE.ModelDto;
using DatSanPickleballBE.ModelFromDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatSanPickleballBE.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LichSanController : ControllerBase
    {
        private readonly QuanLyDatSanPickleBall qly;

        public LichSanController(QuanLyDatSanPickleBall qly)
        {
            this.qly = qly;
        }

        [HttpGet]
        [Route("/LichSan/ListLichSan")]
        public async Task<ActionResult<IEnumerable<LichSanDto>>> GetAllLichSan()
        {
            var list = await qly.LichSans
                .Select(ls => new LichSanDto
                {
                    MaLichSan = ls.MaLichSan,
                    MaSan = ls.MaSan.GetValueOrDefault(),
                    MaKhungGio = ls.MaKhungGio.GetValueOrDefault(),
                    Ngay = ls.Ngay.GetValueOrDefault(),
                    IsBooked = ls.IsBooked.GetValueOrDefault()
                })
                .ToListAsync();

            return Ok(list);
        }

        [HttpGet]
        [Route("/LichSan/ListNgay")]
        public IActionResult GetNgay()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);

            var ngayList = qly.LichSans
                .Where(ls => ls.Ngay >= today)
                .Select(ls => ls.Ngay) // Lấy phần ngày, bỏ giờ
                .Distinct()
                .OrderBy(d => d)
                .ToList();

            if (ngayList == null || !ngayList.Any())
            {
                return NotFound(new { message = "Không có ngày nào phù hợp." });
            }

            return Ok(ngayList);
        }

        [HttpGet]
        [Route("/MaLichSan/{maLichSan}")]
        public async Task<ActionResult<LichSanDto>> GetLichSanById(int maLichSan)
        {
            var ls = await qly.LichSans.FirstOrDefaultAsync(l => l.MaLichSan == maLichSan);

            if (ls == null)
            {
                return NotFound();
            }

            var lichSanDto = new LichSanDto
            {
                MaLichSan = ls.MaLichSan,
                MaSan = ls.MaSan.GetValueOrDefault(),
                MaKhungGio = ls.MaKhungGio.GetValueOrDefault(),
                Ngay = ls.Ngay.GetValueOrDefault(),
                IsBooked = ls.IsBooked.GetValueOrDefault()
            };

            return Ok(lichSanDto);
        }
        [HttpGet]
        [Route("/LichSan/GetTimeSlots/{ngay}/{maSan}")]
        public IActionResult GetLichTheoNgayVaSan(DateTime ngay, int maSan)
        {
            var ngayDateOnly = DateOnly.FromDateTime(ngay);

            // Lấy tất cả khung giờ
            var allSlots = qly.KhungGios
                .Select(k => new TimeSlotDto
                {
                    MaKhungGio = k.MaKhungGio,
                    KhungGio = $"{k.GioBatDau:HH\\:mm} - {k.GioKetThuc:HH\\:mm}", // Format 24h
                    TrangThai = "Trống"
                })
                .ToList();

            // Lấy danh sách đã đặt cho ngày & sân được chọn
            var bookedSlots = qly.LichSans
                .Where(ls => ls.Ngay == ngayDateOnly && ls.MaSan == maSan)
                .Select(ls => new { ls.MaKhungGio, ls.IsBooked })
                .ToList();

            // Merge trạng thái
            foreach (var slot in allSlots)
            {
                var found = bookedSlots.FirstOrDefault(b => b.MaKhungGio == slot.MaKhungGio);
                if (found != null && found.IsBooked == true)
                {
                    slot.TrangThai = "Đã đặt";
                }
            }

            // Sắp xếp theo MaKhungGio tăng dần
            var sortedSlots = allSlots.OrderBy(s => s.MaKhungGio).ToList();

            return Ok(sortedSlots);
        }
    }
}
