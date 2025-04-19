using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Configurations.Data.Migrations
{
    /// <inheritdoc />
    public partial class _002 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FieldChanged",
                table: "TaskHistories");

            migrationBuilder.DropColumn(
                name: "NewValue",
                table: "TaskHistories");

            migrationBuilder.DropColumn(
                name: "OldValue",
                table: "TaskHistories");

            migrationBuilder.AddColumn<string>(
                name: "Change",
                table: "TaskHistories",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Change",
                table: "TaskHistories");

            migrationBuilder.AddColumn<string>(
                name: "FieldChanged",
                table: "TaskHistories",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NewValue",
                table: "TaskHistories",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OldValue",
                table: "TaskHistories",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
