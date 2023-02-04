using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeQuest.Migrations
{
    /// <inheritdoc />
    public partial class ModifiesImageEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "Images");

            migrationBuilder.AddColumn<bool>(
                name: "IsPrimaryImage",
                table: "Images",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPrimaryImage",
                table: "Images");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Images",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "Images",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
