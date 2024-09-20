using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Meta.Instagram.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedFeorighnKeyAgain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Profiles_ProfileId",
                table: "Accounts");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_ProfileId",
                table: "Accounts");

            migrationBuilder.AlterColumn<string>(
                name: "ProfileId",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "ProfileId1",
                table: "Accounts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_ProfileId1",
                table: "Accounts",
                column: "ProfileId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Profiles_ProfileId1",
                table: "Accounts",
                column: "ProfileId1",
                principalTable: "Profiles",
                principalColumn: "ProfileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Profiles_ProfileId1",
                table: "Accounts");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_ProfileId1",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "ProfileId1",
                table: "Accounts");

            migrationBuilder.AlterColumn<string>(
                name: "ProfileId",
                table: "Accounts",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_ProfileId",
                table: "Accounts",
                column: "ProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Profiles_ProfileId",
                table: "Accounts",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "ProfileId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
