
// Họ tên: Trần Thị Thảo Như
// MSSV: 2124110075
// Lớp: CCQ2411C
// Môn: lập trình ứng dụng

using Microsoft.EntityFrameworkCore;
using TranThiThaoNhu_2124110075.API.Models;

namespace TranThiThaoNhu_2124110075.API.Data
{
    public class SupermarketDbContext : DbContext
    {
        public SupermarketDbContext(
            DbContextOptions<SupermarketDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ==========================================
            // 1. DANH MỤC SẢN PHẨM - DAILY FRESH
            // ==========================================
            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    CategoryId = 1,
                    CategoryName = "Bánh kem",
                    Description = "Bánh kem sinh nhật, bánh kem socola và bánh kem theo yêu cầu"
                },
                new Category
                {
                    CategoryId = 2,
                    CategoryName = "Bánh mousse",
                    Description = "Bánh mousse chanh dây, dâu tây, socola và trà xanh"
                },
                new Category
                {
                    CategoryId = 3,
                    CategoryName = "Bánh ngọt",
                    Description = "Tiramisu, cheesecake, cupcake và bánh ngọt dùng trong ngày"
                },
                new Category
                {
                    CategoryId = 4,
                    CategoryName = "Bánh mì",
                    Description = "Bánh mì ngọt, bánh mì phô mai và bánh mì nhân kem"
                },
                new Category
                {
                    CategoryId = 5,
                    CategoryName = "Bánh quy",
                    Description = "Bánh quy bơ, bánh quy socola và bánh quy hạnh nhân"
                },
                new Category
                {
                    CategoryId = 6,
                    CategoryName = "Bánh tart",
                    Description = "Bánh tart trứng, tart trái cây và tart phô mai"
                },
                new Category
                {
                    CategoryId = 7,
                    CategoryName = "Bánh su kem",
                    Description = "Su kem truyền thống, su kem socola và su kem trà xanh"
                },
                new Category
                {
                    CategoryId = 8,
                    CategoryName = "Nước ép cam",
                    Description = "Nước ép cam tươi nguyên chất"
                },
                new Category
                {
                    CategoryId = 9,
                    CategoryName = "Nước ép dưa hấu",
                    Description = "Nước ép dưa hấu tươi, phục vụ trong ngày"
                },
                new Category
                {
                    CategoryId = 10,
                    CategoryName = "Nước ép thơm",
                    Description = "Nước ép thơm nguyên chất và nước ép thơm kết hợp"
                },
                new Category
                {
                    CategoryId = 11,
                    CategoryName = "Nước ép táo",
                    Description = "Nước ép táo tươi và nước ép táo kết hợp"
                },
                new Category
                {
                    CategoryId = 12,
                    CategoryName = "Nước ép hỗn hợp",
                    Description = "Nước ép kết hợp nhiều loại trái cây tươi"
                },
                new Category
                {
                    CategoryId = 13,
                    CategoryName = "Trà trái cây",
                    Description = "Trà đào, trà vải, trà dâu và trà cam sả"
                },
                new Category
                {
                    CategoryId = 14,
                    CategoryName = "Cà phê",
                    Description = "Cà phê sữa, cà phê đen và bạc xỉu"
                },
                new Category
                {
                    CategoryId = 15,
                    CategoryName = "Combo bánh và nước",
                    Description = "Combo bánh ngọt kết hợp nước ép hoặc trà trái cây"
                }
            );

            // ==========================================
            // 2. SẢN PHẨM - DAILY FRESH
            // ==========================================
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    ProductId = 1,
                    Barcode = "DF001",
                    ProductName = "Bánh kem socola",
                    Price = 250000m,
                    StockQuantity = 20,
                    CategoryId = 1
                },
                new Product
                {
                    ProductId = 2,
                    Barcode = "DF002",
                    ProductName = "Bánh kem dâu tây",
                    Price = 280000m,
                    StockQuantity = 15,
                    CategoryId = 1
                },
                new Product
                {
                    ProductId = 3,
                    Barcode = "DF003",
                    ProductName = "Bánh mousse chanh dây",
                    Price = 45000m,
                    StockQuantity = 30,
                    CategoryId = 2
                },
                new Product
                {
                    ProductId = 4,
                    Barcode = "DF004",
                    ProductName = "Bánh tiramisu",
                    Price = 50000m,
                    StockQuantity = 25,
                    CategoryId = 3
                },
                new Product
                {
                    ProductId = 5,
                    Barcode = "DF005",
                    ProductName = "Bánh mì phô mai",
                    Price = 25000m,
                    StockQuantity = 40,
                    CategoryId = 4
                },
                new Product
                {
                    ProductId = 6,
                    Barcode = "DF006",
                    ProductName = "Nước ép cam tươi",
                    Price = 30000m,
                    StockQuantity = 50,
                    CategoryId = 8
                },
                new Product
                {
                    ProductId = 7,
                    Barcode = "DF007",
                    ProductName = "Nước ép dưa hấu",
                    Price = 28000m,
                    StockQuantity = 45,
                    CategoryId = 9
                },
                new Product
                {
                    ProductId = 8,
                    Barcode = "DF008",
                    ProductName = "Nước ép thơm",
                    Price = 30000m,
                    StockQuantity = 35,
                    CategoryId = 10
                },
                new Product
                {
                    ProductId = 9,
                    Barcode = "DF009",
                    ProductName = "Trà đào cam sả",
                    Price = 35000m,
                    StockQuantity = 40,
                    CategoryId = 13
                },
                new Product
                {
                    ProductId = 10,
                    Barcode = "DF010",
                    ProductName = "Trà vải",
                    Price = 32000m,
                    StockQuantity = 40,
                    CategoryId = 13
                },
                new Product
                {
                    ProductId = 11,
                    Barcode = "DF011",
                    ProductName = "Cheesecake việt quất",
                    Price = 55000m,
                    StockQuantity = 20,
                    CategoryId = 3
                },
                new Product
                {
                    ProductId = 12,
                    Barcode = "DF012",
                    ProductName = "Bánh tart trứng",
                    Price = 22000m,
                    StockQuantity = 35,
                    CategoryId = 6
                },
                new Product
                {
                    ProductId = 13,
                    Barcode = "DF013",
                    ProductName = "Bánh su kem",
                    Price = 18000m,
                    StockQuantity = 30,
                    CategoryId = 7
                },
                new Product
                {
                    ProductId = 14,
                    Barcode = "DF014",
                    ProductName = "Nước ép táo",
                    Price = 32000m,
                    StockQuantity = 30,
                    CategoryId = 11
                },
                new Product
                {
                    ProductId = 15,
                    Barcode = "DF015",
                    ProductName = "Combo bánh mousse và nước ép cam",
                    Price = 69000m,
                    StockQuantity = 20,
                    CategoryId = 15
                }
            );

            // ==========================================
            // 3. KHÁCH HÀNG DAILY FRESH
            // ==========================================
            modelBuilder.Entity<Customer>().HasData(
                new Customer
                {
                    CustomerId = 1,
                    CustomerName = "Nguyễn Minh Anh",
                    PhoneNumber = "0901234567",
                    Address = "123 Võ Văn Ngân, TP. Thủ Đức, TP. Hồ Chí Minh",
                    RewardPoints = 250,
                    MembershipRank = "Vàng"
                },
                new Customer
                {
                    CustomerId = 2,
                    CustomerName = "Trần Ngọc Mai",
                    PhoneNumber = "0912345678",
                    Address = "45 Lê Văn Sỹ, Quận Phú Nhuận, TP. Hồ Chí Minh",
                    RewardPoints = 120,
                    MembershipRank = "Bạc"
                },
                new Customer
                {
                    CustomerId = 3,
                    CustomerName = "Lê Hoàng Nam",
                    PhoneNumber = "0987654321",
                    Address = "78/2 Nguyễn Oanh, Quận Gò Vấp, TP. Hồ Chí Minh",
                    RewardPoints = 50,
                    MembershipRank = "Chuẩn"
                },
                new Customer
                {
                    CustomerId = 4,
                    CustomerName = "Phạm Thu Hà",
                    PhoneNumber = "0934567890",
                    Address = "12 Cộng Hòa, Quận Tân Bình, TP. Hồ Chí Minh",
                    RewardPoints = 180,
                    MembershipRank = "Bạc"
                },
                new Customer
                {
                    CustomerId = 5,
                    CustomerName = "Đỗ Gia Huy",
                    PhoneNumber = "0971234567",
                    Address = "250 Nguyễn Trãi, Quận 1, TP. Hồ Chí Minh",
                    RewardPoints = 350,
                    MembershipRank = "Vàng"
                },
                new Customer
                {
                    CustomerId = 6,
                    CustomerName = "Vũ Thị Phương",
                    PhoneNumber = "0956789012",
                    Address = "567 Xô Viết Nghệ Tĩnh, Quận Bình Thạnh, TP. Hồ Chí Minh",
                    RewardPoints = 90,
                    MembershipRank = "Chuẩn"
                },
                new Customer
                {
                    CustomerId = 7,
                    CustomerName = "Đặng Minh Giang",
                    PhoneNumber = "0967890123",
                    Address = "89 Điện Biên Phủ, Quận Bình Thạnh, TP. Hồ Chí Minh",
                    RewardPoints = 210,
                    MembershipRank = "Bạc"
                },
                new Customer
                {
                    CustomerId = 8,
                    CustomerName = "Bùi Thị Hoa",
                    PhoneNumber = "0978901234",
                    Address = "204 Hoàng Văn Thụ, Quận Tân Bình, TP. Hồ Chí Minh",
                    RewardPoints = 400,
                    MembershipRank = "Vàng"
                },
                new Customer
                {
                    CustomerId = 9,
                    CustomerName = "Hồ Hải Long",
                    PhoneNumber = "0989012345",
                    Address = "15 Trường Chinh, Quận Tân Bình, TP. Hồ Chí Minh",
                    RewardPoints = 60,
                    MembershipRank = "Chuẩn"
                },
                new Customer
                {
                    CustomerId = 10,
                    CustomerName = "Ngô Thị Kim",
                    PhoneNumber = "0909112233",
                    Address = "34 Quang Trung, Quận Gò Vấp, TP. Hồ Chí Minh",
                    RewardPoints = 150,
                    MembershipRank = "Bạc"
                },
                new Customer
                {
                    CustomerId = 11,
                    CustomerName = "Dương Quốc Hưng",
                    PhoneNumber = "0918223344",
                    Address = "99 Lê Đức Thọ, Quận Gò Vấp, TP. Hồ Chí Minh",
                    RewardPoints = 310,
                    MembershipRank = "Vàng"
                },
                new Customer
                {
                    CustomerId = 12,
                    CustomerName = "Lý Tiểu Long",
                    PhoneNumber = "0927334455",
                    Address = "210 Kha Vạn Cân, TP. Thủ Đức, TP. Hồ Chí Minh",
                    RewardPoints = 80,
                    MembershipRank = "Chuẩn"
                },
                new Customer
                {
                    CustomerId = 13,
                    CustomerName = "Mai Thị Tuyết",
                    PhoneNumber = "0936445566",
                    Address = "77 Nguyễn Văn Quá, Quận 12, TP. Hồ Chí Minh",
                    RewardPoints = 220,
                    MembershipRank = "Bạc"
                },
                new Customer
                {
                    CustomerId = 14,
                    CustomerName = "Phan Văn Tài",
                    PhoneNumber = "0945556677",
                    Address = "140 Đặng Văn Bi, TP. Thủ Đức, TP. Hồ Chí Minh",
                    RewardPoints = 130,
                    MembershipRank = "Bạc"
                },
                new Customer
                {
                    CustomerId = 15,
                    CustomerName = "Đỗ Mỹ Linh",
                    PhoneNumber = "0954667788",
                    Address = "55 Hai Bà Trưng, Quận 1, TP. Hồ Chí Minh",
                    RewardPoints = 500,
                    MembershipRank = "Kim cương"
                }
            );
            // ==========================================
            // 4. NHÂN VIÊN DAILY FRESH
            // ==========================================
            modelBuilder.Entity<Employee>().HasData(
                new Employee
                {
                    EmployeeId = 1,
                    Username = "admin01",
                    FullName = "Nguyễn Minh Anh",
                    Role = "Admin",
                    PasswordHash = "SEED_ONLY_01"
                },
                new Employee
                {
                    EmployeeId = 2,
                    Username = "admin02",
                    FullName = "Trần Quốc Bảo",
                    Role = "Admin",
                    PasswordHash = "SEED_ONLY_02"
                },
                new Employee
                {
                    EmployeeId = 3,
                    Username = "cashier01",
                    FullName = "Phạm Ngọc Lan",
                    Role = "Cashier",
                    PasswordHash = "SEED_ONLY_03"
                },
                new Employee
                {
                    EmployeeId = 4,
                    Username = "cashier02",
                    FullName = "Võ Thị Thanh",
                    Role = "Cashier",
                    PasswordHash = "SEED_ONLY_04"
                },
                new Employee
                {
                    EmployeeId = 5,
                    Username = "cashier03",
                    FullName = "Đặng Minh Thư",
                    Role = "Cashier",
                    PasswordHash = "SEED_ONLY_05"
                },
                new Employee
                {
                    EmployeeId = 6,
                    Username = "cashier04",
                    FullName = "Bùi Gia Hân",
                    Role = "Cashier",
                    PasswordHash = "SEED_ONLY_06"
                },
                new Employee
                {
                    EmployeeId = 7,
                    Username = "cashier05",
                    FullName = "Ngô Khánh Linh",
                    Role = "Cashier",
                    PasswordHash = "SEED_ONLY_07"
                },
                new Employee
                {
                    EmployeeId = 8,
                    Username = "cashier06",
                    FullName = "Phan Tuấn Kiệt",
                    Role = "Cashier",
                    PasswordHash = "SEED_ONLY_08"
                },
                new Employee
                {
                    EmployeeId = 9,
                    Username = "cashier07",
                    FullName = "Đỗ Thảo Vy",
                    Role = "Cashier",
                    PasswordHash = "SEED_ONLY_09"
                },
                new Employee
                {
                    EmployeeId = 10,
                    Username = "cashier08",
                    FullName = "Hồ Minh Khang",
                    Role = "Cashier",
                    PasswordHash = "SEED_ONLY_10"
                },
                new Employee
                {
                    EmployeeId = 11,
                    Username = "warehouse01",
                    FullName = "Nguyễn Thành Đạt",
                    Role = "Warehouse",
                    PasswordHash = "SEED_ONLY_11"
                },
                new Employee
                {
                    EmployeeId = 12,
                    Username = "warehouse02",
                    FullName = "Trần Thị Kim Oanh",
                    Role = "Warehouse",
                    PasswordHash = "SEED_ONLY_12"
                },
                new Employee
                {
                    EmployeeId = 13,
                    Username = "warehouse03",
                    FullName = "Lê Quốc Huy",
                    Role = "Warehouse",
                    PasswordHash = "SEED_ONLY_13"
                },
                new Employee
                {
                    EmployeeId = 14,
                    Username = "warehouse04",
                    FullName = "Phạm Thị Thu Hà",
                    Role = "Warehouse",
                    PasswordHash = "SEED_ONLY_14"
                },
                new Employee
                {
                    EmployeeId = 15,
                    Username = "warehouse05",
                    FullName = "Vũ Minh Đức",
                    Role = "Warehouse",
                    PasswordHash = "SEED_ONLY_15"
                }
            );
        }
    }
}