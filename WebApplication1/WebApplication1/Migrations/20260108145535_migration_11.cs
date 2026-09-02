using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    public partial class migration_11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IkonIllustrationName",
                table: "SocialLinks",
                newName: "IkonFileName");

            migrationBuilder.RenameColumn(
                name: "IkonIllustrationExtention",
                table: "SocialLinks",
                newName: "IkonFileExtension");

            migrationBuilder.RenameColumn(
                name: "MapIllustrationName",
                table: "Addresses",
                newName: "MapFileName");

            migrationBuilder.RenameColumn(
                name: "MapIllustrationExtension",
                table: "Addresses",
                newName: "MapFileExtension");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IkonFileName",
                table: "SocialLinks",
                newName: "IkonIllustrationName");

            migrationBuilder.RenameColumn(
                name: "IkonFileExtension",
                table: "SocialLinks",
                newName: "IkonIllustrationExtention");

            migrationBuilder.RenameColumn(
                name: "MapFileName",
                table: "Addresses",
                newName: "MapIllustrationName");

            migrationBuilder.RenameColumn(
                name: "MapFileExtension",
                table: "Addresses",
                newName: "MapIllustrationExtension");
        }
    }
}
