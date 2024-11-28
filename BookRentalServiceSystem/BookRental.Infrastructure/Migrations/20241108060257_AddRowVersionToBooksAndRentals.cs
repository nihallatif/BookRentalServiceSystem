using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookRental.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRowVersionToBooksAndRentals : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WaitingLists_Books_BookId",
                table: "WaitingLists");

            migrationBuilder.DropForeignKey(
                name: "FK_WaitingLists_Users_UserId",
                table: "WaitingLists");

            migrationBuilder.DropIndex(
                name: "IX_WaitingLists_BookId",
                table: "WaitingLists");

            migrationBuilder.DropIndex(
                name: "IX_WaitingLists_UserId",
                table: "WaitingLists");

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Rentals",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Books",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Rentals");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Books");

            migrationBuilder.CreateIndex(
                name: "IX_WaitingLists_BookId",
                table: "WaitingLists",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_WaitingLists_UserId",
                table: "WaitingLists",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_WaitingLists_Books_BookId",
                table: "WaitingLists",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WaitingLists_Users_UserId",
                table: "WaitingLists",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
