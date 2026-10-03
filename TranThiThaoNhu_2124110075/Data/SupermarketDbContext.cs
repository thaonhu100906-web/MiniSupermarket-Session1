//họ tên: Trần Thị Thảo Như
//Mssv: 2124110075
//Lớp: CCQ2411C
//Môn: lập trình ứng dụng

using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using TranThiThaoNhu_2124110075.API.Models;

namespace TranThiThaoNhu_2124110075.API.Data
{
    public class SupermarketDbContext : DbContext
    {
        public SupermarketDbContext(DbContextOptions<SupermarketDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --- 1. SEED 15 CATEGORIES ---
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, CategoryName = "Bánh kem", Description = "Các loại bánh kem sinh nhật và bánh kem theo yêu cầu" },
                new Category { CategoryId = 2, CategoryName = "Bánh ngọt", Description = "Bánh ngọt, bánh mousse và bánh ăn nhẹ" },
                new Category { CategoryId = 3, CategoryName = "Bánh mì", Description = "Bánh mì ngọt, bánh mì nhân và bánh mì ăn sáng" },
                new Category { CategoryId = 4, CategoryName = "Nước ép", Description = "Các loại nước ép trái cây tươi" },
                new Category { CategoryId = 5, CategoryName = "Trà trái cây", Description = "Trà trái cây và các loại thức uống thanh mát" },
                new Category { CategoryId = 6, CategoryName = "Thực phẩm tươi sống", Description = "Thịt, cá, rau củ quả hàng ngày" },
                new Category { CategoryId = 7, CategoryName = "Đồ uống & Giải khát", Description = "Nước ngọt, nước suối, trà sữa" },
                new Category { CategoryId = 8, CategoryName = "Bánh kẹo các loại", Description = "Bánh quy, kẹo dẻo, socola" },
                new Category { CategoryId = 9, CategoryName = "Hóa mỹ phẩm", Description = "Dầu gội, sữa tắm, bột giặt" },
                new Category { CategoryId = 10, CategoryName = "Đồ gia dụng nhà bếp", Description = "Chén, đũa, xoong nồi, màng bọc" },
                new Category { CategoryId = 11, CategoryName = "Thực phẩm khô", Description = "Mì tôm, phở gói, miến, ngũ cốc" },
                new Category { CategoryId = 12, CategoryName = "Sữa & Sản phẩm từ sữa", Description = "Sữa tươi, sữa chua, phô mai" },
                new Category { CategoryId = 13, CategoryName = "Gia vị nấu ăn", Description = "Dầu ăn, nước mắm, hạt nêm, bột ngọt" },
                new Category { CategoryId = 14, CategoryName = "Đồ dùng văn phòng phẩm", Description = "Bút bi, tập vở, băng keo, kéo" },
                new Category { CategoryId = 15, CategoryName = "Sản phẩm hữu cơ (Organic)", Description = "Rau sạch hữu cơ, gạo lứt, mật ong nguyên chất" }
            );

            // --- 2. SEED 15 PRODUCTS ---
            modelBuilder.Entity<Product>().HasData(
                new Product { ProductId = 1, Barcode = "DF001", ProductName = "Bánh kem socola", Price = 250000m, StockQuantity = 20, CategoryId = 1 },
                new Product { ProductId = 2, Barcode = "DF002", ProductName = "Bánh kem dâu", Price = 280000m, StockQuantity = 15, CategoryId = 1 },
                new Product { ProductId = 3, Barcode = "DF003", ProductName = "Bánh mousse chanh dây", Price = 45000m, StockQuantity = 30, CategoryId = 2 },
                new Product { ProductId = 4, Barcode = "DF004", ProductName = "Bánh tiramisu", Price = 50000m, StockQuantity = 25, CategoryId = 2 },
                new Product { ProductId = 5, Barcode = "DF005", ProductName = "Bánh mì phô mai", Price = 25000m, StockQuantity = 40, CategoryId = 3 },
                new Product { ProductId = 6, Barcode = "DF006", ProductName = "Nước ép cam", Price = 30000m, StockQuantity = 50, CategoryId = 4 },
                new Product { ProductId = 7, Barcode = "DF007", ProductName = "Nước ép dưa hấu", Price = 28000m, StockQuantity = 45, CategoryId = 4 },
                new Product { ProductId = 8, Barcode = "DF008", ProductName = "Nước ép dứa", Price = 30000m, StockQuantity = 35, CategoryId = 4 },
                new Product { ProductId = 9, Barcode = "DF009", ProductName = "Trà đào cam sả", Price = 35000m, StockQuantity = 40, CategoryId = 5 },
                new Product { ProductId = 10, Barcode = "DF010", ProductName = "Trà vải", Price = 32000m, StockQuantity = 40, CategoryId = 5 },
                new Product { ProductId = 11, Barcode = "DF011", ProductName = "Thịt heo ba rọi (500g)", Price = 65000m, StockQuantity = 50, CategoryId = 6 },
                new Product { ProductId = 12, Barcode = "DF012", ProductName = "Coca Cola lon 330ml", Price = 10000m, StockQuantity = 200, CategoryId = 7 },
                new Product { ProductId = 13, Barcode = "DF013", ProductName = "Bánh quy Cosy vị bơ", Price = 25000m, StockQuantity = 120, CategoryId = 8 },
                new Product { ProductId = 14, Barcode = "DF014", ProductName = "Dầu gội Clear Men 650ml", Price = 145000m, StockQuantity = 40, CategoryId = 9 },
                new Product { ProductId = 15, Barcode = "DF015", ProductName = "Dầu ăn Neptune 1 Lít", Price = 52000m, StockQuantity = 110, CategoryId = 13 }
            );

            // --- 3. SEED 15 CUSTOMERS (Có tên cụ thể và địa chỉ số nhà, tên đường đầy đủ) ---
            modelBuilder.Entity<Customer>().HasData(
                new Customer { CustomerId = 1, CustomerName = "Nguyễn Minh Anh", PhoneNumber = "0901234567", Address = "123 Đường Võ Văn Ngân, Phường Linh Chiểu, TP. Thủ Đức, TP. Hồ Chí Minh", RewardPoints = 250, MembershipRank = "Vàng" },
                new Customer { CustomerId = 2, CustomerName = "Trần Ngọc Mai", PhoneNumber = "0912345678", Address = "45 Lê Văn Sỹ, Phường 13, Quận Phú Nhuận, TP. Hồ Chí Minh", RewardPoints = 120, MembershipRank = "Bạc" },
                new Customer { CustomerId = 3, CustomerName = "Lê Hoàng Nam", PhoneNumber = "0987654321", Address = "78/2 Đường Nguyễn Oanh, Phường 17, Quận Gò Vấp, TP. Hồ Chí Minh", RewardPoints = 50, MembershipRank = "Chuẩn" },
                new Customer { CustomerId = 4, CustomerName = "Phạm Thu Hà", PhoneNumber = "0934567890", Address = "12 Cộng Hòa, Phường 4, Quận Tân Bình, TP. Hồ Chí Minh", RewardPoints = 180, MembershipRank = "Bạc" },
                new Customer { CustomerId = 5, CustomerName = "Đỗ Gia Huy", PhoneNumber = "0971234567", Address = "250 Đường Nguyễn Trãi, Phường Nguyễn Cư Trinh, Quận 1, TP. Hồ Chí Minh", RewardPoints = 350, MembershipRank = "Vàng" },
                new Customer { CustomerId = 6, CustomerName = "Vũ Thị Phương", PhoneNumber = "0956789012", Address = "567 Xô Viết Nghệ Tĩnh, Phường 26, Quận Bình Thạnh, TP. Hồ Chí Minh", RewardPoints = 90, MembershipRank = "Chuẩn" },
                new Customer { CustomerId = 7, CustomerName = "Đặng Minh Giang", PhoneNumber = "0967890123", Address = "89 Điện Biên Phủ, Phường 15, Quận Bình Thạnh, TP. Hồ Chí Minh", RewardPoints = 210, MembershipRank = "Bạc" },
                new Customer { CustomerId = 8, CustomerName = "Bùi Thị Hoa", PhoneNumber = "0978901234", Address = "204 Hoàng Văn Thụ, Phường 4, Quận Tân Bình, TP. Hồ Chí Minh", RewardPoints = 400, MembershipRank = "Vàng" },
                new Customer { CustomerId = 9, CustomerName = "Hồ Hải Long", PhoneNumber = "0989012345", Address = "15 Trường Chinh, Phường 14, Quận Tân Bình, TP. Hồ Chí Minh", RewardPoints = 60, MembershipRank = "Chuẩn" },
                new Customer { CustomerId = 10, CustomerName = "Ngô Thị Kim", PhoneNumber = "0909112233", Address = "34 Quang Trung, Phường 10, Quận Gò Vấp, TP. Hồ Chí Minh", RewardPoints = 150, MembershipRank = "Bạc" },
                new Customer { CustomerId = 11, CustomerName = "Dương Quốc Hưng", PhoneNumber = "0918223344", Address = "99 Lê Đức Thọ, Phường 17, Quận Gò Vấp, TP. Hồ Chí Minh", RewardPoints = 310, MembershipRank = "Vàng" },
                new Customer { CustomerId = 12, CustomerName = "Lý Tiểu Long", PhoneNumber = "0927334455", Address = "210 Kha Vạn Cân, Phường Linh Trung, TP. Thủ Đức, TP. Hồ Chí Minh", RewardPoints = 80, MembershipRank = "Chuẩn" },
                new Customer { CustomerId = 13, CustomerName = "Mai Thị Tuyết", PhoneNumber = "0936445566", Address = "77 Nguyễn Văn Quá, Phường Đông Hưng Thuận, Quận 12, TP. Hồ Chí Minh", RewardPoints = 220, MembershipRank = "Bạc" },
                new Customer { CustomerId = 14, CustomerName = "Phan Văn Tài", PhoneNumber = "0945556677", Address = "140 Đặng Văn Bi, Phường Trường Thọ, TP. Thủ Đức, TP. Hồ Chí Minh", RewardPoints = 130, MembershipRank = "Bạc" },
                new Customer { CustomerId = 15, CustomerName = "Đỗ Mỹ Linh", PhoneNumber = "0954667788", Address = "55 Hai Bà Trưng, Phường Bến Nghé, Quận 1, TP. Hồ Chí Minh", RewardPoints = 500, MembershipRank = "Kim cương" }
            );
        }
    }
}