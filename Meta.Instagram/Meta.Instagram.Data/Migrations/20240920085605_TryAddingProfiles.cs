using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Meta.Instagram.Data.Migrations
{
    /// <inheritdoc />
    public partial class TryAddingProfiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Profiles_ProfileId1",
                table: "Accounts");

            migrationBuilder.DropIndex(
                name: "IX_Profiles_AccountId",
                table: "Profiles");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_ProfileId1",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "ProfileId",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "ProfileId1",
                table: "Accounts");

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

            migrationBuilder.AddColumn<string>(
                name: "ProfileId",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProfileId1",
                table: "Accounts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_AccountId",
                table: "Profiles",
                column: "AccountId");

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
    }
}
