using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoBookKeeper.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddCheckMinLengthContsraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_TransactionType_TypeId",
                table: "Transaction");

            migrationBuilder.DropTable(
                name: "UserBookRole");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_TypeId",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "Transaction");

            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "UserToken",
                type: "character varying(512)",
                maxLength: 512,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "NameIdentifier",
                table: "Transaction",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(64)",
                oldMaxLength: 64);

            migrationBuilder.CreateTable(
                name: "BookRole",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    Access = table.Column<int>(type: "integer", nullable: false),
                    BookId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookRole", x => x.Id);
                    table.CheckConstraint("CK_MinLength_Name", "LENGTH(\"Name\") >= 16");
                    table.ForeignKey(
                        name: "FK_BookRole_Book_BookId",
                        column: x => x.BookId,
                        principalTable: "Book",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddCheckConstraint(
                name: "CK_MinLength_Name",
                table: "TransactionType",
                sql: "LENGTH(\"Name\") >= 2");

            migrationBuilder.AddCheckConstraint(
                name: "CK_MinLength_NameIdentifier",
                table: "Transaction",
                sql: "LENGTH(\"NameIdentifier\") >= 4");

            migrationBuilder.AddCheckConstraint(
                name: "CK_MinLength_Title",
                table: "Book",
                sql: "LENGTH(\"Title\") >= 3");

            migrationBuilder.AddCheckConstraint(
                name: "CK_MinLength_UserName",
                table: "ApplicationUser",
                sql: "LENGTH(\"UserName\") >= 4");

            migrationBuilder.CreateIndex(
                name: "IX_BookRole_BookId",
                table: "BookRole",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_BookRole_Name_BookId",
                table: "BookRole",
                columns: new[] { "Name", "BookId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookRole");

            migrationBuilder.DropCheckConstraint(
                name: "CK_MinLength_Name",
                table: "TransactionType");

            migrationBuilder.DropCheckConstraint(
                name: "CK_MinLength_NameIdentifier",
                table: "Transaction");

            migrationBuilder.DropCheckConstraint(
                name: "CK_MinLength_Title",
                table: "Book");

            migrationBuilder.DropCheckConstraint(
                name: "CK_MinLength_UserName",
                table: "ApplicationUser");

            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "UserToken",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(512)",
                oldMaxLength: 512);

            migrationBuilder.AlterColumn<string>(
                name: "NameIdentifier",
                table: "Transaction",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128);

            migrationBuilder.AddColumn<Guid>(
                name: "TypeId",
                table: "Transaction",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "UserBookRole",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BookId = table.Column<Guid>(type: "uuid", nullable: false),
                    Access = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBookRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserBookRole_Book_BookId",
                        column: x => x.BookId,
                        principalTable: "Book",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_TypeId",
                table: "Transaction",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBookRole_BookId",
                table: "UserBookRole",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBookRole_Name_BookId",
                table: "UserBookRole",
                columns: new[] { "Name", "BookId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_TransactionType_TypeId",
                table: "Transaction",
                column: "TypeId",
                principalTable: "TransactionType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
