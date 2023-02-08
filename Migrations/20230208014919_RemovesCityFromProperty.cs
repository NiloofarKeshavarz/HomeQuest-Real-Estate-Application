using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeQuest.Migrations
{
    /// <inheritdoc />
    public partial class RemovesCityFromProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Properties");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "City",
                table: "Properties",
                type: "int",
                maxLength: 50,
                nullable: false,
                defaultValue: 0);
        }
    }
}
