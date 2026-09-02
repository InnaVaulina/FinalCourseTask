using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    public partial class migration_5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RequestIn",
                table: "Blogs",
                newName: "PostDate");

            migrationBuilder.RenameColumn(
                name: "Illustration",
                table: "Blogs",
                newName: "IllustrationId");

            migrationBuilder.RenameColumn(
                name: "Article",
                table: "Blogs",
                newName: "ArticleId");

            migrationBuilder.CreateTable(
                name: "bigTexts",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bigTexts", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bigTexts");

            migrationBuilder.RenameColumn(
                name: "PostDate",
                table: "Blogs",
                newName: "RequestIn");

            migrationBuilder.RenameColumn(
                name: "IllustrationId",
                table: "Blogs",
                newName: "Illustration");

            migrationBuilder.RenameColumn(
                name: "ArticleId",
                table: "Blogs",
                newName: "Article");
        }
    }
}
