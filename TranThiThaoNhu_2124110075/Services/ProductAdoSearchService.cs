using System.Data;
using Microsoft.Data.SqlClient;

namespace TranThiThaoNhu_2124110075.API.Services
{
    /// <summary>Ví dụ ADO.NET dùng SqlConnection, SqlCommand, SqlParameter và SqlDataReader.</summary>
    public sealed class ProductAdoSearchService
    {
        private readonly string _connectionString;

        public ProductAdoSearchService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Chưa cấu hình DefaultConnection.");
        }

        public async Task<IReadOnlyList<ProductAdoSearchResult>> SearchAsync(string keyword, CancellationToken cancellationToken = default)
        {
            var results = new List<ProductAdoSearchResult>();
            await using var connection = new SqlConnection(_connectionString);
            await using var command = new SqlCommand("dbo.SearchProductsByKeyword", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.Add(new SqlParameter("@Keyword", SqlDbType.NVarChar, 150)
            {
                Value = keyword.Trim()
            });

            await connection.OpenAsync(cancellationToken);
            await using var reader = await command.ExecuteReaderAsync(cancellationToken);
            while (await reader.ReadAsync(cancellationToken))
            {
                results.Add(new ProductAdoSearchResult(
                    reader.GetInt32(reader.GetOrdinal("ProductId")),
                    reader.GetString(reader.GetOrdinal("Barcode")),
                    reader.GetString(reader.GetOrdinal("ProductName")),
                    reader.GetDecimal(reader.GetOrdinal("Price")),
                    reader.GetInt32(reader.GetOrdinal("StockQuantity")),
                    reader.GetInt32(reader.GetOrdinal("CategoryId")),
                    reader.GetString(reader.GetOrdinal("CategoryName"))));
            }
            return results;
        }
    }

    public sealed record ProductAdoSearchResult(
        int ProductId, string Barcode, string ProductName, decimal Price,
        int StockQuantity, int CategoryId, string CategoryName);
}
