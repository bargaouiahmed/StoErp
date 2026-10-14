using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoErp.Modules.Auth.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddOutboxDeadStateAndErrorHistorySchemaChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "outbox_messages",
                schema: "messaging",
                newName: "outbox_messages",
                newSchema: "auth");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "messaging");

            migrationBuilder.RenameTable(
                name: "outbox_messages",
                schema: "auth",
                newName: "outbox_messages",
                newSchema: "messaging");
        }
    }
}
