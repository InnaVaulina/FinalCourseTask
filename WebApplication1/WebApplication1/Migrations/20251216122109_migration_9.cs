using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    public partial class migration_9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_SocialLinks_ContactId",
                table: "SocialLinks",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_Phones_ContactId",
                table: "Phones",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_Mails_ContactId",
                table: "Mails",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_ContactId",
                table: "Addresses",
                column: "ContactId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Contacts_ContactId",
                table: "Addresses",
                column: "ContactId",
                principalTable: "Contacts",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Mails_Contacts_ContactId",
                table: "Mails",
                column: "ContactId",
                principalTable: "Contacts",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Phones_Contacts_ContactId",
                table: "Phones",
                column: "ContactId",
                principalTable: "Contacts",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SocialLinks_Contacts_ContactId",
                table: "SocialLinks",
                column: "ContactId",
                principalTable: "Contacts",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Contacts_ContactId",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Mails_Contacts_ContactId",
                table: "Mails");

            migrationBuilder.DropForeignKey(
                name: "FK_Phones_Contacts_ContactId",
                table: "Phones");

            migrationBuilder.DropForeignKey(
                name: "FK_SocialLinks_Contacts_ContactId",
                table: "SocialLinks");

            migrationBuilder.DropIndex(
                name: "IX_SocialLinks_ContactId",
                table: "SocialLinks");

            migrationBuilder.DropIndex(
                name: "IX_Phones_ContactId",
                table: "Phones");

            migrationBuilder.DropIndex(
                name: "IX_Mails_ContactId",
                table: "Mails");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_ContactId",
                table: "Addresses");
        }
    }
}
