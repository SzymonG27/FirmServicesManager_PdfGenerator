using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PDFGenerator.Migrations.ApplicationDb
{
    public partial class SecInitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accesories",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FixID = table.Column<int>(type: "int", nullable: false),
                    NameOfAccesory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumberOfAccesory = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accesories", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ClientFirmRelations",
                columns: table => new
                {
                    ClientID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirmID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientFirmRelations", x => new { x.ClientID, x.FirmID });
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SurName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<int>(type: "int", nullable: false),
                    isFirm = table.Column<bool>(type: "bit", nullable: false),
                    EMail = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Firms",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirmName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NIP = table.Column<int>(type: "int", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Firms", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Fixes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmpWhoAcceptID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfAdmission = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    DateOfRelease = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ItemToFix = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WhatAccesory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WhatToFix = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CostOfRepair = table.Column<float>(type: "real", nullable: false),
                    PublicComments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrivateComments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordIfExist = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Barcode = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fixes", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accesories");

            migrationBuilder.DropTable(
                name: "ClientFirmRelations");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Firms");

            migrationBuilder.DropTable(
                name: "Fixes");
        }
    }
}
