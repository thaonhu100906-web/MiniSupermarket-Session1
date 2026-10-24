BÁO CÁO BÀI THỰC HÀNH BUỔI 3 - LẬP TRÌNH ỨNG DỤNG .NET CORE

👤 THÔNG TIN CÁ NHÂN

Họ và tên: Trần Thị Thảo Như

Mã số sinh viên (MSSV): 2124110075

Lớp: CCQ2411C

Đề tài dự án: Hệ thống Quản lý Tiệm Bánh ngọt & Nước ép Daily Fresh

Port API mặc định: 5138 (Cấu hình Http trong launchSettings.json đã fix port 5138)

🎯 MỤC TIÊU BÀI THỰC HÀNH BUỔI 3

Buổi 3 tập trung vào việc hoàn thiện và nâng cấp toàn bộ hệ thống backend (.NET Core Web API) và frontend (WinForms App) cho dự án Daily Fresh:

Phân trang (Paging): Triển khai phân trang chuẩn RESTful cho danh sách Sản phẩm (Products) kết hợp tìm kiếm và lọc theo danh mục.

Xử lý chuỗi tìm kiếm không mất dòng đầu / mất khoảng trắng: Tối ưu hóa API và WinForms UI khi lọc dữ liệu realtime.

Chuyển đổi cấu trúc dữ liệu Barcode: Chuẩn hóa mã vạch sản phẩm sang định dạng số EAN-13 chuẩn siêu thị / tiệm bánh.

Cập nhật hệ thống Tài khoản & Phân quyền: Đổi tên thực thể Employees thành Users, tích hợp mã hóa mật khẩu Hash Password & Seed data chính xác cho Daily Fresh.

🛠 CẤU TRÚC DỰ ÁN (SOLUTION STRUCTURE)

Solution TranThiThaoNhu_2124110075.sln bao gồm 2 project chính:

TranThiThaoNhu_2124110075/
├── TranThiThaoNhu_2124110075/                     # Backend API (.NET Core)
│   ├── Controllers/                               # API Controllers
│   │   ├── AuthController.cs                      # Xử lý đăng nhập / xác thực
│   │   ├── CategoryController.cs                  # Quan lý Danh mục Bánh & Nước ép
│   │   ├── CustomersController.cs                 # Quản lý Khách hàng thân thiết
│   │   ├── OrdersController.cs                    # Quản lý Hóa đơn & Chi tiết hóa đơn
│   │   ├── ProductsController.cs                  # API Sản phẩm (có Phân trang PagedResult)
│   │   └── UsersController.cs                     # Quản lý Tài khoản người dùng / Nhân viên
│   ├── Data/
│   │   └── SupermarketDbContext.cs                # DbContext EF Core (kết nối SQL Server)
│   ├── DTOs/                                      # Data Transfer Objects
│   │   ├── Products/                              # ProductCreateDto, ProductUpdateDto, PagedResultDto, ProductListItemDto
│   │   ├── Categories/                            # CategoryRequestDto
│   │   ├── Customers/                             # CustomerCreateDto, CustomerUpdateDto
│   │   ├── Orders/                                # CheckoutRequestDto, OrderListItemDto
│   │   └── Users/                                 # CreateUserDto, UpdateUserDto
│   ├── Migrations/                                # Các bản Migration EF Core đã áp dụng
│   │   ├── 20261023190000_RenameEmployeesToUsers.cs
│   │   └── 20261024100000_ConvertBarcodeToNumericEan13.cs
│   ├── Models/                                    # Real entities (Category, Product, Customer, User, Order, OrderDetail)
│   ├── Scripts/                                   # SQL Stored Procedures & Scripts hỗ trợ
│   └── Services/                                  # ADO.NET Search Service & Business logic
│
└── MiniSupermarket.WinForms/                      # Frontend Desktop Client (WinForms C#)
    ├── ApiClientService.cs                        # HTTP Client kết nối tới API port 5138
    ├── DailyFreshTheme.cs                         # Giao diện Pastel / Theme thương hiệu Daily Fresh
    ├── PagedListResponse.cs                       # Model hứng dữ liệu phân trang từ API
    ├── SessionManager.cs                          # Quản lý phiên làm việc & thông tin User đăng nhập
    ├── FormLogin.cs                               # Màn hình Đăng nhập
    ├── FormMainShell.cs                           # Giao diện chính (MDI / Navigation)
    ├── FormPOS.cs                                 # Màn hình Bán hàng tại quầy (POS)
    ├── FormProductManagement.cs                   # Màn hình Quản lý Bánh & Nước ép (Tích hợp phân trang)
    ├── FormCategoryManagement.cs                  # Màn hình Quản lý Danh mục
    ├── FormCustomerManagement.cs                  # Màn hình Quản lý Khách hàng
    ├── FormAccountManagement.cs                   # Màn hình Quản lý Tài khoản / Nhân viên
    └── FormQuickReport.cs                         # Báo cáo doanh thu ca làm việc


🚀 CÁC TÍNH NĂNG CHÍNH ĐÃ HOÀN THÀNH TRONG BUỔI 3

1. Phân trang & Tìm kiếm Sản phẩm (API & WinForms)

API Backend: API GET /api/Products hỗ trợ nhận tham số pageNumber, pageSize, search, categoryId.

Trả về chuẩn DTO: Đóng gói thông qua PagedResultDto<T> gồm:

Items: Danh sách sản phẩm của trang hiện tại.

TotalCount: Tổng số lượng bản ghi thỏa điều kiện.

PageNumber & PageSize: Trang hiện tại và kích thước trang.

TotalPages: Tổng số trang được tính tự động.

WinForms UI: FormProductManagement có bộ nút chuyển trang (Trang đầu, Trang trước, Trang sau, Trang cuối) và hiển thị chỉ số trang rõ ràng.

2. Sửa lỗi Mất dòng đầu / Khoảng trắng trong GridView

Cập nhật thuật toán bind dữ liệu trong ApiClientService và WinForms DataGridView giúp hiển thị đúng dòng đầu tiên khi danh sách trả về từ API phân trang.

3. Chuẩn hóa Mã vạch EAN-13 (Numeric Barcode)

Áp dụng Migration ConvertBarcodeToNumericEan13: Tự động chuyển đổi các mã barcode cũ thành chuỗi số chuẩn 13 chữ số (EAN-13) cho sản phẩm bánh ngọt & nước ép.

4. Quản lý Tài khoản & Phân quyền (Users & Authentication)

Đổi tên thực thể từ Employee sang User để mở rộng hệ thống.

Seed dữ liệu khởi tạo tài khoản quản lý và nhân viên cho Daily Fresh, hỗ trợ mã hóa mật khẩu bảo mật và đăng nhập chuẩn API.

💻 HƯỚNG DẪN CHẠY DỰ ÁN

1. Khởi động API (Backend)

Mở file solution TranThiThaoNhu_2124110075.sln bằng Visual Studio 2022.

Đảm bảo kết nối CSDL trong appsettings.json trỏ đúng vào SQL Server của bạn.

Mở Package Manager Console, cập nhật CSDL (nếu cần):

Update-Database


Đặt project TranThiThaoNhu_2124110075 làm Startup Project và bấm F5 hoặc chọn profile Http để chạy.

URL Swagger API mặc định: http://localhost:5138/swagger/index.html

2. Khởi động WinForms Client (Frontend)

Đặt project MiniSupermarket.WinForms làm Startup Project (hoặc chạy Multiple Startup Projects).

Đảm bảo ApiClientService.cs đang kết nối đúng địa chỉ API: http://localhost:5138/.

Khởi chạy dự án WinForms, đăng nhập bằng tài khoản nhân viên / quản lý đã seed sẵn để sử dụng đầy đủ các chức năng quản lý và POS bán bánh/nước ép.
