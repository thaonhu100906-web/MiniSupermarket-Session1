# Daily Fresh – Ứng dụng quản lý tiệm bánh ngọt và nước ép

## 1. Giới thiệu

Daily Fresh là ứng dụng quản lý cửa hàng được xây dựng bằng C# và .NET. Dự án gồm ứng dụng desktop Windows Forms dành cho người sử dụng và ASP.NET Core Web API làm trung gian xử lý dữ liệu, kết nối cơ sở dữ liệu SQL Server.

## 2. Công nghệ sử dụng

- **Ngôn ngữ:** C#
- **Nền tảng:** .NET 8
- **Giao diện desktop:** Windows Forms
- **Backend:** ASP.NET Core Web API
- **Cơ sở dữ liệu:** Microsoft SQL Server
- **ORM:** Entity Framework Core 8
- **Xác thực:** JWT Bearer Token
- **Tài liệu API:** Swagger / OpenAPI

## 3. Chức năng chính

- Đăng nhập và xác thực người dùng bằng JWT.
- Quản lý sản phẩm.
- Quản lý danh mục sản phẩm.
- Quản lý khách hàng.
- Quản lý tài khoản người dùng và phân quyền.
- Bán hàng tại quầy (POS), lập hóa đơn và lưu chi tiết đơn hàng.
- Tra cứu, tìm kiếm và phân trang dữ liệu.
- Xem báo cáo doanh thu theo ca.

## 4. Cấu trúc dự án

```text
TranThiThaoNhu_2124110075/
├── MiniSupermarket.WinForms/                 # Ứng dụng giao diện Windows Forms
│   ├── FormLogin.cs                           # Màn hình đăng nhập
│   ├── FormMainShell.cs                       # Giao diện chính
│   ├── FormPOS.cs                              # Màn hình bán hàng
│   ├── FormProductManagement.cs                # Quản lý sản phẩm
│   ├── FormCategoryManagement.cs               # Quản lý danh mục
│   ├── FormCustomerManagement.cs               # Quản lý khách hàng
│   ├── FormAccountManagement.cs                # Quản lý tài khoản
│   ├── FormQuickReport.cs                      # Báo cáo nhanh
│   ├── ApiClientService.cs                     # Gọi API
│   └── MiniSupermarket.WinForms.csproj
│
├── TranThiThaoNhu_2124110075/
│   ├── Controllers/                            # Các API Controller
│   ├── Data/                                   # DbContext và dữ liệu mẫu
│   ├── Models/                                 # Các lớp thực thể
│   ├── Migrations/                             # EF Core Migrations
│   ├── Scripts/                                # Stored Procedure SQL
│   ├── Services/                               # Các dịch vụ xử lý
│   ├── appsettings.json                        # Cấu hình API và kết nối DB
│   └── TranThiThaoNhu_2124110075.API.csproj
│
└── TranThiThaoNhu_2124110075.sln               # Solution
```

## 5. Yêu cầu cài đặt

Trước khi chạy dự án, cần chuẩn bị:

1. Windows 10/11.
2. Visual Studio 2022 hoặc .NET 8 SDK.
3. SQL Server Express hoặc SQL Server Developer.
4. SQL Server Management Studio (SSMS) để kiểm tra cơ sở dữ liệu (không bắt buộc).

## 6. Hướng dẫn cài đặt và chạy chương trình

### Bước 1: Giải nén source code

Giải nén file ZIP vào thư mục trên máy tính.

### Bước 2: Cấu hình SQL Server

Mở file:

```text
TranThiThaoNhu_2124110075/appsettings.json
```

Kiểm tra chuỗi kết nối hiện tại:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=tranthithaonhudb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

Nếu máy sử dụng SQL Server instance khác, thay `localhost\\SQLEXPRESS` bằng tên server tương ứng. Đảm bảo SQL Server đang chạy và tài khoản Windows có quyền kết nối.

### Bước 3: Chạy Backend API

Mở Terminal hoặc PowerShell tại thư mục chứa file solution, sau đó chạy:

```powershell
dotnet restore
dotnet build
dotnet run --project .\TranThiThaoNhu_2124110075\TranThiThaoNhu_2124110075.API.csproj --launch-profile http
```

Khi API khởi động, chương trình sẽ tự động:

- Kết nối SQL Server.
- Áp dụng các EF Core Migrations để tạo/cập nhật cấu trúc cơ sở dữ liệu.
- Khởi tạo dữ liệu mẫu được khai báo trong DbContext/Migrations.
- Tạo hoặc cập nhật các Stored Procedure từ thư mục `Scripts`.

Sau khi chạy thành công, mở Swagger tại:

```text
http://localhost:5138/swagger
```

**Lưu ý:** Giữ cửa sổ Terminal đang chạy API. Ứng dụng Windows Forms cần API hoạt động để đăng nhập và tải dữ liệu.

### Bước 4: Chạy ứng dụng Windows Forms

Mở một cửa sổ Terminal/PowerShell khác tại thư mục gốc của source code và chạy:

```powershell
dotnet run --project .\MiniSupermarket.WinForms\MiniSupermarket.WinForms.csproj
```

Hoặc mở file solution `TranThiThaoNhu_2124110075.sln` bằng Visual Studio 2022, chọn `MiniSupermarket.WinForms` làm Startup Project và nhấn **Start**.

Ứng dụng Windows Forms đang cấu hình gọi API tại địa chỉ:

```text
http://localhost:5138/api/
```

Vì vậy, nếu thay đổi cổng chạy API thì cần cập nhật địa chỉ tương ứng trong `ApiClientService.cs` và `FormLogin.cs`.

## 7. Phân trang API

Các API danh sách hỗ trợ phân trang với tham số `pageNumber` và `pageSize`:

| Chức năng | Endpoint |
|---|---|
| Sản phẩm | `GET /api/Products/paged?pageNumber=1&pageSize=10` |
| Danh mục | `GET /api/Categories/paged?pageNumber=1&pageSize=10` |
| Khách hàng | `GET /api/Customers/paged?pageNumber=1&pageSize=10` |
| Tài khoản | `GET /api/Users/paged?pageNumber=1&pageSize=10` |
| Đơn hàng | `GET /api/Orders/paged?pageNumber=1&pageSize=10` |

Một số endpoint hỗ trợ thêm tham số `keyword` để tìm kiếm dữ liệu.

## 8. Một số API khác

- `POST /api/auth/login`: đăng nhập và nhận JWT.
- `GET /api/Products/search`: tìm kiếm sản phẩm.
- `GET /api/Products/ado-search`: tìm kiếm sản phẩm bằng ADO.NET.
- `POST /api/Orders/checkout`: thực hiện thanh toán đơn hàng.
- `GET /api/Orders/shift-revenue`: xem báo cáo doanh thu theo ca.

Có thể xem chi tiết tham số và kết quả trả về của từng API trong Swagger.

## 9. Lưu ý khi sử dụng

- Cần bật SQL Server trước khi chạy Backend API.
- Cần chạy Backend API trước khi mở và sử dụng Windows Forms.
- Không tự ý đổi cổng API nếu chưa cập nhật địa chỉ trong ứng dụng desktop.
- Không xóa cơ sở dữ liệu nếu còn dữ liệu cần giữ. API tự động áp dụng migrations khi khởi động, nhưng không tự xóa database hiện có.
- Tài khoản đăng nhập mẫu được khai báo trong phần seed dữ liệu của `Data/SupermarketDbContext.cs`. Sử dụng thông tin tài khoản được cung cấp kèm bài thực hành hoặc do giảng viên hướng dẫn.

## 10. Tác giả

- **Họ và tên:** Trần Thị Thảo Như
- **MSSV:** 2124110075
- **Lớp:** CCQ2411C
- **Môn học:** Lập trình ứng dụng
