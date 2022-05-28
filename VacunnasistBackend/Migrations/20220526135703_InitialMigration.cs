using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VacunnasistBackend.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Offices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vaccines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vaccines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DNI = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BelongsToRiskGroup = table.Column<bool>(type: "bit", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PreferedOfficeId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Offices_PreferedOfficeId",
                        column: x => x.PreferedOfficeId,
                        principalTable: "Offices",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AppliedVaccines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppliedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AppliedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    VaccineId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppliedVaccines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppliedVaccines_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppliedVaccines_Vaccines_VaccineId",
                        column: x => x.VaccineId,
                        principalTable: "Vaccines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Offices",
                columns: new[] { "Id", "Address", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1, "Calle 52 113, La Plata", true, "La Plata I" },
                    { 2, "Calle Falsa 100, La Plata", true, "Quilmes" },
                    { 3, "Calle 14 1140, La Plata", true, "La Plata II" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Address", "BelongsToRiskGroup", "BirthDate", "DNI", "Email", "FullName", "Gender", "IsActive", "PasswordHash", "PhoneNumber", "PreferedOfficeId", "Role", "UserName" },
                values: new object[,]
                {
                    { 1, "Calle Falsa 1234, La Plata", false, new DateTime(2022, 5, 26, 0, 0, 0, 0, DateTimeKind.Local), "11111111", "admin@vacunassist.com", "Administrador", "other", true, "1000:fxnBCT4gusUH54XSfv3/NexrxIF8frzN:brFsz3fRJViwv5aLDbh6upbNCfHlsCPV", "2215897845", null, "administrator", "Admin" },
                    { 2, "Calle Falsa 4567, La Plata", false, new DateTime(2022, 5, 26, 0, 0, 0, 0, DateTimeKind.Local), "11111111", "vacunador@email.com", "Vacunador", "other", true, "1000:XcLxetun5RHIfkTJLEn/+ukSZi/0dMF9:29mLYRpMyema9o/z16Tb28mVRq5hcyEP", "1158987895", null, "vacunator", "Vacunador" }
                });

            migrationBuilder.InsertData(
                table: "Vaccines",
                columns: new[] { "Id", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1, true, "COVID-19" },
                    { 2, true, "Fiebre amarilla" },
                    { 3, true, "Gripe" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Address", "BelongsToRiskGroup", "BirthDate", "DNI", "Email", "FullName", "Gender", "IsActive", "PasswordHash", "PhoneNumber", "PreferedOfficeId", "Role", "UserName" },
                values: new object[] { 3, "Calle Falsa 789, La Plata", false, new DateTime(2022, 5, 26, 0, 0, 0, 0, DateTimeKind.Local), "12548987", "email@email.com", "Paciente", "other", true, "1000:89VpTrIcIWKNaENr5jPQkTBGEa3TUMhQ:y24Q2sxGKxvDBZ5pDdbz2STQwWc1IePs", "11-8795-1478", 1, "patient", "Paciente" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Address", "BelongsToRiskGroup", "BirthDate", "DNI", "Email", "FullName", "Gender", "IsActive", "PasswordHash", "PhoneNumber", "PreferedOfficeId", "Role", "UserName" },
                values: new object[] { 4, "Calle Falsa 111, La Plata", false, new DateTime(1987, 6, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "33170336", "email2@email.com", "Juan Perez", "male", true, "1000:Eei2M4H+T2HL6gQMSwHVQ/3p3yh/v8jn:yasNAYQZWQ211mEf+JfumShxh0dTl3j5", "211-235-1478", 2, "patient", "jperez" });

            migrationBuilder.InsertData(
                table: "AppliedVaccines",
                columns: new[] { "Id", "AppliedBy", "AppliedDate", "Comment", "UserId", "VaccineId" },
                values: new object[] { 1, null, new DateTime(2022, 3, 12, 10, 30, 1, 0, DateTimeKind.Unspecified), null, 3, 1 });

            migrationBuilder.InsertData(
                table: "AppliedVaccines",
                columns: new[] { "Id", "AppliedBy", "AppliedDate", "Comment", "UserId", "VaccineId" },
                values: new object[] { 2, null, new DateTime(2022, 5, 10, 14, 30, 25, 0, DateTimeKind.Unspecified), null, 3, 2 });

            migrationBuilder.CreateIndex(
                name: "IX_AppliedVaccines_UserId",
                table: "AppliedVaccines",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AppliedVaccines_VaccineId",
                table: "AppliedVaccines",
                column: "VaccineId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_PreferedOfficeId",
                table: "Users",
                column: "PreferedOfficeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppliedVaccines");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Vaccines");

            migrationBuilder.DropTable(
                name: "Offices");
        }
    }
}
