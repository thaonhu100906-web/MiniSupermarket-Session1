/* Tự động được API áp dụng sau EF Core Migrations; có thể chạy thủ công trong SSMS khi cần. */
/* Chạy script này trong SQL Server Management Studio trên database tranthithaonhudb.
   Không tạo/xóa database; chỉ tạo/cập nhật stored procedure tìm kiếm sản phẩm. */
CREATE OR ALTER PROCEDURE dbo.SearchProductsByKeyword
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
END;
GO
