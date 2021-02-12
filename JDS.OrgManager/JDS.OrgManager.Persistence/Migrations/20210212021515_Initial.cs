using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace JDS.OrgManager.Persistence.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "PaidTimeOffPolicies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    AllowsUnlimitedPto = table.Column<bool>(type: "bit", nullable: false),
                    EmployeeLevel = table.Column<int>(type: "int", nullable: false),
                    IsDefaultForEmployeeLevel = table.Column<bool>(type: "bit", nullable: false),
                    MaxPtoHours = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PtoAccrualRate = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    LastModifiedUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaidTimeOffPolicies", x => new { x.TenantId, x.Id });
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address1 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Address2 = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    AspNetUsersId = table.Column<int>(type: "int", nullable: false),
                    City = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: true),
                    CurrencyCode = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    State = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    Title = table.Column<int>(type: "int", nullable: true),
                    ZipCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    LastModifiedUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customers_Currencies_CurrencyCode",
                        column: x => x.CurrencyCode,
                        principalTable: "Currencies",
                        principalColumn: "Code");
                    // JDS
                    table.ForeignKey(
                        name: "FK_Customers_AspNetUsers_AspNetUsersId",
                        column: x => x.AspNetUsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Tenants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssignmentKey = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    LastModifiedUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tenants_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    Address1 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Address2 = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    AspNetUsersId = table.Column<int>(type: "int", nullable: false),
                    City = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    CurrencyCode = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    DateHired = table.Column<DateTime>(type: "date", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "date", nullable: false),
                    DateTerminated = table.Column<DateTime>(type: "date", nullable: true),
                    EmployeeLevel = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    PaidTimeOffPolicyId = table.Column<int>(type: "int", nullable: false),
                    PtoHoursRemaining = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    Salary = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    SocialSecurityNumber = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    State = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    ZipCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    LastModifiedUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => new { x.TenantId, x.Id });
                    table.ForeignKey(
                        name: "FK_Employees_Currencies_CurrencyCode",
                        column: x => x.CurrencyCode,
                        principalTable: "Currencies",
                        principalColumn: "Code");
                    table.ForeignKey(
                        name: "FK_Employees_PaidTimeOffPolicies_TenantId_PaidTimeOffPolicyId",
                        columns: x => new { x.TenantId, x.PaidTimeOffPolicyId },
                        principalTable: "PaidTimeOffPolicies",
                        principalColumns: new[] { "TenantId", "Id" });
                    table.ForeignKey(
                        name: "FK_Employees_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    // JDS
                    table.ForeignKey(
                        name: "FK_Employees_AspNetUsers_AspNetUsersId",
                        column: x => x.AspNetUsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TenantAspNetUsers",
                columns: table => new
                {
                    AspNetUsersId = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenantAspNetUsers", x => new { x.TenantId, x.AspNetUsersId });
                    table.ForeignKey(
                        name: "FK_TenantAspNetUsers_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    // JDS
                    table.ForeignKey(
                        name: "FK_TenantAspNetUsers_AspNetUsers_AspNetUsersId",
                        column: x => x.AspNetUsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EmployeeManagers",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    ManagerId = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeManagers", x => new { x.TenantId, x.ManagerId, x.EmployeeId });
                    table.ForeignKey(
                        name: "FK_EmployeeManagers_Employees_TenantId_EmployeeId",
                        columns: x => new { x.TenantId, x.EmployeeId },
                        principalTable: "Employees",
                        principalColumns: new[] { "TenantId", "Id" });
                    table.ForeignKey(
                        name: "FK_EmployeeManagers_Employees_TenantId_ManagerId",
                        columns: x => new { x.TenantId, x.ManagerId },
                        principalTable: "Employees",
                        principalColumns: new[] { "TenantId", "Id" });
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CurrencyCode",
                table: "Customers",
                column: "CurrencyCode");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeManagers_TenantId_EmployeeId",
                table: "EmployeeManagers",
                columns: new[] { "TenantId", "EmployeeId" });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_CurrencyCode",
                table: "Employees",
                column: "CurrencyCode");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_TenantId_PaidTimeOffPolicyId",
                table: "Employees",
                columns: new[] { "TenantId", "PaidTimeOffPolicyId" });

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_CustomerId",
                table: "Tenants",
                column: "CustomerId");

            // JDS
            migrationBuilder.CreateIndex(
                name: "IX_TenantAspNetUsers_AspNetUsersId",
                table: "TenantAspNetUsers",
                column: "AspNetUsersId");

            // JDS
            migrationBuilder.CreateIndex(
                name: "IX_Customers_AspNetUsersId",
                table: "Customers",
                column: "AspNetUsersId");

            // JDS
            migrationBuilder.CreateIndex(
                name: "IX_Employees_AspNetUsersId",
                table: "Employees",
                column: "AspNetUsersId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeManagers");

            migrationBuilder.DropTable(
                name: "TenantAspNetUsers");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "PaidTimeOffPolicies");

            migrationBuilder.DropTable(
                name: "Tenants");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Currencies");
        }
    }
}
