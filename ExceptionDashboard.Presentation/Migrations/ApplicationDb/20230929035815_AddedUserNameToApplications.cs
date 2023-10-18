using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExceptionDashboard.WebApi.Migrations.ApplicationDb
{
    /// <inheritdoc />
    public partial class AddedUserNameToApplications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "ApplicationDb",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "ApplicationDb");
        }
    }
}
