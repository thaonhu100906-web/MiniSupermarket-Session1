/* Tự động được API áp dụng sau EF Core Migrations; có thể chạy thủ công trong SSMS khi cần. */
/* Chay script nay mot lan tren database TranThiThaoNhuDb.
   Bao cao doanh thu theo khoang thoi gian/ca duoc truyen vao. */
CREATE OR ALTER PROCEDURE dbo.GetShiftRevenue
    @StartDate DATETIME2,
    @EndDate DATETIME2
AS
BEGIN
    SET NOCOUNT ON;

    IF (@StartDate IS NULL OR @EndDate IS NULL OR @StartDate >= @EndDate)
    BEGIN
        THROW 50001, N'Khoang thoi gian bao cao khong hop le.', 1;
    END;

    SELECT
        COUNT_BIG(*) AS OrderCount,
        COALESCE(SUM(TotalAmount), 0) AS TotalRevenue,
        @StartDate AS StartDate,
        @EndDate AS EndDate
    FROM dbo.Orders
    WHERE OrderDate >= @StartDate AND OrderDate < @EndDate;
END;
GO
