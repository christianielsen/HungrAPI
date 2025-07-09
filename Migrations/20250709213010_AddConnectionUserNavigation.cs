using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HungrAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddConnectionUserNavigation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Connections_User1Id",
                table: "Connections",
                column: "User1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Connections_User2Id",
                table: "Connections",
                column: "User2Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Connections_Users_User1Id",
                table: "Connections",
                column: "User1Id",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Connections_Users_User2Id",
                table: "Connections",
                column: "User2Id",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Connections_Users_User1Id",
                table: "Connections");

            migrationBuilder.DropForeignKey(
                name: "FK_Connections_Users_User2Id",
                table: "Connections");

            migrationBuilder.DropIndex(
                name: "IX_Connections_User1Id",
                table: "Connections");

            migrationBuilder.DropIndex(
                name: "IX_Connections_User2Id",
                table: "Connections");
        }
    }
}
