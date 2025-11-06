using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BTL002.Migrations
{
    /// <inheritdoc />
    public partial class AddVerifyKeyToUsers : Migration
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

            migrationBuilder.AddColumn<bool>(
                name: "IsVerified",
                table: "NguoiDungs",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
            name: "VerifyKey",
            table: "NguoiDungs");

            migrationBuilder.DropColumn(
                name: "IsVerified",
                table: "NguoiDungs");
        }
    }
}
