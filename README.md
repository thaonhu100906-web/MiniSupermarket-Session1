ĐỒ ÁN MÔN HỌC: LẬP TRÌNH ỨNG DỤNG .NET CORE

HỆ THỐNG QUẢN LÝ SIÊU THỊ MINI (MINISUPERMARKET SYSTEM)

👤 THÔNG TIN SINH VIÊN

Họ và tên: Trần Thị Thảo Như

Mã số sinh viên (MSSV): 2124110075

Lớp: CCQ2411C

Tên môn học: Lập trình Ứng dụng .NET Core

Học kỳ/Năm học: HK2 - 2025/2026

📌 TỔNG QUAN VỀ BÀI THỰC HÀNH BUỔI 2

Chủ đề: Bảo mật & Phân quyền JWT (JSON Web Token) cho Web API & Tích hợp WinForms Client.

🎯 Mục tiêu bài học

Lý thuyết:

Nắm vững cơ chế Stateless Authentication (Xác thực không trạng thái) với JWT.

Phân biệt rõ sự khác nhau giữa Authentication (Xác thực danh tính) và Authorization (Phân quyền truy cập).

Ứng dụng Data Annotations & ModelState trong việc validate dữ liệu đầu vào tại Backend.

Thực hành:

Tích hợp AuthController cấp phát Token trong dự án ASP.NET Core Web API (MiniSupermarket.API).

Cấu hình phân quyền với các thuộc tính [Authorize] và [Authorize(Roles = "...")].

Cập nhật ứng dụng WinForms Client (MiniSupermarket.WinForms) với màn hình đăng nhập FormLogin, lưu trữ phiên qua SessionManager và đính kèm Bearer Token vào HTTP Header khi gọi API.

🏗 KẾT CẤU DỰ ÁN (PROJECT STRUCTURE)

MiniSupermarket/
│
├── MiniSupermarket.API/                    # ASP.NET Core Web API (Backend)
│   ├── Controllers/
│   │   ├── AuthController.cs               # Controller cấp phát và kiểm tra JWT Token
│   │   └── CategoriesController.cs         # Controller quản lý danh mục (đã bảo vệ bằng [Authorize])
│   ├── Dtos/                               # Data Transfer Objects
│   ├── Program.cs                          # Cấu hình Services, JwtBearer Authentication & Middleware
│   └── appsettings.json                    # Cấu hình Secret Key cho JWT
│
└── MiniSupermarket.WinForms/               # Windows Forms App (Frontend Client)
    ├── FormLogin.cs                        # Giao diện và logic Đăng nhập
    ├── FormCategoryManagement.cs           # Giao diện Quản lý danh mục sản phẩm
    ├── SessionManager.cs                   # Quản lý phiên làm việc tạm thời (Lưu JwtToken, CurrentRole)
    └── Program.cs                          # Điểm khởi chạy chính ứng dụng (chạy FormLogin đầu tiên)


🔑 NỘI DUNG KỸ THUẬT NỔI BẬT

1. Kiến trúc JWT Stateless Authentication

Header: Chứa thuật toán mã hóa (HS256).

Payload (Claims): Lưu thông tin định danh người dùng (Username, Role, thời hạn hết hạn).

Signature: Khóa bí mật (SymmetricSecurityKey) dùng để ký và xác thực tính toàn vẹn của Token, chống giả mạo.

2. Phân quyền Người dùng (Role-Based Access Control)

Admin: Toàn quyền quản trị hệ thống, truy cập toàn bộ Endpoint (bao gồm cả /admin-dashboard).

Cashier (Thu ngân): Chỉ truy cập được các chức năng hỗ trợ bán hàng, POS (Endpoint /staff-pos).

🚀 HƯỚNG DẪN CÀI ĐẶT VÀ CHẠY ỨNG DỤNG

🛠 Yêu cầu môi trường

Visual Studio 2022 (hoặc mới hơn) hỗ trợ .NET 8.0 / .NET 10.0

SDK .NET tương ứng

1. Cài đặt các gói NuGet cần thiết (Backend)

Mở dự án MiniSupermarket.API và cài đặt 2 gói thư viện sau:

System.IdentityModel.Tokens.Jwt

Microsoft.AspNetCore.Authentication.JwtBearer

Lưu ý: Nếu gặp lỗi cache NuGet trong quá trình cài đặt, chạy lệnh sau trong Terminal/PowerShell:

dotnet nuget locals all --clear


2. Khởi chạy Web API (Backend)

Đặt MiniSupermarket.API làm Startup Project.

Nhấn F5 hoặc chọn Run.

Swagger UI sẽ tự động khởi chạy tại địa chỉ dạng: https://localhost:7123/swagger

3. Tài khoản kiểm thử mặc định (Demo Credentials)

Tài khoản (Username)

Mật khẩu (Password)

Vai trò (Role)

Quyền hạn

admin

123456

Admin

Quyền quản trị tối cao, gọi được tất cả API

cashier

123456

Cashier

Quyền nhân viên thu ngân, bị giới hạn API Admin

🧪 KỊCH BẢN KIỂM CHỨNG BẢO MẬT (TEST CASES)

Test Case 1: Kiểm tra API khi chưa xác thực

Thao tác: Mở Swagger UI, gọi trực tiếp API GET /api/categories mà chưa thực hiện đăng nhập.

Kết quả: Trả về lỗi 401 Unauthorized.

Test Case 2: Kiểm tra phân quyền Admin vs Cashier

Gọi API POST /api/auth/login với tài khoản cashier / 123456 để lấy token.

Copy Token thu được, dán vào nút Authorize ở góc trên Swagger theo định dạng: Bearer <token>.

Gọi thử GET /api/categories/staff-pos ➔ Thành công (200 OK).

Gọi thử GET /api/categories/admin-dashboard ➔ Bị từ chối (403 Forbidden) do tài khoản Cashier không đủ thẩm quyền.

🗺 ĐỊNH HƯỚNG PHÁT TRIỂN TIẾP THEO (BUỔI 3)

Tích hợp SQL Server & Entity Framework Core (EF Core) vào dự án Backend.

Chuyển đổi dữ liệu từ dạng In-Memory sang cơ sở dữ liệu quan hệ hoàn chỉnh (Code-First Migration).

Xây dựng các bảng dữ liệu cốt lõi: Users, Categories, Products, Customers, Orders, OrderDetails.

Viết truy vấn LINQ nâng cao đảm bảo tính toàn vẹn dữ liệu (ACID) cho giao dịch bán hàng.
