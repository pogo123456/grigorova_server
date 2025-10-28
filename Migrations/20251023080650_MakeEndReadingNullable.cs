using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Grigorova_Server.Migrations
{
    /// <inheritdoc />
    public partial class MakeEndReadingNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Genres_GenreId",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_Borrowings_Books_BookId",
                table: "Borrowings");

            migrationBuilder.DropForeignKey(
                name: "FK_Borrowings_Users_UserId",
                table: "Borrowings");

            migrationBuilder.DropIndex(
                name: "IX_Borrowings_BookId",
                table: "Borrowings");

            migrationBuilder.DropIndex(
                name: "IX_Borrowings_UserId",
                table: "Borrowings");

            migrationBuilder.DropIndex(
                name: "IX_Books_GenreId",
                table: "Books");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndReading",
                table: "Borrowings",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "EndReading",
                table: "Borrowings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Borrowings_BookId",
                table: "Borrowings",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Borrowings_UserId",
                table: "Borrowings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_GenreId",
                table: "Books",
                column: "GenreId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Genres_GenreId",
                table: "Books",
                column: "GenreId",
                principalTable: "Genres",
                principalColumn: "GenresId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Borrowings_Books_BookId",
                table: "Borrowings",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "BookId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Borrowings_Users_UserId",
                table: "Borrowings",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
