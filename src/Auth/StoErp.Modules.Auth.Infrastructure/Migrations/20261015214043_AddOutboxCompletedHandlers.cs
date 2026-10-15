using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoErp.Modules.Auth.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOutboxCompletedHandlers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string[]>(
                name: "completed_handlers",
                schema: "auth",
                table: "outbox_messages",
                type: "text[]",
                nullable: false,
                defaultValue: new string[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "completed_handlers",
                schema: "auth",
                table: "outbox_messages");
        }
    }
}
