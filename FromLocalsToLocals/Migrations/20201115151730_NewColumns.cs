using Microsoft.EntityFrameworkCore.Migrations;

namespace FromLocalsToLocals.Migrations
{
    public partial class NewColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReplyDate",
                table: "Reviews",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReplySender",
                table: "Reviews",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReplyDate",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "ReplySender",
                table: "Reviews");
        }
    }
}
