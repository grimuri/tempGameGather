using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GameGather.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateUsersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Birthday = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    VerifiedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ResetPasswordToken_Value = table.Column<Guid>(type: "uuid", nullable: true),
                    ResetPasswordToken_CreatedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ResetPasswordToken_ExpiresOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ResetPasswordToken_Type = table.Column<int>(type: "integer", nullable: true),
                    Ban_CreatedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Ban_ExpiresOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Ban_Message = table.Column<string>(type: "text", nullable: true),
                    Role = table.Column<string>(type: "text", nullable: false),
                    Password_ExpiresOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Password_LastModifiedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Password_Value = table.Column<string>(type: "text", nullable: false),
                    VerificationToken_CreatedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    VerificationToken_ExpiresOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    VerificationToken_Type = table.Column<int>(type: "integer", nullable: false),
                    VerificationToken_Value = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
