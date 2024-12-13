using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleDataManagementSystem.Backend.Database.Migrations
{
    /// <inheritdoc />
    public partial class Added_Column_ItemEntity_IsEnabled : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEnabled",
                schema: "dbo",
                table: "Items",
                type: "bit",
                nullable: false,
                defaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEnabled",
                schema: "dbo",
                table: "Items");
        }
    }
}
