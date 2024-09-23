using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AutoBookKeeper.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddedUserTokens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Book_BookId",
                table: "Roles");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Book_BookId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_TransactionTypes_TypeId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionTypes_Book_BookId",
                table: "TransactionTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TransactionTypes",
                table: "TransactionTypes");

            migrationBuilder.DropIndex(
                name: "IX_TransactionTypes_BookId",
                table: "TransactionTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Roles",
                table: "Roles");

            migrationBuilder.RenameTable(
                name: "TransactionTypes",
                newName: "TransactionType");

            migrationBuilder.RenameTable(
                name: "Transactions",
                newName: "Transaction");

            migrationBuilder.RenameTable(
                name: "Roles",
                newName: "UserBookRole");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_TypeId",
                table: "Transaction",
                newName: "IX_Transaction_TypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_BookId",
                table: "Transaction",
                newName: "IX_Transaction_BookId");

            migrationBuilder.RenameIndex(
                name: "IX_Roles_BookId",
                table: "UserBookRole",
                newName: "IX_UserBookRole_BookId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TransactionType",
                table: "TransactionType",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transaction",
                table: "Transaction",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserBookRole",
                table: "UserBookRole",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "UserToken",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Token = table.Column<string>(type: "text", nullable: false),
                    ExpirationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserToken_ApplicationUser_UserId",
                        column: x => x.UserId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransactionType_BookId_Name",
                table: "TransactionType",
                columns: new[] { "BookId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_NameIdentifier",
                table: "Transaction",
                column: "NameIdentifier",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserBookRole_Name_BookId",
                table: "UserBookRole",
                columns: new[] { "Name", "BookId" });

            migrationBuilder.CreateIndex(
                name: "IX_UserToken_ExpirationTime",
                table: "UserToken",
                column: "ExpirationTime");

            migrationBuilder.CreateIndex(
                name: "IX_UserToken_UserId",
                table: "UserToken",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Book_BookId",
                table: "Transaction",
                column: "BookId",
                principalTable: "Book",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_TransactionType_TypeId",
                table: "Transaction",
                column: "TypeId",
                principalTable: "TransactionType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionType_Book_BookId",
                table: "TransactionType",
                column: "BookId",
                principalTable: "Book",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserBookRole_Book_BookId",
                table: "UserBookRole",
                column: "BookId",
                principalTable: "Book",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Book_BookId",
                table: "Transaction");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_TransactionType_TypeId",
                table: "Transaction");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionType_Book_BookId",
                table: "TransactionType");

            migrationBuilder.DropForeignKey(
                name: "FK_UserBookRole_Book_BookId",
                table: "UserBookRole");

            migrationBuilder.DropTable(
                name: "UserToken");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserBookRole",
                table: "UserBookRole");

            migrationBuilder.DropIndex(
                name: "IX_UserBookRole_Name_BookId",
                table: "UserBookRole");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TransactionType",
                table: "TransactionType");

            migrationBuilder.DropIndex(
                name: "IX_TransactionType_BookId_Name",
                table: "TransactionType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Transaction",
                table: "Transaction");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_NameIdentifier",
                table: "Transaction");

            migrationBuilder.RenameTable(
                name: "UserBookRole",
                newName: "Roles");

            migrationBuilder.RenameTable(
                name: "TransactionType",
                newName: "TransactionTypes");

            migrationBuilder.RenameTable(
                name: "Transaction",
                newName: "Transactions");

            migrationBuilder.RenameIndex(
                name: "IX_UserBookRole_BookId",
                table: "Roles",
                newName: "IX_Roles_BookId");

            migrationBuilder.RenameIndex(
                name: "IX_Transaction_TypeId",
                table: "Transactions",
                newName: "IX_Transactions_TypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Transaction_BookId",
                table: "Transactions",
                newName: "IX_Transactions_BookId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Roles",
                table: "Roles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TransactionTypes",
                table: "TransactionTypes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionTypes_BookId",
                table: "TransactionTypes",
                column: "BookId");

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Book_BookId",
                table: "Roles",
                column: "BookId",
                principalTable: "Book",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Book_BookId",
                table: "Transactions",
                column: "BookId",
                principalTable: "Book",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_TransactionTypes_TypeId",
                table: "Transactions",
                column: "TypeId",
                principalTable: "TransactionTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionTypes_Book_BookId",
                table: "TransactionTypes",
                column: "BookId",
                principalTable: "Book",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
