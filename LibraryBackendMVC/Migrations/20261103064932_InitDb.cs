using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryBackendMVC.Migrations
{
    /// <inheritdoc />
    public partial class InitDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Members",
                columns: new[] { "Id", "CreateTime", "Email", "Password", "Role", "UserName" },
                values: new object[] { 1, new DateTime(2026, 11, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@library.com", "123456", "Admin", "admin" });
        }
    }
}
