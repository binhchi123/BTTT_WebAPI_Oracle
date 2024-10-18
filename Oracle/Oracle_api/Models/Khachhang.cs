using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Oracle_api.Models;

public partial class Khachhang
{
    public int       Khachhangid { get; set; }
    public string?   Hoten       { get; set; }
    public DateTime? Ngaysinh    { get; set; }
    public string?   Sdt         { get; set; }

    [JsonIgnore]
    public virtual ICollection<Hoadon> Hoadons { get; set; } = new List<Hoadon>();
}
