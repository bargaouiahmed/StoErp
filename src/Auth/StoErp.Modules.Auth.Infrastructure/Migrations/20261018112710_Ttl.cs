using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoErp.Modules.Auth.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Ttl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ttl_expires_at",
                schema: "auth",
                table: "verification_tokens",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ttl_expires_at",
                schema: "auth",
                table: "password_reset_tokens",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_verification_tokens_ttl_expires_at",
                schema: "auth",
                table: "verification_tokens",
                column: "ttl_expires_at");

            migrationBuilder.CreateIndex(
                name: "ix_password_reset_tokens_ttl_expires_at",
                schema: "auth",
                table: "password_reset_tokens",
                column: "ttl_expires_at");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_verification_tokens_ttl_expires_at",
                schema: "auth",
                table: "verification_tokens");

            migrationBuilder.DropIndex(
                name: "ix_password_reset_tokens_ttl_expires_at",
                schema: "auth",
                table: "password_reset_tokens");

            migrationBuilder.DropColumn(
                name: "ttl_expires_at",
                schema: "auth",
                table: "verification_tokens");

            migrationBuilder.DropColumn(
                name: "ttl_expires_at",
                schema: "auth",
                table: "password_reset_tokens");
        }
    }
}
