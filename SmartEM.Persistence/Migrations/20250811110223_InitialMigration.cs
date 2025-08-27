using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartEM.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "identity_schema");

            migrationBuilder.CreateSequence(
                name: "ExternalLoginSequence");

            migrationBuilder.CreateSequence(
                name: "RefreshTokenSequence");

            migrationBuilder.CreateSequence(
                name: "UserSequence");

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "identity_schema",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AccountType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsFirstLogin = table.Column<bool>(type: "bit", nullable: false),
                    LastLoginTimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastPasswordResetToken = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Photo_FileName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Photo_ContentType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    JobTitle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SequentialId = table.Column<int>(type: "int", nullable: false, defaultValueSql: "NEXT VALUE FOR [UserSequence]")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExternalLogins",
                schema: "identity_schema",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProviderId = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ProviderName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SequentialId = table.Column<int>(type: "int", nullable: false, defaultValueSql: "NEXT VALUE FOR [ExternalLoginSequence]")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalLogins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExternalLogins_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "identity_schema",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                schema: "identity_schema",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TokenString = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Expiry = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRevoked = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SequentialId = table.Column<int>(type: "int", nullable: false, defaultValueSql: "NEXT VALUE FOR [RefreshTokenSequence]")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "identity_schema",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExternalLogins_UserId",
                schema: "identity_schema",
                table: "ExternalLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                schema: "identity_schema",
                table: "RefreshTokens",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExternalLogins",
                schema: "identity_schema");

            migrationBuilder.DropTable(
                name: "RefreshTokens",
                schema: "identity_schema");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "identity_schema");

            migrationBuilder.DropSequence(
                name: "ExternalLoginSequence");

            migrationBuilder.DropSequence(
                name: "RefreshTokenSequence");

            migrationBuilder.DropSequence(
                name: "UserSequence");
        }
    }
}
