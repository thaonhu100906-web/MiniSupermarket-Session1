using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using TranThiThaoNhu_2124110075.API.Data;

#nullable disable

namespace TranThiThaoNhu_2124110075.API.Migrations
{
    [DbContext(typeof(SupermarketDbContext))]
    [Migration("20261024100000_ConvertBarcodeToNumericEan13")]
    public partial class ConvertBarcodeToNumericEan13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"UPDATE dbo.Products SET Barcode = CASE ProductId
                WHEN 1 THEN '8930000000019' WHEN 2 THEN '8930000000026' WHEN 3 THEN '8930000000033'
                WHEN 4 THEN '8930000000040' WHEN 5 THEN '8930000000057' WHEN 6 THEN '8930000000064'
                WHEN 7 THEN '8930000000071' WHEN 8 THEN '8930000000088' WHEN 9 THEN '8930000000095'
                WHEN 10 THEN '8930000000101' WHEN 11 THEN '8930000000118' WHEN 12 THEN '8930000000125'
                WHEN 13 THEN '8930000000132' WHEN 14 THEN '8930000000149' WHEN 15 THEN '8930000000156'
                ELSE Barcode END WHERE ProductId BETWEEN 1 AND 15;");

            // Tạo stored procedure ngay trong migration để lệnh Update-Database
            // tự tạo đủ database, dữ liệu và stored procedures sau khi xóa database.
            migrationBuilder.Sql(@"
IF OBJECT_ID(N'dbo.SearchProductsByKeyword', N'P') IS NOT NULL
    DROP PROCEDURE dbo.SearchProductsByKeyword;");
            migrationBuilder.Sql(@"
CREATE PROCEDURE dbo.SearchProductsByKeyword
    @Keyword NVARCHAR(150)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT p.ProductId, p.Barcode, p.ProductName, p.Price, p.StockQuantity,
           p.CategoryId, c.CategoryName
    FROM dbo.Products AS p
    INNER JOIN dbo.Categories AS c ON c.CategoryId = p.CategoryId
    WHERE p.ProductName LIKE N'%' + @Keyword + N'%'
       OR p.Barcode LIKE N'%' + @Keyword + N'%'
    ORDER BY p.ProductName;
END;");

            migrationBuilder.Sql(@"
IF OBJECT_ID(N'dbo.GetShiftRevenue', N'P') IS NOT NULL
    DROP PROCEDURE dbo.GetShiftRevenue;");
            migrationBuilder.Sql(@"
CREATE PROCEDURE dbo.GetShiftRevenue
    @StartDate DATETIME2,
    @EndDate DATETIME2
AS
BEGIN
    SET NOCOUNT ON;
    IF (@StartDate IS NULL OR @EndDate IS NULL OR @StartDate >= @EndDate)
        THROW 50001, N'Khoang thoi gian bao cao khong hop le.', 1;
    SELECT COUNT_BIG(*) AS OrderCount,
           COALESCE(SUM(TotalAmount), 0) AS TotalRevenue,
           @StartDate AS StartDate,
           @EndDate AS EndDate
    FROM dbo.Orders
    WHERE OrderDate >= @StartDate AND OrderDate < @EndDate;
END;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"UPDATE dbo.Products SET Barcode = 'DF' + RIGHT('000' + CAST(ProductId AS VARCHAR(3)), 3)
                WHERE ProductId BETWEEN 1 AND 15;");
        }
    }
}
