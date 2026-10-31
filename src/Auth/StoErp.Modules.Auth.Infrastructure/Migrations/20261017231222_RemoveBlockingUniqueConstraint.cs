using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoErp.Modules.Auth.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveBlockingUniqueConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_password_reset_tokens_user_id",
                schema: "auth",
                table: "password_reset_tokens");

            migrationBuilder.CreateIndex(
                name: "ix_password_reset_tokens_user_id",
                schema: "auth",
                table: "password_reset_tokens",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_password_reset_tokens_user_id",
                schema: "auth",
                table: "password_reset_tokens");

            migrationBuilder.CreateIndex(
                name: "ix_password_reset_tokens_user_id",
                schema: "auth",
                table: "password_reset_tokens",
                column: "user_id",
                unique: true);
        }
    }
}
