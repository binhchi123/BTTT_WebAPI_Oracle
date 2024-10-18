namespace Oracle_api.Models.ViewModels
{
    public class HoaDonViewModel
    {
        public int                          Hoadonid                { get; set; }
        public int?                         Khachhangid             { get; set; }
        public string?                      Tenhoadon               { get; set; }
        public string?                      Magiaodich              { get; set; }
        public DateTime?                    Thoigiantao             { get; set; }
        public DateTime?                    Thoigiancapnhat         { get; set; }
        public string?                      Ghichu                  { get; set; }
        public double                       Tongtien                { get; set; }
        public List<ChiTietHoaDonViewModel>? ChiTietHoaDonViewModels { get; set; }
    }
}
