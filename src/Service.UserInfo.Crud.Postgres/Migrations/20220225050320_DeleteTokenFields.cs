using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Service.UserInfo.Crud.Postgres.Migrations
{
    public partial class DeleteTokenFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_userinfo_ActivationHash",
                schema: "education",
                table: "userinfo");

            migrationBuilder.DropColumn(
                name: "ActivationHash",
                schema: "education",
                table: "userinfo");

            migrationBuilder.DropColumn(
                name: "IpAddress",
                schema: "education",
                table: "userinfo");

            migrationBuilder.DropColumn(
                name: "JwtToken",
                schema: "education",
                table: "userinfo");

            migrationBuilder.DropColumn(
                name: "RefreshToken",
                schema: "education",
                table: "userinfo");

            migrationBuilder.DropColumn(
                name: "RefreshTokenExpires",
                schema: "education",
                table: "userinfo");

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                schema: "education",
                table: "userinfo",
                type: "boolean",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                schema: "education",
                table: "userinfo");

            migrationBuilder.AddColumn<string>(
                name: "ActivationHash",
                schema: "education",
                table: "userinfo",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IpAddress",
                schema: "education",
                table: "userinfo",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JwtToken",
                schema: "education",
                table: "userinfo",
                type: "character varying(800)",
                maxLength: 800,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                schema: "education",
                table: "userinfo",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshTokenExpires",
                schema: "education",
                table: "userinfo",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_userinfo_ActivationHash",
                schema: "education",
                table: "userinfo",
                column: "ActivationHash");
        }
    }
}
