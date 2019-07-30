using Microsoft.EntityFrameworkCore.Migrations;

namespace JWTAuth.Migrations
{
    public partial class AlterUserEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AccessKey",
                table: "Users",
                newName: "Password");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Users",
                newName: "AccessKey");
        }
    }
}
