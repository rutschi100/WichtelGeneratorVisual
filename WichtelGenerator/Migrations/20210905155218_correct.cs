using Microsoft.EntityFrameworkCore.Migrations;

namespace WichtelGenerator.Core.Migrations
{
    public partial class correct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_WhiteListModel_ID",
                table: "WhiteListModel",
                column: "ID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SecretSantaModel_ID",
                table: "SecretSantaModel",
                column: "ID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MailAdressModel_ID",
                table: "MailAdressModel",
                column: "ID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ConfigModels_ID",
                table: "ConfigModels",
                column: "ID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BlackListModel_ID",
                table: "BlackListModel",
                column: "ID",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WhiteListModel_ID",
                table: "WhiteListModel");

            migrationBuilder.DropIndex(
                name: "IX_SecretSantaModel_ID",
                table: "SecretSantaModel");

            migrationBuilder.DropIndex(
                name: "IX_MailAdressModel_ID",
                table: "MailAdressModel");

            migrationBuilder.DropIndex(
                name: "IX_ConfigModels_ID",
                table: "ConfigModels");

            migrationBuilder.DropIndex(
                name: "IX_BlackListModel_ID",
                table: "BlackListModel");
        }
    }
}
