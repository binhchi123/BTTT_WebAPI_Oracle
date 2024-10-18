namespace Oracle_api.Models;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public virtual DbSet<Chitiethoadon> Chitiethoadons { get; set; }
    public virtual DbSet<Hoadon>        Hoadons        { get; set; }
    public virtual DbSet<Khachhang>     Khachhangs     { get; set; }
    public virtual DbSet<Loaisanpham>   Loaisanphams   { get; set; }
    public virtual DbSet<Sanpham>       Sanphams       { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseOracle("User Id=TEST; Password=12345678;Data Source=localhost:1521/mypdb;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasDefaultSchema("TEST")
            .UseCollation("USING_NLS_COMP");

        modelBuilder.Entity<Chitiethoadon>(entity =>
        {
            entity.HasKey(e => e.Chitiethoadonid).HasName("SYS_C007678");

            entity.ToTable("CHITIETHOADON");

            entity.Property(e => e.Chitiethoadonid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("CHITIETHOADONID");
            entity.Property(e => e.Dvt)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("DVT");
            entity.Property(e => e.Hoadonid)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("HOADONID");
            entity.Property(e => e.Sanphamid)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("SANPHAMID");
            entity.Property(e => e.Soluong)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("SOLUONG");
            entity.Property(e => e.Thanhtien)
                .HasColumnType("NUMBER")
                .HasColumnName("THANHTIEN");

            entity.HasOne(d => d.Hoadon).WithMany(p => p.Chitiethoadons)
                .HasForeignKey(d => d.Hoadonid)
                .HasConstraintName("SYS_C007679");

            entity.HasOne(d => d.Sanpham).WithMany(p => p.Chitiethoadons)
                .HasForeignKey(d => d.Sanphamid)
                .HasConstraintName("SYS_C007680");
        });

        modelBuilder.Entity<Hoadon>(entity =>
        {
            entity.HasKey(e => e.Hoadonid).HasName("SYS_C007675");

            entity.ToTable("HOADON");

            entity.Property(e => e.Hoadonid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("HOADONID");
            entity.Property(e => e.Ghichu)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("GHICHU");
            entity.Property(e => e.Khachhangid)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("KHACHHANGID");
            entity.Property(e => e.Magiaodich)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("MAGIAODICH");
            entity.Property(e => e.Tenhoadon)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("TENHOADON");
            entity.Property(e => e.Thoigiancapnhat)
                .HasColumnType("DATE")
                .HasColumnName("THOIGIANCAPNHAT");
            entity.Property(e => e.Thoigiantao)
                .HasColumnType("DATE")
                .HasColumnName("THOIGIANTAO");
            entity.Property(e => e.Tongtien)
                .HasColumnType("NUMBER")
                .HasColumnName("TONGTIEN");

            entity.HasOne(d => d.Khachhang).WithMany(p => p.Hoadons)
                .HasForeignKey(d => d.Khachhangid)
                .HasConstraintName("SYS_C007676");
        });

        modelBuilder.Entity<Khachhang>(entity =>
        {
            entity.HasKey(e => e.Khachhangid).HasName("SYS_C007673");

            entity.ToTable("KHACHHANG");

            entity.Property(e => e.Khachhangid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("KHACHHANGID");
            entity.Property(e => e.Hoten)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("HOTEN");
            entity.Property(e => e.Ngaysinh)
                .HasColumnType("DATE")
                .HasColumnName("NGAYSINH");
            entity.Property(e => e.Sdt)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("SDT");
        });

        modelBuilder.Entity<Loaisanpham>(entity =>
        {
            entity.HasKey(e => e.Loaisanphamid).HasName("SYS_C007668");

            entity.ToTable("LOAISANPHAM");

            entity.Property(e => e.Loaisanphamid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("LOAISANPHAMID");
            entity.Property(e => e.Tenloaisanpham)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("TENLOAISANPHAM");
        });

        modelBuilder.Entity<Sanpham>(entity =>
        {
            entity.HasKey(e => e.Sanphamid).HasName("SYS_C007670");

            entity.ToTable("SANPHAM");

            entity.Property(e => e.Sanphamid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("SANPHAMID");
            entity.Property(e => e.Giathanh)
                .HasColumnType("NUMBER")
                .HasColumnName("GIATHANH");
            entity.Property(e => e.Kyhieusanpham)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("KYHIEUSANPHAM");
            entity.Property(e => e.Loaisanphamid)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("LOAISANPHAMID");
            entity.Property(e => e.Mota)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("MOTA");
            entity.Property(e => e.Ngayhethan)
                .HasColumnType("DATE")
                .HasColumnName("NGAYHETHAN");
            entity.Property(e => e.Tensanpham)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("TENSANPHAM");

            entity.HasOne(d => d.Loaisanpham).WithMany(p => p.Sanphams)
                .HasForeignKey(d => d.Loaisanphamid)
                .HasConstraintName("SYS_C007671");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
