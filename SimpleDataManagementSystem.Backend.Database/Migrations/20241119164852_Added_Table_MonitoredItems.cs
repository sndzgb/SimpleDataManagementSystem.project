using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleDataManagementSystem.Backend.Database.Migrations
{
    /// <inheritdoc />
    public partial class Added_Table_MonitoredItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MonitoredItems",
                schema: "dbo",
                columns: table => new
                {
                    ItemNazivproizvoda = table.Column<string>(type: "nvarchar(512)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    StartedMonitoringAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonitoredItems_Nazivproizvoda_UserId", x => new { x.ItemNazivproizvoda, x.UserId });
                    table.ForeignKey(
                        name: "FK_MonitoredItems_Items_ItemNazivproizvoda_Nazivproizvoda",
                        column: x => x.ItemNazivproizvoda,
                        principalSchema: "dbo",
                        principalTable: "Items",
                        principalColumn: "Nazivproizvoda",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MonitoredItems_Users_UserId_Id",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MonitoredItems_UserId",
                schema: "dbo",
                table: "MonitoredItems",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MonitoredItems",
                schema: "dbo");
        }
    }
}
