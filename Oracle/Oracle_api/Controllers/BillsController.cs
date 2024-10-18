using System.Collections;

namespace Oracle_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BillsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Bills
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Hoadon>>> GetHoadons(int pageIndex = 1, int pageSize = 20)
        {
            pageIndex = pageIndex <= 1 ? 1 : pageIndex;
            pageSize = pageSize <= 20 ? 20 : pageSize;
            var hoadon = await _context.Hoadons.OrderByDescending(h => h.Thoigiantao)
                                               .Skip((pageIndex - 1) * pageSize)
                                               .Take(pageSize)
                                               .ToListAsync();
            
            if(hoadon == null || !hoadon.Any())
            {
                return NotFound("Không tìm thấy hóa đơn nào");
            }
            return Ok(hoadon);
        }

        // PUT: api/Bills/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHoadon(int id, [FromBody] HoaDonViewModel hoadonViewModel)
        {
            if (hoadonViewModel == null || !hoadonViewModel.ChiTietHoaDonViewModels.Any())
            {
                return BadRequest("Hóa đơn phải có ít nhất một chi tiết hóa đơn.");
            }
            var hoadon = await _context.Hoadons.Include(h => h.Chitiethoadons)
                                               .FirstOrDefaultAsync(h => h.Hoadonid == id);
            if (hoadon == null)
            {
                return NotFound("Hóa đơn không tồn tại.");
            }
            var khachhang = await _context.Khachhangs.FindAsync(hoadonViewModel.Khachhangid);
            if (khachhang == null)
            {
                return BadRequest("Khách hàng không tồn tại.");
            }
            if (string.IsNullOrWhiteSpace(hoadonViewModel.Tenhoadon))
            {
                return BadRequest("Tên hóa đơn không để trống");
            }
            hoadon.Khachhangid         = hoadonViewModel.Khachhangid;
            hoadon.Tenhoadon           = hoadonViewModel.Tenhoadon;
            hoadon.Ghichu              = hoadonViewModel.Ghichu;
            hoadon.Thoigiancapnhat     = DateTime.Now;
            double tongTien            = 0;
            foreach (var cthdViewModel in hoadonViewModel.ChiTietHoaDonViewModels)
            {
                var sanpham = await _context.Sanphams.FindAsync(cthdViewModel.Sanphamid);
                if (sanpham == null)
                {
                    return BadRequest("Sản phẩm không tồn tại.");
                }
                if (cthdViewModel.Soluong <= 0)
                {
                    cthdViewModel.Soluong = 1; 
                }
                if (string.IsNullOrWhiteSpace(cthdViewModel.Dvt))
                {
                    return BadRequest("Đơn vị tính không để trống");
                }
                var thanhtien = cthdViewModel.Soluong * sanpham.Giathanh ?? 0;
                var cthd      = hoadon.Chitiethoadons.FirstOrDefault(c => c.Sanphamid == cthdViewModel.Sanphamid);

                if (cthd != null)
                {
                    cthd.Soluong   = cthdViewModel.Soluong;
                    cthd.Thanhtien = thanhtien;
                    cthd.Dvt       = cthdViewModel.Dvt;
                }
                else
                {
                    var newCthd = new Chitiethoadon
                    {
                        Sanphamid = cthdViewModel.Sanphamid,
                        Soluong   = cthdViewModel.Soluong,
                        Thanhtien = thanhtien,
                        Dvt       = cthdViewModel.Dvt,
                        Hoadonid  = hoadon.Hoadonid
                    };
                    hoadon.Chitiethoadons.Add(newCthd);
                }
                tongTien += thanhtien;
            }
            var cthdToRemove = hoadon.Chitiethoadons.Where(ct => !hoadonViewModel.ChiTietHoaDonViewModels
                                                    .Any(cthdVm => cthdVm.Sanphamid == ct.Sanphamid))
                                                    .ToList();
            foreach (var r in cthdToRemove)
            {
                _context.Chitiethoadons.Remove(r);
            }
            hoadon.Tongtien = tongTien;
            _context.Hoadons.Update(hoadon);
            await _context.SaveChangesAsync();

            return Ok(hoadon);
        }

        // POST: api/Bills
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostHoadon([FromBody] HoaDonViewModel hoadonViewModel)
        {
            if (hoadonViewModel == null || !hoadonViewModel.ChiTietHoaDonViewModels.Any())
            {
                return BadRequest("Hóa đơn phải có ít nhất một chi tiết hóa đơn.");
            }
            var khachhang = await _context.Sanphams.FindAsync(hoadonViewModel.Khachhangid);
            if (khachhang == null)
            {
                return BadRequest("Khách hàng không tồn tại.");
            }

            if (string.IsNullOrWhiteSpace(hoadonViewModel.Tenhoadon))
            {
                return BadRequest("Tên hóa đơn không để trống");
            }

            var hoadon = new Hoadon
            {
                Khachhangid = hoadonViewModel.Khachhangid,
                Tenhoadon   = hoadonViewModel.Tenhoadon,
                Ghichu      = hoadonViewModel.Ghichu,
                Thoigiantao = DateTime.Now,
                Tongtien    = 0 
            };
            var lastHoadon             = await _context.Hoadons.OrderByDescending(h => h.Hoadonid)
                                                               .FirstOrDefaultAsync();
            var newHoadonId            = lastHoadon != null ? (int)lastHoadon.Hoadonid + 1 : 1;
            var currentDate            = DateTime.Now.ToString("yyyyMMdd");
            hoadon.Magiaodich          = $"{currentDate}_{newHoadonId:D3}";
            double tongTien            = 0;
            foreach (var cthdViewModel in hoadonViewModel.ChiTietHoaDonViewModels)
            {
                var sanpham = await _context.Sanphams.FindAsync(cthdViewModel.Sanphamid);
                if (sanpham == null)
                {
                    return BadRequest("Sản phẩm không tồn tại.");
                }

                if (cthdViewModel.Soluong <= 0)
                {
                    cthdViewModel.Soluong = 1;
                }

                if (string.IsNullOrWhiteSpace(cthdViewModel.Dvt))
                {
                    return BadRequest("Đơn vị tính không để trống");
                }

                var thanhtien = cthdViewModel.Soluong * sanpham.Giathanh ?? 0;
                var cthd      = new Chitiethoadon
                {
                    Sanphamid = cthdViewModel.Sanphamid,
                    Soluong   = cthdViewModel.Soluong, 
                    Thanhtien = thanhtien, 
                    Dvt       = cthdViewModel.Dvt, 
                    Hoadon    = hoadon 
                };
                tongTien += thanhtien;
                hoadon.Chitiethoadons.Add(cthd);
            }
            hoadon.Tongtien = tongTien;
            _context.Hoadons.Add(hoadon);
            await _context.SaveChangesAsync();

            return Ok(hoadon);
        }

        // DELETE: api/Bills/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHoadon(int id)
        { 
            var hoadon = await _context.Hoadons.Include(h => h.Chitiethoadons)
                                               .FirstOrDefaultAsync(h => h.Hoadonid == id);
            if (hoadon == null)
            {
                return NotFound("Hóa đơn không tồn tại.");
            }
            if (hoadon.Chitiethoadons.Any())
            {
                _context.Chitiethoadons.RemoveRange(hoadon.Chitiethoadons);
            }
            _context.Hoadons.Remove(hoadon);
            await _context.SaveChangesAsync();
            return Ok("Hóa đơn và các chi tiết liên quan đã được xóa thành công.");
        }

        [HttpGet("FilterYearMonth")]
        public async Task<IActionResult> FilterYearMonth([FromQuery] int year, [FromQuery] int month, int pageIndex = 1, int pageSize = 20)
        {
            pageIndex = pageIndex <= 1 ? 1 : pageIndex;
            pageSize = pageSize <= 20 ? 20 : pageSize;
            if (year <= 0 || month <= 0) 
            {
                return BadRequest("Vui lòng nhập năm và tháng vào chỗ trống");
            }
            var hoadon = await _context.Hoadons.Where(h => h.Thoigiantao.Value.Year == year && h.Thoigiantao.Value.Month == month)
                                               .Skip((pageIndex - 1) * pageSize)
                                               .Take(pageSize)
                                               .ToListAsync();                 
            if (!hoadon.Any())
            {
                return NotFound("Không tìm thấy hóa đơn nào.");
            }
            return Ok(hoadon);
        }

        [HttpGet("FilterDay")]
        public async Task<IActionResult> FilterDay([FromQuery] int fromDate, [FromQuery] int toDate, int pageIndex = 1, int pageSize = 20)
        {
            pageIndex = pageIndex <= 1 ? 1 : pageIndex;
            pageSize = pageSize <= 20 ? 20 : pageSize;
            if (fromDate <=0 || toDate <= 0)
            {
                return BadRequest("Vui lòng điền ngày vào chỗ trống");
            }
            var hoadon = await _context.Hoadons.Where(h => h.Thoigiantao.Value.Day >= fromDate && h.Thoigiantao.Value.Day <= toDate)
                                               .Skip((pageIndex - 1) * pageSize)
                                               .Take(pageSize)
                                               .ToListAsync();
            if (!hoadon.Any())
            {
                return NotFound("Không tìm thấy hóa đơn nào.");
            }
            return Ok(hoadon);
        }


        [HttpGet("FilterTotalPrice")]
        public async Task<IActionResult> FilterTotalPrice([FromQuery] int fromTotalPrice, [FromQuery] int toTotalPrice, int pageIndex = 1, int pageSize = 20)
        {
            pageIndex = pageIndex <= 1 ? 1 : pageIndex;
            pageSize = pageSize <= 20 ? 20 : pageSize;
            if (fromTotalPrice <= 0 || toTotalPrice <= 0)
            {
                return BadRequest("Vui lòng điền tổng tiền vào chỗ trống");
            }
            var hoadon = await _context.Hoadons.Where(h => h.Tongtien >= fromTotalPrice && h.Tongtien <= toTotalPrice)
                                               .Skip((pageIndex - 1) * pageSize)
                                               .Take(pageSize)
                                               .ToListAsync();
            if (!hoadon.Any())
            {
                return NotFound("Không tìm thấy hóa đơn nào.");
            }
            return Ok(hoadon);
        }

        [HttpGet("SearchByQuery")]
        public async Task<IActionResult> SearchByQuery([FromQuery] string? maGiaoDich, [FromQuery] string? tenHoaDon, int pageIndex = 1, int pageSize = 20)
        {
            pageIndex = pageIndex <= 1 ? 1 : pageIndex;
            pageSize = pageSize <= 20 ? 20 : pageSize;
            var hoadon = await _context.Hoadons.ToListAsync();
            if (maGiaoDich != null && tenHoaDon != null)
            {
                hoadon = await _context.Hoadons.Where(h => h.Magiaodich == maGiaoDich && h.Tenhoadon == tenHoaDon)
                                               .Skip((pageIndex - 1) * pageSize)
                                               .Take(pageSize)
                                               .ToListAsync();
            }

            if (maGiaoDich != null && tenHoaDon == null)
            {
                hoadon = await _context.Hoadons.Where(h => h.Magiaodich == maGiaoDich)
                                               .Skip((pageIndex - 1) * pageSize)
                                               .Take(pageSize)
                                               .ToListAsync();
            }

            if (maGiaoDich == null && tenHoaDon != null)
            {
                hoadon = await _context.Hoadons.Where(h => h.Tenhoadon == tenHoaDon)
                                               .Skip((pageIndex - 1) * pageSize)
                                               .Take(pageSize)
                                               .ToListAsync();
            }

            if (!hoadon.Any())
            {
                return NotFound("Không tìm thấy hóa đơn nào.");
            }
            return Ok(hoadon);
        }

        private bool HoadonExists(int id)
        {
            return _context.Hoadons.Any(e => e.Hoadonid == id);
        }
    }
}
