using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BTL002.Migrations
{
    /// <inheritdoc />
    public partial class AddVerifyKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsVerified",
                table: "NguoiDungs",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "NguoiDungs",
                keyColumn: "ma_nguoi_dung",
                keyValue: 1,
                column: "IsVerified",
                value: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsVerified",
                table: "NguoiDungs");
        }
    }
}
