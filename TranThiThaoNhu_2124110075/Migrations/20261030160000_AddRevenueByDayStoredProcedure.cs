using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using TranThiThaoNhu_2124110075.API.Data;

#nullable disable

namespace TranThiThaoNhu_2124110075.API.Migrations
{
    [DbContext(typeof(SupermarketDbContext))]
    [Migration("20261030160000_AddRevenueByDayStoredProcedure")]
    public partial class AddRevenueByDayStoredProcedure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                CREATE OR ALTER PROCEDURE dbo.GetRevenueByDay
                    @StartDate DATETIME2,
                    @EndDate DATETIME2
                AS
                BEGIN
                    SET NOCOUNT ON;

                    IF (@StartDate IS NULL OR @EndDate IS NULL OR @StartDate >= @EndDate)
                    BEGIN
                        THROW 50002, N'Khoảng thời gian báo cáo không hợp lệ.', 1;
                    END;

                    SELECT
                        CAST(OrderDate AS DATE) AS [Date],
                        COALESCE(SUM(TotalAmount), 0) AS Revenue,
                        COUNT(*) AS OrderCount
                    FROM dbo.Orders
                    WHERE OrderDate >= @StartDate
                      AND OrderDate < @EndDate
                    GROUP BY CAST(OrderDate AS DATE)
                    ORDER BY CAST(OrderDate AS DATE);
                END;
                """);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS dbo.GetRevenueByDay;");
        }
    }
}
