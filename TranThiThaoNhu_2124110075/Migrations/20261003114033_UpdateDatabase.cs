using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TranThiThaoNhu_2124110075.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "CategoryName", "Description" },
                values: new object[,]
                {
                    { 6, "Thực phẩm tươi sống", "Thịt, cá, rau củ quả hàng ngày" },
                    { 7, "Đồ uống & Giải khát", "Nước ngọt, nước suối, trà sữa" },
                    { 8, "Bánh kẹo các loại", "Bánh quy, kẹo dẻo, socola" },
                    { 9, "Hóa mỹ phẩm", "Dầu gội, sữa tắm, bột giặt" },
                    { 10, "Đồ gia dụng nhà bếp", "Chén, đũa, xoong nồi, màng bọc" },
                    { 11, "Thực phẩm khô", "Mì tôm, phở gói, miến, ngũ cốc" },
                    { 12, "Sữa & Sản phẩm từ sữa", "Sữa tươi, sữa chua, phô mai" },
                    { 13, "Gia vị nấu ăn", "Dầu ăn, nước mắm, hạt nêm, bột ngọt" },
                    { 14, "Đồ dùng văn phòng phẩm", "Bút bi, tập vở, băng keo, kéo" },
                    { 15, "Sản phẩm hữu cơ (Organic)", "Rau sạch hữu cơ, gạo lứt, mật ong nguyên chất" }
                });

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 1,
                column: "Address",
                value: "123 Đường Võ Văn Ngân, Phường Linh Chiểu, TP. Thủ Đức, TP. Hồ Chí Minh");

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 2,
                column: "Address",
                value: "45 Lê Văn Sỹ, Phường 13, Quận Phú Nhuận, TP. Hồ Chí Minh");

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 3,
                column: "Address",
                value: "78/2 Đường Nguyễn Oanh, Phường 17, Quận Gò Vấp, TP. Hồ Chí Minh");

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 4,
                column: "Address",
                value: "12 Cộng Hòa, Phường 4, Quận Tân Bình, TP. Hồ Chí Minh");

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 5,
                column: "Address",
                value: "250 Đường Nguyễn Trãi, Phường Nguyễn Cư Trinh, Quận 1, TP. Hồ Chí Minh");

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "CustomerId", "Address", "CustomerName", "MembershipRank", "PhoneNumber", "RewardPoints" },
                values: new object[,]
                {
                    { 6, "567 Xô Viết Nghệ Tĩnh, Phường 26, Quận Bình Thạnh, TP. Hồ Chí Minh", "Vũ Thị Phương", "Chuẩn", "0956789012", 90 },
                    { 7, "89 Điện Biên Phủ, Phường 15, Quận Bình Thạnh, TP. Hồ Chí Minh", "Đặng Minh Giang", "Bạc", "0967890123", 210 },
                    { 8, "204 Hoàng Văn Thụ, Phường 4, Quận Tân Bình, TP. Hồ Chí Minh", "Bùi Thị Hoa", "Vàng", "0978901234", 400 },
                    { 9, "15 Trường Chinh, Phường 14, Quận Tân Bình, TP. Hồ Chí Minh", "Hồ Hải Long", "Chuẩn", "0989012345", 60 },
                    { 10, "34 Quang Trung, Phường 10, Quận Gò Vấp, TP. Hồ Chí Minh", "Ngô Thị Kim", "Bạc", "0909112233", 150 },
                    { 11, "99 Lê Đức Thọ, Phường 17, Quận Gò Vấp, TP. Hồ Chí Minh", "Dương Quốc Hưng", "Vàng", "0918223344", 310 },
                    { 12, "210 Kha Vạn Cân, Phường Linh Trung, TP. Thủ Đức, TP. Hồ Chí Minh", "Lý Tiểu Long", "Chuẩn", "0927334455", 80 },
                    { 13, "77 Nguyễn Văn Quá, Phường Đông Hưng Thuận, Quận 12, TP. Hồ Chí Minh", "Mai Thị Tuyết", "Bạc", "0936445566", 220 },
                    { 14, "140 Đặng Văn Bi, Phường Trường Thọ, TP. Thủ Đức, TP. Hồ Chí Minh", "Phan Văn Tài", "Bạc", "0945556677", 130 },
                    { 15, "55 Hai Bà Trưng, Phường Bến Nghé, Quận 1, TP. Hồ Chí Minh", "Đỗ Mỹ Linh", "Kim cương", "0954667788", 500 }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "Barcode", "CategoryId", "Price", "ProductName", "StockQuantity" },
                values: new object[,]
                {
                    { 11, "DF011", 6, 65000m, "Thịt heo ba rọi (500g)", 50 },
                    { 12, "DF012", 7, 10000m, "Coca Cola lon 330ml", 200 },
                    { 13, "DF013", 8, 25000m, "Bánh quy Cosy vị bơ", 120 },
                    { 14, "DF014", 9, 145000m, "Dầu gội Clear Men 650ml", 40 },
                    { 15, "DF015", 13, 52000m, "Dầu ăn Neptune 1 Lít", 110 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 13);

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 1,
                column: "Address",
                value: "Thủ Đức, TP. Hồ Chí Minh");

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 2,
                column: "Address",
                value: "Bình Thạnh, TP. Hồ Chí Minh");

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 3,
                column: "Address",
                value: "Gò Vấp, TP. Hồ Chí Minh");

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 4,
                column: "Address",
                value: "Tân Bình, TP. Hồ Chí Minh");

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 5,
                column: "Address",
                value: "Quận 1, TP. Hồ Chí Minh");
        }
    }
}
