using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomClothing.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDesignRequestFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomerName",
                table: "DesignRequests",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeliveryAddress",
                table: "DesignRequests",
                type: "text",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "DesignRequests",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CustomerName", "DeliveryAddress", "Description" },
                values: new object[] { "Алексей", "ул. Ленина 1", "Хочу вышивку золотого дракона" });

            migrationBuilder.UpdateData(
                table: "DesignRequests",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CustomerName", "DeliveryAddress" },
                values: new object[] { "Алена", "ул. Карла Маркса 2" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerName",
                table: "DesignRequests");

            migrationBuilder.DropColumn(
                name: "DeliveryAddress",
                table: "DesignRequests");

            migrationBuilder.UpdateData(
                table: "DesignRequests",
                keyColumn: "Id",
                keyValue: 1,
                column: "Description",
                value: "Хочу вышивку золотого дракона на всю спину");
        }
    }
}
