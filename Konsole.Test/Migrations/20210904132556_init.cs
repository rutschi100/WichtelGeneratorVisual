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
                        name: "FK_SecretSantaModel_SecretSantaModel_ChoiseID",
                        column: x => x.ChoiseID,
                        principalTable: "SecretSantaModel",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SantaBlackListSecretSantaModel",
                columns: table => new
                {
                    BlackListID = table.Column<Guid>(type: "TEXT", nullable: false),
                    BlackListID1 = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SantaBlackListSecretSantaModel", x => new { x.BlackListID, x.BlackListID1 });
                    table.ForeignKey(
                        name: "FK_SantaBlackListSecretSantaModel_SantaBlackList_BlackListID1",
                        column: x => x.BlackListID1,
                        principalTable: "SantaBlackList",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SantaBlackListSecretSantaModel_SecretSantaModel_BlackListID",
                        column: x => x.BlackListID,
                        principalTable: "SecretSantaModel",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SantaWhiteListSecretSantaModel",
                columns: table => new
                {
                    WhiteListID = table.Column<Guid>(type: "TEXT", nullable: false),
                    WhiteListID1 = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SantaWhiteListSecretSantaModel", x => new { x.WhiteListID, x.WhiteListID1 });
                    table.ForeignKey(
                        name: "FK_SantaWhiteListSecretSantaModel_SantaWhiteList_WhiteListID1",
                        column: x => x.WhiteListID1,
                        principalTable: "SantaWhiteList",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SantaWhiteListSecretSantaModel_SecretSantaModel_WhiteListID",
                        column: x => x.WhiteListID,
                        principalTable: "SecretSantaModel",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConfigModels_ID",
                table: "ConfigModels",
                column: "ID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MailAdressModel_ConfigModelID",
                table: "MailAdressModel",
                column: "ConfigModelID");

            migrationBuilder.CreateIndex(
                name: "IX_SantaBlackListSecretSantaModel_BlackListID1",
                table: "SantaBlackListSecretSantaModel",
                column: "BlackListID1");

            migrationBuilder.CreateIndex(
                name: "IX_SantaWhiteListSecretSantaModel_WhiteListID1",
                table: "SantaWhiteListSecretSantaModel",
                column: "WhiteListID1");

            migrationBuilder.CreateIndex(
                name: "IX_SecretSantaModel_ChoiseID",
                table: "SecretSantaModel",
                column: "ChoiseID");

            migrationBuilder.CreateIndex(
                name: "IX_SecretSantaModel_ConfigModelID",
                table: "SecretSantaModel",
                column: "ConfigModelID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MailAdressModel");

            migrationBuilder.DropTable(
                name: "SantaBlackListSecretSantaModel");

            migrationBuilder.DropTable(
                name: "SantaWhiteListSecretSantaModel");

            migrationBuilder.DropTable(
                name: "SantaBlackList");

            migrationBuilder.DropTable(
                name: "SantaWhiteList");

            migrationBuilder.DropTable(
                name: "SecretSantaModel");

            migrationBuilder.DropTable(
                name: "ConfigModels");
        }
    }
}
