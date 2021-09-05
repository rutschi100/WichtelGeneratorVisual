using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Konsole.Test.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConfigModels",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FontSize = table.Column<int>(type: "INTEGER", nullable: false),
                    NotificationsEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    MailNotificationEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    Port = table.Column<int>(type: "INTEGER", nullable: false),
                    Absender = table.Column<string>(type: "TEXT", nullable: true),
                    ServerName = table.Column<string>(type: "TEXT", nullable: true),
                    Username = table.Column<string>(type: "TEXT", nullable: true),
                    Passwort = table.Column<string>(type: "TEXT", nullable: true),
                    SslOn = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfigModels", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SantaBlackList",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SantaBlackList", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SantaWhiteList",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SantaWhiteList", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "MailAdressModel",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Mail = table.Column<string>(type: "TEXT", nullable: true),
                    ConfigModelID = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MailAdressModel", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MailAdressModel_ConfigModels_ConfigModelID",
                        column: x => x.ConfigModelID,
                        principalTable: "ConfigModels",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SecretSantaModel",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    MailAdress = table.Column<string>(type: "TEXT", nullable: true),
                    ChoiseID = table.Column<Guid>(type: "TEXT", nullable: true),
                    BlackListID = table.Column<Guid>(type: "TEXT", nullable: true),
                    WhiteListID = table.Column<Guid>(type: "TEXT", nullable: true),
                    ConfigModelID = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecretSantaModel", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SecretSantaModel_ConfigModels_ConfigModelID",
                        column: x => x.ConfigModelID,
                        principalTable: "ConfigModels",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SecretSantaModel_SantaBlackList_BlackListID",
                        column: x => x.BlackListID,
                        principalTable: "SantaBlackList",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SecretSantaModel_SantaWhiteList_WhiteListID",
                        column: x => x.WhiteListID,
                        principalTable: "SantaWhiteList",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SecretSantaModel_SecretSantaModel_ChoiseID",
                        column: x => x.ChoiseID,
                        principalTable: "SecretSantaModel",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MailAdressModel_ConfigModelID",
                table: "MailAdressModel",
                column: "ConfigModelID");

            migrationBuilder.CreateIndex(
                name: "IX_SecretSantaModel_BlackListID",
                table: "SecretSantaModel",
                column: "BlackListID");

            migrationBuilder.CreateIndex(
                name: "IX_SecretSantaModel_ChoiseID",
                table: "SecretSantaModel",
                column: "ChoiseID");

            migrationBuilder.CreateIndex(
                name: "IX_SecretSantaModel_ConfigModelID",
                table: "SecretSantaModel",
                column: "ConfigModelID");

            migrationBuilder.CreateIndex(
                name: "IX_SecretSantaModel_WhiteListID",
                table: "SecretSantaModel",
                column: "WhiteListID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MailAdressModel");

            migrationBuilder.DropTable(
                name: "SecretSantaModel");

            migrationBuilder.DropTable(
                name: "ConfigModels");

            migrationBuilder.DropTable(
                name: "SantaBlackList");

            migrationBuilder.DropTable(
                name: "SantaWhiteList");
        }
    }
}
