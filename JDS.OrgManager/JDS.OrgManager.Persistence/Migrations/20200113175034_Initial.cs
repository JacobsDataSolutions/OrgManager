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
    public partial class Initial : Migration
    {
        #region Protected Methods

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeManagers");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Currencies");

            migrationBuilder.DropTable(
                name: "PaidTimeOffPolicies");
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    Code = table.Column<string>(maxLength: 3, nullable: false)
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
                    Name = table.Column<string>(maxLength: 25, nullable: true),
                    PtoAccrualRate = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 10, nullable: true),
                    CreatedUtc = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<string>(maxLength: 10, nullable: true),
                    LastModifiedUtc = table.Column<DateTime>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaidTimeOffPolicies", x => new { x.TenantId, x.Id });
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    TenantId = table.Column<int>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address1 = table.Column<string>(maxLength: 50, nullable: true),
                    Address2 = table.Column<string>(maxLength: 15, nullable: true),
                    City = table.Column<string>(maxLength: 30, nullable: true),
                    CurrencyCode = table.Column<string>(maxLength: 3, nullable: true),
                    DateExited = table.Column<DateTime>(nullable: true),
                    DateHired = table.Column<DateTime>(nullable: false),
                    DateOfBirth = table.Column<DateTime>(nullable: false),
                    EmployeeLevel = table.Column<int>(nullable: false),
                    FirstName = table.Column<string>(maxLength: 25, nullable: true),
                    Gender = table.Column<int>(nullable: false),
                    LastName = table.Column<string>(maxLength: 25, nullable: true),
                    MiddleName = table.Column<string>(maxLength: 25, nullable: true),
                    PaidTimeOffPolicyId = table.Column<int>(nullable: false),
                    PtoHoursRemaining = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    Salary = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    SocialSecurityNumber = table.Column<string>(maxLength: 11, nullable: true),
                    State = table.Column<string>(maxLength: 2, nullable: true),
                    Zip = table.Column<string>(maxLength: 10, nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 10, nullable: true),
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
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employees_PaidTimeOffPolicies_TenantId_PaidTimeOffPolicyId",
                        columns: x => new { x.TenantId, x.PaidTimeOffPolicyId },
                        principalTable: "PaidTimeOffPolicies",
                        principalColumns: new[] { "TenantId", "Id" },
                        onDelete: ReferentialAction.Cascade);
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
        }

        #endregion
    }
}