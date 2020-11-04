using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FromLocalsToLocals.Migrations
{
    public partial class Extend_IdentityUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VendorsCount",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "VendorsCount",
                table: "AspNetUsers");
        }
    }
}
