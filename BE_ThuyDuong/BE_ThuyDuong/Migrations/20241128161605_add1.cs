using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BE_ThuyDuong.Migrations
{
    /// <inheritdoc />
    public partial class add1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_historryPays_users_UserId",
                table: "historryPays");

            migrationBuilder.DropForeignKey(
                name: "FK_somfirmEmails_users_UserId",
                table: "somfirmEmails");

            migrationBuilder.DropForeignKey(
                name: "FK_users_roles_RoleId",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_somfirmEmails",
                table: "somfirmEmails");

            migrationBuilder.RenameTable(
                name: "somfirmEmails",
                newName: "comfirmEmails");

            migrationBuilder.RenameIndex(
                name: "IX_somfirmEmails_UserId",
                table: "comfirmEmails",
                newName: "IX_comfirmEmails_UserId");

            migrationBuilder.AlterColumn<int>(
                name: "RoleId",
                table: "users",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UrlAvt",
                table: "users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "historryPays",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "BillId",
                table: "historryPays",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_comfirmEmails",
                table: "comfirmEmails",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "bills",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_bills_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_historryPays_BillId",
                table: "historryPays",
                column: "BillId");

            migrationBuilder.CreateIndex(
                name: "IX_bills_UserId",
                table: "bills",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_comfirmEmails_users_UserId",
                table: "comfirmEmails",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_historryPays_bills_BillId",
                table: "historryPays",
                column: "BillId",
                principalTable: "bills",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_historryPays_users_UserId",
                table: "historryPays",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_users_roles_RoleId",
                table: "users",
                column: "RoleId",
                principalTable: "roles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_comfirmEmails_users_UserId",
                table: "comfirmEmails");

            migrationBuilder.DropForeignKey(
                name: "FK_historryPays_bills_BillId",
                table: "historryPays");

            migrationBuilder.DropForeignKey(
                name: "FK_historryPays_users_UserId",
                table: "historryPays");

            migrationBuilder.DropForeignKey(
                name: "FK_users_roles_RoleId",
                table: "users");

            migrationBuilder.DropTable(
                name: "bills");

            migrationBuilder.DropIndex(
                name: "IX_historryPays_BillId",
                table: "historryPays");

            migrationBuilder.DropPrimaryKey(
                name: "PK_comfirmEmails",
                table: "comfirmEmails");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "users");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "users");

            migrationBuilder.DropColumn(
                name: "UrlAvt",
                table: "users");

            migrationBuilder.DropColumn(
                name: "BillId",
                table: "historryPays");

            migrationBuilder.RenameTable(
                name: "comfirmEmails",
                newName: "somfirmEmails");

            migrationBuilder.RenameIndex(
                name: "IX_comfirmEmails_UserId",
                table: "somfirmEmails",
                newName: "IX_somfirmEmails_UserId");

            migrationBuilder.AlterColumn<int>(
                name: "RoleId",
                table: "users",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "historryPays",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_somfirmEmails",
                table: "somfirmEmails",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_historryPays_users_UserId",
                table: "historryPays",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_somfirmEmails_users_UserId",
                table: "somfirmEmails",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_users_roles_RoleId",
                table: "users",
                column: "RoleId",
                principalTable: "roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
