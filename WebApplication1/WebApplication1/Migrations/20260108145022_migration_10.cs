using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    public partial class migration_10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IkonIllustrationId",
                table: "SocialLinks",
                newName: "IkonIllustrationName");

            migrationBuilder.RenameColumn(
                name: "MapIllustrationId",
                table: "Addresses",
                newName: "MapIllustrationName");

            migrationBuilder.RenameColumn(
                name: "Adress",
                table: "Addresses",
                newName: "MapIllustrationExtension");

            migrationBuilder.AddColumn<string>(
                name: "IkonIllustrationExtention",
                table: "SocialLinks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Addresses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IkonIllustrationExtention",
                table: "SocialLinks");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Addresses");

            migrationBuilder.RenameColumn(
                name: "IkonIllustrationName",
                table: "SocialLinks",
                newName: "IkonIllustrationId");

            migrationBuilder.RenameColumn(
                name: "MapIllustrationName",
                table: "Addresses",
                newName: "MapIllustrationId");

            migrationBuilder.RenameColumn(
                name: "MapIllustrationExtension",
                table: "Addresses",
                newName: "Adress");
        }
    }
}
