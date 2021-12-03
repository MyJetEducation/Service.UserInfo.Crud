using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Service.UserInfo.Crud.Postgres.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "education");

            migrationBuilder.CreateTable(
                name: "userinfo",
                schema: "education",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Sex = table.Column<bool>(type: "boolean", nullable: true),
                    UserName = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    Role = table.Column<string>(type: "text", nullable: false),
                    JwtToken = table.Column<string>(type: "character varying(800)", maxLength: 800, nullable: true),
                    RefreshToken = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    RefreshTokenExpires = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IpAddress = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userinfo", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_userinfo_Id",
                schema: "education",
                table: "userinfo",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_userinfo_UserName",
                schema: "education",
                table: "userinfo",
                column: "UserName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "userinfo",
                schema: "education");
        }
    }
}
