// Copyright ©2020 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace JDS.OrgManager.Persistence.Migrations
{
    public partial class InitialRefactored : Migration
    {
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeManagers");

            migrationBuilder.DropTable(
                name: "TenantAspNetUsers");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Tenants");

            migrationBuilder.DropTable(
                name: "PaidTimeOffPolicies");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Currencies");
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    Code = table.Column<string>(maxLength: 3, nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "PaidTimeOffPolicies",
                columns: table => new
                {
                    TenantId = table.Column<int>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AllowsUnlimitedPto = table.Column<bool>(nullable: false),
                    EmployeeLevel = table.Column<int>(nullable: false),
                    IsDefaultForEmployeeLevel = table.Column<bool>(nullable: false),
                    MaxPtoHours = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    PtoAccrualRate = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 10, nullable: false),
                    CreatedUtc = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<string>(maxLength: 10, nullable: true),
                    LastModifiedUtc = table.Column<DateTime>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaidTimeOffPolicies", x => new { x.TenantId, x.Id });
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address1 = table.Column<string>(maxLength: 50, nullable: false),
                    Address2 = table.Column<string>(maxLength: 15, nullable: true),
                    AspNetUsersId = table.Column<int>(nullable: false),
                    City = table.Column<string>(maxLength: 30, nullable: false),
                    CompanyId = table.Column<int>(nullable: true),
                    CurrencyCode = table.Column<string>(maxLength: 3, nullable: false),
                    FirstName = table.Column<string>(maxLength: 25, nullable: false),
                    LastName = table.Column<string>(maxLength: 35, nullable: false),
                    MiddleName = table.Column<string>(maxLength: 25, nullable: true),
                    State = table.Column<string>(maxLength: 2, nullable: false),
                    Title = table.Column<int>(nullable: true),
                    Zip = table.Column<string>(maxLength: 10, nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 10, nullable: false),
                    CreatedUtc = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<string>(maxLength: 10, nullable: true),
                    LastModifiedUtc = table.Column<DateTime>(nullable: true),
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
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssignmentKey = table.Column<Guid>(nullable: false),
                    CustomerId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Slug = table.Column<string>(maxLength: 25, nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 10, nullable: false),
                    CreatedUtc = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<string>(maxLength: 10, nullable: true),
                    LastModifiedUtc = table.Column<DateTime>(nullable: true),
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
                    TenantId = table.Column<int>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address1 = table.Column<string>(maxLength: 50, nullable: false),
                    Address2 = table.Column<string>(maxLength: 15, nullable: true),
                    AspNetUsersId = table.Column<int>(nullable: false),
                    City = table.Column<string>(maxLength: 30, nullable: false),
                    CurrencyCode = table.Column<string>(maxLength: 3, nullable: false),
                    DateTerminated = table.Column<DateTime>(type: "date", nullable: true),
                    DateHired = table.Column<DateTime>(type: "date", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "date", nullable: false),
                    EmployeeLevel = table.Column<int>(nullable: false),
                    FirstName = table.Column<string>(maxLength: 25, nullable: false),
                    Gender = table.Column<int>(nullable: false),
                    LastName = table.Column<string>(maxLength: 35, nullable: false),
                    MiddleName = table.Column<string>(maxLength: 25, nullable: true),
                    PaidTimeOffPolicyId = table.Column<int>(nullable: false),
                    PtoHoursRemaining = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    Salary = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    SocialSecurityNumber = table.Column<string>(maxLength: 11, nullable: true),
                    State = table.Column<string>(maxLength: 2, nullable: false),
                    Zip = table.Column<string>(maxLength: 10, nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 10, nullable: false),
                    CreatedUtc = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<string>(maxLength: 10, nullable: true),
                    LastModifiedUtc = table.Column<DateTime>(nullable: true),
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
                        name: "FK_Employees_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Employees_PaidTimeOffPolicies_TenantId_PaidTimeOffPolicyId",
                        columns: x => new { x.TenantId, x.PaidTimeOffPolicyId },
                        principalTable: "PaidTimeOffPolicies",
                        principalColumns: new[] { "TenantId", "Id" });
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
                    TenantId = table.Column<int>(nullable: false),
                    AspNetUsersId = table.Column<int>(nullable: false),
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
                    // Jake
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
                    TenantId = table.Column<int>(nullable: false),
                    EmployeeId = table.Column<int>(nullable: false),
                    ManagerId = table.Column<int>(nullable: false),
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

            // Jake
            migrationBuilder.CreateIndex(
                name: "IX_TenantAspNetUsers_AspNetUsersId",
                table: "TenantAspNetUsers",
                column: "AspNetUsersId");

            // Jake
            migrationBuilder.CreateIndex(
                name: "IX_Customers_AspNetUsersId",
                table: "Customers",
                column: "AspNetUsersId");

            // Jake
            migrationBuilder.CreateIndex(
                name: "IX_Employees_AspNetUsersId",
                table: "Employees",
                column: "AspNetUsersId");
        }
    }
}