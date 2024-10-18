using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Oracle_api.Models;

public partial class Sanpham
{
    public int       Sanphamid     { get; set; }
    public int?      Loaisanphamid { get; set; }
    public string?   Tensanpham    { get; set; }
    public double?   Giathanh      { get; set; }
    public string?   Mota          { get; set; }
    public DateTime? Ngayhethan    { get; set; }
    public string?   Kyhieusanpham { get; set; }

    [JsonIgnore]
    public virtual ICollection<Chitiethoadon> Chitiethoadons { get; set; } = new List<Chitiethoadon>();
    [JsonIgnore]
    public virtual Loaisanpham?               Loaisanpham    { get; set; }
}
