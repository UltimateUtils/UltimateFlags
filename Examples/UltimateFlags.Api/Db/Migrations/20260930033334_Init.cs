using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UltimateFlags.Api.v10.Db.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Flags",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Key = table.Column<string>(type: "TEXT", nullable: false),
                    IsOn = table.Column<bool>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    ParentId = table.Column<Guid>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Flags_Flags_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Flags",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Flags_CreatedAt",
                table: "Flags",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Flags_DeletedAt",
                table: "Flags",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Flags_IsOn",
                table: "Flags",
                column: "IsOn");

            migrationBuilder.CreateIndex(
                name: "IX_Flags_Name",
                table: "Flags",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Flags_ParentId_Name",
                table: "Flags",
                columns: new[] { "ParentId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Flags_UpdatedAt",
                table: "Flags",
                column: "UpdatedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Flags");
        }
    }
}
