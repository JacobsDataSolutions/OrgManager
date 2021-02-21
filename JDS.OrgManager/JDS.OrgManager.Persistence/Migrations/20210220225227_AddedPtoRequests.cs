using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace JDS.OrgManager.Persistence.Migrations
{
    public partial class AddedPtoRequests : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PaidTimeOffRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApprovalStatus = table.Column<int>(type: "int", nullable: false),
                    EndDate = table.Column<DateTime>(type: "date", nullable: false),
                    ForEmployeeId = table.Column<int>(type: "int", nullable: false),
                    HoursRequested = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    StartDate = table.Column<DateTime>(type: "date", nullable: false),
                    Paid = table.Column<bool>(type: "bit", nullable: false),
                    SubmittedById = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    LastModifiedUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaidTimeOffRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaidTimeOffRequests_Employees_TenantId_ForEmployeeId",
                        columns: x => new { x.TenantId, x.ForEmployeeId },
                        principalTable: "Employees",
                        principalColumns: new[] { "TenantId", "Id" });
                    table.ForeignKey(
                        name: "FK_PaidTimeOffRequests_Employees_TenantId_SubmittedById",
                        columns: x => new { x.TenantId, x.SubmittedById },
                        principalTable: "Employees",
                        principalColumns: new[] { "TenantId", "Id" });
                    table.ForeignKey(
                        name: "FK_PaidTimeOffRequests_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaidTimeOffRequests_TenantId_ForEmployeeId",
                table: "PaidTimeOffRequests",
                columns: new[] { "TenantId", "ForEmployeeId" });

            migrationBuilder.CreateIndex(
                name: "IX_PaidTimeOffRequests_TenantId_SubmittedById",
                table: "PaidTimeOffRequests",
                columns: new[] { "TenantId", "SubmittedById" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaidTimeOffRequests");
        }
    }
}
