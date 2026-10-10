using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TranThiThaoNhu_2124110075.API.Migrations
{
    /// <inheritdoc />
    public partial class AddDailyFreshEmployeeData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "FullName", "PasswordHash", "Role", "Username" },
                values: new object[,]
                {
                    { 1, "Nguyễn Minh Anh", "SEED_ONLY_01", "Admin", "admin01" },
                    { 2, "Trần Quốc Bảo", "SEED_ONLY_02", "Admin", "admin02" },
                    { 3, "Phạm Ngọc Lan", "SEED_ONLY_03", "Cashier", "cashier01" },
                    { 4, "Võ Thị Thanh", "SEED_ONLY_04", "Cashier", "cashier02" },
                    { 5, "Đặng Minh Thư", "SEED_ONLY_05", "Cashier", "cashier03" },
                    { 6, "Bùi Gia Hân", "SEED_ONLY_06", "Cashier", "cashier04" },
                    { 7, "Ngô Khánh Linh", "SEED_ONLY_07", "Cashier", "cashier05" },
                    { 8, "Phan Tuấn Kiệt", "SEED_ONLY_08", "Cashier", "cashier06" },
                    { 9, "Đỗ Thảo Vy", "SEED_ONLY_09", "Cashier", "cashier07" },
                    { 10, "Hồ Minh Khang", "SEED_ONLY_10", "Cashier", "cashier08" },
                    { 11, "Nguyễn Thành Đạt", "SEED_ONLY_11", "Warehouse", "warehouse01" },
                    { 12, "Trần Thị Kim Oanh", "SEED_ONLY_12", "Warehouse", "warehouse02" },
                    { 13, "Lê Quốc Huy", "SEED_ONLY_13", "Warehouse", "warehouse03" },
                    { 14, "Phạm Thị Thu Hà", "SEED_ONLY_14", "Warehouse", "warehouse04" },
                    { 15, "Vũ Minh Đức", "SEED_ONLY_15", "Warehouse", "warehouse05" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 15);
        }
    }
}
