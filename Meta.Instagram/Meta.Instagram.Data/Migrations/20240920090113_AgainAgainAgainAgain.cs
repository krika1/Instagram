using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Meta.Instagram.Data.Migrations
{
    /// <inheritdoc />
    public partial class AgainAgainAgainAgain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Profiles_AccountId",
                table: "Profiles");

            migrationBuilder.AlterColumn<string>(
                name: "AccountId",
                table: "Profiles",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_AccountId",
                table: "Profiles",
                column: "AccountId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Profiles_AccountId",
                table: "Profiles");

            migrationBuilder.AlterColumn<string>(
                name: "AccountId",
                table: "Profiles",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_AccountId",
                table: "Profiles",
                column: "AccountId",
                unique: true,
                filter: "[AccountId] IS NOT NULL");
        }
    }
}
