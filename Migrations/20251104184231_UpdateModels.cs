using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BTL002.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "NguoiDungs",
                keyColumn: "ma_nguoi_dung",
                keyValue: 1);

            migrationBuilder.InsertData(
                table: "NguoiDungs",
                columns: new[] { "ma_nguoi_dung", "dia_chi", "email", "ho_ten", "mat_khau", "ngay_dang_ky", "so_dien_thoai", "vai_tro" },
                values: new object[] { 2, "Việt Nam", "admin@bookstore.com", "Quản trị viên", "lsrjXOipsCRBeL8o5JZsLOG4OFcjqWprg4hYzdbKCh4=", "2025-11-05", "0000000000", "Admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "NguoiDungs",
                keyColumn: "ma_nguoi_dung",
                keyValue: 2);

            migrationBuilder.InsertData(
                table: "NguoiDungs",
                columns: new[] { "ma_nguoi_dung", "dia_chi", "email", "ho_ten", "mat_khau", "ngay_dang_ky", "so_dien_thoai", "vai_tro" },
                values: new object[] { 1, "Việt Nam", "admin@bookstore.com", "Quản trị viên", "Admin@123", "2025-11-01", "0000000000", "Admin" });
        }
    }
}
