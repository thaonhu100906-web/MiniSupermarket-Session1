// Họ tên: Trần Thị Thảo Như
// MSSV: 2124110075
// Lớp: CCQ2411C
// Môn: lập trình ứng dụng

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814

namespace TranThiThaoNhu_2124110075.API.Migrations
{
    public partial class AddProductAndCustomer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    RewardPoints = table.Column<int>(type: "int", nullable: false),
                    MembershipRank = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerId);
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[]
                {
                    "CustomerId",
                    "Address",
                    "CustomerName",
                    "MembershipRank",
                    "PhoneNumber",
                    "RewardPoints"
                },
                values: new object[,]
                {
                    { 1, "Thủ Đức, TP. Hồ Chí Minh", "Nguyễn Minh Anh", "Vàng", "0901234567", 250 },
                    { 2, "Bình Thạnh, TP. Hồ Chí Minh", "Trần Ngọc Mai", "Bạc", "0912345678", 120 },
                    { 3, "Gò Vấp, TP. Hồ Chí Minh", "Lê Hoàng Nam", "Chuẩn", "0987654321", 50 },
                    { 4, "Tân Bình, TP. Hồ Chí Minh", "Phạm Thu Hà", "Bạc", "0934567890", 180 },
                    { 5, "Quận 1, TP. Hồ Chí Minh", "Đỗ Gia Huy", "Vàng", "0971234567", 350 }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[]
                {
                    "ProductId",
                    "Barcode",
                    "CategoryId",
                    "Price",
                    "ProductName",
                    "StockQuantity"
                },
                values: new object[,]
                {
                    { 1, "DF001", 1, 250000m, "Bánh kem socola", 20 },
                    { 2, "DF002", 1, 280000m, "Bánh kem dâu", 15 },
                    { 3, "DF003", 2, 45000m, "Bánh mousse chanh dây", 30 },
                    { 4, "DF004", 2, 50000m, "Bánh tiramisu", 25 },
                    { 5, "DF005", 3, 25000m, "Bánh mì phô mai", 40 },
                    { 6, "DF006", 4, 30000m, "Nước ép cam", 50 },
                    { 7, "DF007", 4, 28000m, "Nước ép dưa hấu", 45 },
                    { 8, "DF008", 4, 30000m, "Nước ép dứa", 35 },
                    { 9, "DF009", 5, 35000m, "Trà đào cam sả", 40 },
                    { 10, "DF010", 5, 32000m, "Trà vải", 40 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 10);

            migrationBuilder.DropTable(
                name: "Customers");
        }
    }
}