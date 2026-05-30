using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CustomClothing.Migrations
{
    /// <inheritdoc />
    public partial class RefactorFoldersNames : Migration
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
                    CustomerName = table.Column<string>(type: "text", nullable: false),
                    CustomerPhone = table.Column<string>(type: "text", nullable: false),
                    DeliveryAddress = table.Column<string>(type: "text", nullable: true),
                    ClothingCategoryId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CompletedWorkId = table.Column<int>(type: "integer", nullable: false),
                    CustomerName = table.Column<string>(type: "text", nullable: false),
                    CustomerPhone = table.Column<string>(type: "text", nullable: false),
                    DeliveryAddress = table.Column<string>(type: "text", nullable: true),
                    OrderDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_CompletedWorks_CompletedWorkId",
                        column: x => x.CompletedWorkId,
                        principalTable: "CompletedWorks",
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
                columns: new[] { "Id", "ClothingCategoryId", "CreatedAt", "CustomerName", "CustomerPhone", "DeliveryAddress", "Description", "Status", "Title" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Алексей", "+79001112233", "ул. Ленина 1", "Хочу вышивку золотого дракона", 0, "Худи с драконом" },
                    { 2, 3, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Алена", "88005553535", "ул. Карла Маркса 2", "Нужны кастомные нашивки по бокам", 0, "Шорты для бега" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompletedWorks_ClothingCategoryId",
                table: "CompletedWorks",
                column: "ClothingCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_DesignRequests_ClothingCategoryId",
                table: "DesignRequests",
                column: "ClothingCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CompletedWorkId",
                table: "Orders",
                column: "CompletedWorkId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DesignRequests");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "CompletedWorks");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
