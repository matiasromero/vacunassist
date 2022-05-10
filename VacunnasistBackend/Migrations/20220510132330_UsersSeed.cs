using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VacunassistBackend.Migrations
{
    public partial class UsersSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "IsActive", "Name", "PasswordHash", "Role" },
                values: new object[] { 1, "admin@mail.com", true, "Admin", "1000:rthvbS3hJ5f2O4Ay45x0EiRlcrN4klFi:5Wka1S8uSTW2oqUx9RGte0rspzqFHWTn", "administrator" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
