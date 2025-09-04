using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Infrastructure.Migrations;

/// <inheritdoc />
public partial class InitalCreate : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "services",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                port_from = table.Column<ushort>(type: "INTEGER", nullable: false),
                port_to = table.Column<ushort>(type: "INTEGER", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_services", x => x.Id);
            });

        migrationBuilder.CreateIndex(
            name: "IX_services_name",
            table: "services",
            column: "name",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "services");
    }
}
