using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BTL002.Migrations
{
    /// <inheritdoc />
    public partial class AddVerifyKeyToNguoiDung : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VerifyKey",
                table: "NguoiDungs",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "NguoiDungs",
                keyColumn: "ma_nguoi_dung",
                keyValue: 1,
                column: "VerifyKey",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VerifyKey",
                table: "NguoiDungs");
        }
    }
}
