using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Oracle_api.Models;

public partial class Hoadon
{
    public int       Hoadonid        { get; set; }
    public int?      Khachhangid     { get; set; }
    public string?   Tenhoadon       { get; set; }
    public string?   Magiaodich      { get; set; }
    public DateTime? Thoigiantao     { get; set; }
    public DateTime? Thoigiancapnhat { get; set; }
    public string?   Ghichu          { get; set; }
    public double    Tongtien        { get; set; }

    [JsonIgnore]
    public virtual ICollection<Chitiethoadon> Chitiethoadons { get; set; } = new List<Chitiethoadon>();
    [JsonIgnore]
    public virtual Khachhang?                 Khachhang      { get; set; }
}
