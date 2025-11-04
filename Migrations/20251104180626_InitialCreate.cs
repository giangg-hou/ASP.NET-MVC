using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BTL002.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DanhMucs",
                columns: table => new
                {
                    ma_danh_muc = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ten_danh_muc = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    mo_ta = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DanhMucs", x => x.ma_danh_muc);
                });

            migrationBuilder.CreateTable(
                name: "NguoiDungs",
                columns: table => new
                {
                    ma_nguoi_dung = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ho_ten = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    mat_khau = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    so_dien_thoai = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    dia_chi = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    vai_tro = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ngay_dang_ky = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NguoiDungs", x => x.ma_nguoi_dung);
                });

            migrationBuilder.CreateTable(
                name: "NhaXuatBans",
                columns: table => new
                {
                    ma_nha_xuat_ban = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ten_nha_xuat_ban = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    dia_chi = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhaXuatBans", x => x.ma_nha_xuat_ban);
                });

            migrationBuilder.CreateTable(
                name: "TacGias",
                columns: table => new
                {
                    ma_tac_gia = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ten_tac_gia = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    tieu_su = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TacGias", x => x.ma_tac_gia);
                });

            migrationBuilder.CreateTable(
                name: "DonHangs",
                columns: table => new
                {
                    ma_don_hang = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ma_nguoi_dung = table.Column<int>(type: "integer", nullable: false),
                    tong_tien = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    trang_thai = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    phuong_thuc_thanh_toan = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ngay_dat_hang = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonHangs", x => x.ma_don_hang);
                    table.ForeignKey(
                        name: "FK_DonHangs_NguoiDungs_ma_nguoi_dung",
                        column: x => x.ma_nguoi_dung,
                        principalTable: "NguoiDungs",
                        principalColumn: "ma_nguoi_dung",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sachs",
                columns: table => new
                {
                    ma_sach = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ten_sach = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    ma_tac_gia = table.Column<int>(type: "integer", nullable: false),
                    ma_nha_xuat_ban = table.Column<int>(type: "integer", nullable: false),
                    ma_danh_muc = table.Column<int>(type: "integer", nullable: false),
                    mo_ta = table.Column<string>(type: "text", nullable: false),
                    gia_ban = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    so_luong_ton = table.Column<int>(type: "integer", nullable: false),
                    hinh_anh = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    nam_xuat_ban = table.Column<int>(type: "integer", nullable: false),
                    ngay_tao = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sachs", x => x.ma_sach);
                    table.ForeignKey(
                        name: "FK_Sachs_DanhMucs_ma_danh_muc",
                        column: x => x.ma_danh_muc,
                        principalTable: "DanhMucs",
                        principalColumn: "ma_danh_muc",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sachs_NhaXuatBans_ma_nha_xuat_ban",
                        column: x => x.ma_nha_xuat_ban,
                        principalTable: "NhaXuatBans",
                        principalColumn: "ma_nha_xuat_ban",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sachs_TacGias_ma_tac_gia",
                        column: x => x.ma_tac_gia,
                        principalTable: "TacGias",
                        principalColumn: "ma_tac_gia",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietDonHangs",
                columns: table => new
                {
                    ma_chi_tiet = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ma_don_hang = table.Column<int>(type: "integer", nullable: false),
                    ma_sach = table.Column<int>(type: "integer", nullable: false),
                    so_luong = table.Column<int>(type: "integer", nullable: false),
                    gia_ban = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    thanh_tien = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietDonHangs", x => x.ma_chi_tiet);
                    table.ForeignKey(
                        name: "FK_ChiTietDonHangs_DonHangs_ma_don_hang",
                        column: x => x.ma_don_hang,
                        principalTable: "DonHangs",
                        principalColumn: "ma_don_hang",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietDonHangs_Sachs_ma_sach",
                        column: x => x.ma_sach,
                        principalTable: "Sachs",
                        principalColumn: "ma_sach",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DanhGias",
                columns: table => new
                {
                    ma_danh_gia = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ma_sach = table.Column<int>(type: "integer", nullable: false),
                    ma_nguoi_dung = table.Column<int>(type: "integer", nullable: false),
                    diem_danh_gia = table.Column<int>(type: "integer", nullable: false),
                    noi_dung = table.Column<string>(type: "text", nullable: false),
                    ngay_danh_gia = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DanhGias", x => x.ma_danh_gia);
                    table.ForeignKey(
                        name: "FK_DanhGias_NguoiDungs_ma_nguoi_dung",
                        column: x => x.ma_nguoi_dung,
                        principalTable: "NguoiDungs",
                        principalColumn: "ma_nguoi_dung",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DanhGias_Sachs_ma_sach",
                        column: x => x.ma_sach,
                        principalTable: "Sachs",
                        principalColumn: "ma_sach",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GioHangs",
                columns: table => new
                {
                    ma_gio_hang = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ma_nguoi_dung = table.Column<int>(type: "integer", nullable: false),
                    ma_sach = table.Column<int>(type: "integer", nullable: false),
                    so_luong = table.Column<int>(type: "integer", nullable: false),
                    ngay_them = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GioHangs", x => x.ma_gio_hang);
                    table.ForeignKey(
                        name: "FK_GioHangs_NguoiDungs_ma_nguoi_dung",
                        column: x => x.ma_nguoi_dung,
                        principalTable: "NguoiDungs",
                        principalColumn: "ma_nguoi_dung",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GioHangs_Sachs_ma_sach",
                        column: x => x.ma_sach,
                        principalTable: "Sachs",
                        principalColumn: "ma_sach",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "YeuThichs",
                columns: table => new
                {
                    ma_yeu_thich = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ma_nguoi_dung = table.Column<int>(type: "integer", nullable: false),
                    ma_sach = table.Column<int>(type: "integer", nullable: false),
                    ngay_them = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YeuThichs", x => x.ma_yeu_thich);
                    table.ForeignKey(
                        name: "FK_YeuThichs_NguoiDungs_ma_nguoi_dung",
                        column: x => x.ma_nguoi_dung,
                        principalTable: "NguoiDungs",
                        principalColumn: "ma_nguoi_dung",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_YeuThichs_Sachs_ma_sach",
                        column: x => x.ma_sach,
                        principalTable: "Sachs",
                        principalColumn: "ma_sach",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "DanhMucs",
                columns: new[] { "ma_danh_muc", "mo_ta", "ten_danh_muc" },
                values: new object[,]
                {
                    { 1, "Sách văn học trong nước và nước ngoài", "Văn học" },
                    { 2, "Sách về kinh doanh và kinh tế", "Kinh tế" },
                    { 3, "Sách về lập trình và công nghệ thông tin", "Công nghệ" },
                    { 4, "Sách phát triển bản thân", "Tâm lý - Kỹ năng sống" },
                    { 5, "Sách dành cho trẻ em", "Thiếu nhi" }
                });

            migrationBuilder.InsertData(
                table: "NguoiDungs",
                columns: new[] { "ma_nguoi_dung", "dia_chi", "email", "ho_ten", "mat_khau", "ngay_dang_ky", "so_dien_thoai", "vai_tro" },
                values: new object[] { 1, "Việt Nam", "admin@bookstore.com", "Quản trị viên", "Admin@123", "2025-11-01", "0000000000", "Admin" });

            migrationBuilder.InsertData(
                table: "NhaXuatBans",
                columns: new[] { "ma_nha_xuat_ban", "dia_chi", "ten_nha_xuat_ban" },
                values: new object[,]
                {
                    { 1, "161B Lý Chính Thắng, Q.3, TP.HCM", "NXB Trẻ" },
                    { 2, "55 Quang Trung, Hai Bà Trưng, Hà Nội", "NXB Kim Đồng" },
                    { 3, "96 Định Công, Hoàng Mai, Hà Nội", "NXB DH Mở Hà Nội" },
                    { 4, "175 Giảng Võ, Đống Đa, Hà Nội", "NXB Lao Động" },
                    { 5, "46 Trần Hưng Đạo, Hoàn Kiếm, Hà Nội", "NXB Thế Giới" }
                });

            migrationBuilder.InsertData(
                table: "TacGias",
                columns: new[] { "ma_tac_gia", "ten_tac_gia", "tieu_su" },
                values: new object[,]
                {
                    { 1, "giang 01", "Nhà văn nổi tiếng Việt Nam" },
                    { 2, "giang 02", "Tác giả người Mỹ về phát triển bản thân" },
                    { 3, "giang 03", "Doanh nhân và tác giả người Mỹ" },
                    { 4, "giang 04", "Nhà văn người Brazil" },
                    { 5, "giang 05", "Nhà văn Nhật Bản đương đại" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDonHangs_ma_don_hang",
                table: "ChiTietDonHangs",
                column: "ma_don_hang");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDonHangs_ma_sach",
                table: "ChiTietDonHangs",
                column: "ma_sach");

            migrationBuilder.CreateIndex(
                name: "IX_DanhGias_ma_nguoi_dung",
                table: "DanhGias",
                column: "ma_nguoi_dung");

            migrationBuilder.CreateIndex(
                name: "IX_DanhGias_ma_sach_ma_nguoi_dung",
                table: "DanhGias",
                columns: new[] { "ma_sach", "ma_nguoi_dung" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DonHangs_ma_nguoi_dung",
                table: "DonHangs",
                column: "ma_nguoi_dung");

            migrationBuilder.CreateIndex(
                name: "IX_GioHangs_ma_nguoi_dung_ma_sach",
                table: "GioHangs",
                columns: new[] { "ma_nguoi_dung", "ma_sach" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GioHangs_ma_sach",
                table: "GioHangs",
                column: "ma_sach");

            migrationBuilder.CreateIndex(
                name: "IX_NguoiDungs_email",
                table: "NguoiDungs",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sachs_ma_danh_muc",
                table: "Sachs",
                column: "ma_danh_muc");

            migrationBuilder.CreateIndex(
                name: "IX_Sachs_ma_nha_xuat_ban",
                table: "Sachs",
                column: "ma_nha_xuat_ban");

            migrationBuilder.CreateIndex(
                name: "IX_Sachs_ma_tac_gia",
                table: "Sachs",
                column: "ma_tac_gia");

            migrationBuilder.CreateIndex(
                name: "IX_YeuThichs_ma_nguoi_dung_ma_sach",
                table: "YeuThichs",
                columns: new[] { "ma_nguoi_dung", "ma_sach" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_YeuThichs_ma_sach",
                table: "YeuThichs",
                column: "ma_sach");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChiTietDonHangs");

            migrationBuilder.DropTable(
                name: "DanhGias");

            migrationBuilder.DropTable(
                name: "GioHangs");

            migrationBuilder.DropTable(
                name: "YeuThichs");

            migrationBuilder.DropTable(
                name: "DonHangs");

            migrationBuilder.DropTable(
                name: "Sachs");

            migrationBuilder.DropTable(
                name: "NguoiDungs");

            migrationBuilder.DropTable(
                name: "DanhMucs");

            migrationBuilder.DropTable(
                name: "NhaXuatBans");

            migrationBuilder.DropTable(
                name: "TacGias");
        }
    }
}
