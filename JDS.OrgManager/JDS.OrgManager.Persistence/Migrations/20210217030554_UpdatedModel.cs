// Copyright ©2021 Jacobs Data Solutions

// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the
// License at

// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace JDS.OrgManager.Persistence.Migrations
{
    public partial class UpdatedModel : Migration
    {
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TenantDefaults");

            migrationBuilder.DropColumn(
                name: "IsPending",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "Customers");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateHired",
                table: "Employees",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Customers",
                type: "int",
                nullable: true);
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Customers");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateHired",
                table: "Employees",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AddColumn<bool>(
                name: "IsPending",
                table: "Employees",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "Customers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "TenantDefaults",
                columns: table => new
                {
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    CurrencyCode = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    EmployeeLevel = table.Column<int>(type: "int", nullable: false),
                    PaidTimeOffPolicyId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    LastModifiedUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenantDefaults", x => x.TenantId);
                    table.ForeignKey(
                        name: "FK_TenantDefaults_Currencies_CurrencyCode",
                        column: x => x.CurrencyCode,
                        principalTable: "Currencies",
                        principalColumn: "Code");
                    table.ForeignKey(
                        name: "FK_TenantDefaults_PaidTimeOffPolicies_TenantId_PaidTimeOffPolicyId",
                        columns: x => new { x.TenantId, x.PaidTimeOffPolicyId },
                        principalTable: "PaidTimeOffPolicies",
                        principalColumns: new[] { "TenantId", "Id" });
                    table.ForeignKey(
                        name: "FK_TenantDefaults_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TenantDefaults_CurrencyCode",
                table: "TenantDefaults",
                column: "CurrencyCode");

            migrationBuilder.CreateIndex(
                name: "IX_TenantDefaults_TenantId_PaidTimeOffPolicyId",
                table: "TenantDefaults",
                columns: new[] { "TenantId", "PaidTimeOffPolicyId" });
        }
    }
}