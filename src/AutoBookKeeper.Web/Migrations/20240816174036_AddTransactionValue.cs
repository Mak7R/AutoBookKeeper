using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoBookKeeper.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddTransactionValue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Value",
                table: "Transaction",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Value",
                table: "Transaction");
        }
    }
}
