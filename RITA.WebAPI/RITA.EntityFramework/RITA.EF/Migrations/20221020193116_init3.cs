using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RITA.EF.Migrations
{
    public partial class init3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestFields_TestDatas_TestDataId",
                table: "RequestFields");

            migrationBuilder.DropForeignKey(
                name: "FK_ResponseFields_TestDatas_TestDataId",
                table: "ResponseFields");

            migrationBuilder.DropForeignKey(
                name: "FK_TestCases_Suites_SuiteId",
                table: "TestCases");

            migrationBuilder.DropForeignKey(
                name: "FK_TestDatas_TestCases_TestCaseId",
                table: "TestDatas");

            migrationBuilder.DropForeignKey(
                name: "FK_TestDatas_TestTypes_TestTypeId",
                table: "TestDatas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TestTypes",
                table: "TestTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TestDatas",
                table: "TestDatas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TestCases",
                table: "TestCases");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Suites",
                table: "Suites");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ResponseFields",
                table: "ResponseFields");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RequestFields",
                table: "RequestFields");

            migrationBuilder.RenameTable(
                name: "TestTypes",
                newName: "TestType");

            migrationBuilder.RenameTable(
                name: "TestDatas",
                newName: "TestData");

            migrationBuilder.RenameTable(
                name: "TestCases",
                newName: "TestCase");

            migrationBuilder.RenameTable(
                name: "Suites",
                newName: "Suite");

            migrationBuilder.RenameTable(
                name: "ResponseFields",
                newName: "ResponseField");

            migrationBuilder.RenameTable(
                name: "RequestFields",
                newName: "RequestField");

            migrationBuilder.RenameIndex(
                name: "IX_TestDatas_TestTypeId",
                table: "TestData",
                newName: "IX_TestData_TestTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_TestDatas_TestCaseId",
                table: "TestData",
                newName: "IX_TestData_TestCaseId");

            migrationBuilder.RenameIndex(
                name: "IX_TestCases_SuiteId",
                table: "TestCase",
                newName: "IX_TestCase_SuiteId");

            migrationBuilder.RenameIndex(
                name: "IX_Suites_AppId",
                table: "Suite",
                newName: "IX_Suite_AppId");

            migrationBuilder.RenameIndex(
                name: "IX_ResponseFields_TestDataId",
                table: "ResponseField",
                newName: "IX_ResponseField_TestDataId");

            migrationBuilder.RenameIndex(
                name: "IX_RequestFields_TestDataId",
                table: "RequestField",
                newName: "IX_RequestField_TestDataId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SuspendedOn",
                table: "TestData",
                type: "timestamp(6)",
                rowVersion: true,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp(6)",
                oldRowVersion: true,
                oldNullable: true)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TestType",
                table: "TestType",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TestData",
                table: "TestData",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TestCase",
                table: "TestCase",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Suite",
                table: "Suite",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ResponseField",
                table: "ResponseField",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RequestField",
                table: "RequestField",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_TestData_Suspended",
                table: "TestData",
                column: "Suspended");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestField_TestData_TestDataId",
                table: "RequestField",
                column: "TestDataId",
                principalTable: "TestData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ResponseField_TestData_TestDataId",
                table: "ResponseField",
                column: "TestDataId",
                principalTable: "TestData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TestCase_Suite_SuiteId",
                table: "TestCase",
                column: "SuiteId",
                principalTable: "Suite",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TestData_TestCase_TestCaseId",
                table: "TestData",
                column: "TestCaseId",
                principalTable: "TestCase",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TestData_TestType_TestTypeId",
                table: "TestData",
                column: "TestTypeId",
                principalTable: "TestType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestField_TestData_TestDataId",
                table: "RequestField");

            migrationBuilder.DropForeignKey(
                name: "FK_ResponseField_TestData_TestDataId",
                table: "ResponseField");

            migrationBuilder.DropForeignKey(
                name: "FK_TestCase_Suite_SuiteId",
                table: "TestCase");

            migrationBuilder.DropForeignKey(
                name: "FK_TestData_TestCase_TestCaseId",
                table: "TestData");

            migrationBuilder.DropForeignKey(
                name: "FK_TestData_TestType_TestTypeId",
                table: "TestData");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TestType",
                table: "TestType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TestData",
                table: "TestData");

            migrationBuilder.DropIndex(
                name: "IX_TestData_Suspended",
                table: "TestData");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TestCase",
                table: "TestCase");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Suite",
                table: "Suite");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ResponseField",
                table: "ResponseField");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RequestField",
                table: "RequestField");

            migrationBuilder.RenameTable(
                name: "TestType",
                newName: "TestTypes");

            migrationBuilder.RenameTable(
                name: "TestData",
                newName: "TestDatas");

            migrationBuilder.RenameTable(
                name: "TestCase",
                newName: "TestCases");

            migrationBuilder.RenameTable(
                name: "Suite",
                newName: "Suites");

            migrationBuilder.RenameTable(
                name: "ResponseField",
                newName: "ResponseFields");

            migrationBuilder.RenameTable(
                name: "RequestField",
                newName: "RequestFields");

            migrationBuilder.RenameIndex(
                name: "IX_TestData_TestTypeId",
                table: "TestDatas",
                newName: "IX_TestDatas_TestTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_TestData_TestCaseId",
                table: "TestDatas",
                newName: "IX_TestDatas_TestCaseId");

            migrationBuilder.RenameIndex(
                name: "IX_TestCase_SuiteId",
                table: "TestCases",
                newName: "IX_TestCases_SuiteId");

            migrationBuilder.RenameIndex(
                name: "IX_Suite_AppId",
                table: "Suites",
                newName: "IX_Suites_AppId");

            migrationBuilder.RenameIndex(
                name: "IX_ResponseField_TestDataId",
                table: "ResponseFields",
                newName: "IX_ResponseFields_TestDataId");

            migrationBuilder.RenameIndex(
                name: "IX_RequestField_TestDataId",
                table: "RequestFields",
                newName: "IX_RequestFields_TestDataId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SuspendedOn",
                table: "TestDatas",
                type: "timestamp(6)",
                rowVersion: true,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp(6)",
                oldRowVersion: true,
                oldNullable: true)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TestTypes",
                table: "TestTypes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TestDatas",
                table: "TestDatas",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TestCases",
                table: "TestCases",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Suites",
                table: "Suites",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ResponseFields",
                table: "ResponseFields",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RequestFields",
                table: "RequestFields",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestFields_TestDatas_TestDataId",
                table: "RequestFields",
                column: "TestDataId",
                principalTable: "TestDatas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ResponseFields_TestDatas_TestDataId",
                table: "ResponseFields",
                column: "TestDataId",
                principalTable: "TestDatas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TestCases_Suites_SuiteId",
                table: "TestCases",
                column: "SuiteId",
                principalTable: "Suites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TestDatas_TestCases_TestCaseId",
                table: "TestDatas",
                column: "TestCaseId",
                principalTable: "TestCases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TestDatas_TestTypes_TestTypeId",
                table: "TestDatas",
                column: "TestTypeId",
                principalTable: "TestTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
