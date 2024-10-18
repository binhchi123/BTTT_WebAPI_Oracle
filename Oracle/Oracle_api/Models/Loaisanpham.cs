using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Oracle_api.Models;

public partial class Loaisanpham
{
    public int     Loaisanphamid  { get; set; }
    public string? Tenloaisanpham { get; set; }

    [JsonIgnore]
    public virtual ICollection<Sanpham> Sanphams { get; set; } = new List<Sanpham>();
}
