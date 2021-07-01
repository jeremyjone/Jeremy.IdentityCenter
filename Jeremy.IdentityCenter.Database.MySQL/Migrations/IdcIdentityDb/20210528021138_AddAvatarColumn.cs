using Microsoft.EntityFrameworkCore.Migrations;

namespace Jeremy.IdentityCenter.Database.MySQL.Migrations.IdcIdentityDb
{
    public partial class AddAvatarColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Avatar",
                table: "idcUsers",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Avatar",
                table: "idcUsers");
        }
    }
}
