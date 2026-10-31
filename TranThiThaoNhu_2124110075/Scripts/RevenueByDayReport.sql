/* Stored Procedure cho tiêu chí 45 - được tạo tự động bởi EF Core Migration.
   Có thể chạy thủ công để khôi phục procedure nếu cần. */
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
GO
