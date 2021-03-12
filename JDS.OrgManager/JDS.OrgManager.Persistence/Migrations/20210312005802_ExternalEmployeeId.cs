using Microsoft.EntityFrameworkCore.Migrations;

namespace JDS.OrgManager.Persistence.Migrations
{
    public partial class ExternalEmployeeId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExternalEmployeeId",
                table: "Employees",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExternalEmployeeId",
                table: "Employees");
        }
    }
}
