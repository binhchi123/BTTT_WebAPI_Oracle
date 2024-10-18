namespace Oracle_api.Models;
public partial class Chitiethoadon
{
    public int     Chitiethoadonid { get; set; }
    public int?    Hoadonid        { get; set; }
    public int?    Sanphamid       { get; set; }
    public int     Soluong         { get; set; }
    public string? Dvt             { get; set; }
    public double? Thanhtien       { get; set; }

    [JsonIgnore]
    public virtual Hoadon?  Hoadon  { get; set; }
    [JsonIgnore]
    public virtual Sanpham? Sanpham { get; set; }
}
