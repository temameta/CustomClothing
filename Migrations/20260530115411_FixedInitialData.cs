using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CustomClothing.Migrations
{
    /// <inheritdoc />
    public partial class FixedInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompletedWorks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsPublic = table.Column<bool>(type: "boolean", nullable: false),
                    ClothingCategoryId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompletedWorks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompletedWorks_Categories_ClothingCategoryId",
                        column: x => x.ClothingCategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DesignRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    CustomerPhone = table.Column<string>(type: "text", nullable: false),
                    ClothingCategoryId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DesignRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DesignRequests_Categories_ClothingCategoryId",
                        column: x => x.ClothingCategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Куртки" },
                    { 2, "Футболки" },
                    { 3, "Штаны" }
                });

            migrationBuilder.InsertData(
                table: "CompletedWorks",
                columns: new[] { "Id", "ClothingCategoryId", "Description", "IsPublic", "Name" },
                values: new object[,]
                {
                    { 1, 1, "Классическая куртка с ручной росписью на спине", true, "Кожанка 'Old School'" },
                    { 2, 2, "Светящийся принт, оверсайз крой", true, "Футболка 'Cyber-Punk'" },
                    { 3, 1, "Этот заказ не виден обычным пользователям", false, "Секретный проект X" }
                });

            migrationBuilder.InsertData(
                table: "DesignRequests",
                columns: new[] { "Id", "ClothingCategoryId", "CreatedAt", "CustomerPhone", "Description", "Title" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "+79001112233", "Хочу вышивку золотого дракона на всю спину", "Худи с драконом" },
                    { 2, 3, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "88005553535", "Нужны кастомные нашивки по бокам", "Шорты для бега" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompletedWorks_ClothingCategoryId",
                table: "CompletedWorks",
                column: "ClothingCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_DesignRequests_ClothingCategoryId",
                table: "DesignRequests",
                column: "ClothingCategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompletedWorks");

            migrationBuilder.DropTable(
                name: "DesignRequests");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
