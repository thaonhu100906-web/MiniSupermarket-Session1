# MiniSupermarket – hướng dẫn khởi tạo

## Tạo lại cấu trúc và dữ liệu mẫu

1. Cấu hình chuỗi kết nối SQL Server trong `TranThiThaoNhu_2124110075/appsettings.json`.
2. Mở Terminal tại thư mục solution và chạy:

```powershell
dotnet restore
dotnet build
 dotnet run --project .\TranThiThaoNhu_2124110075\TranThiThaoNhu_2124110075.API.csproj
```

API tự chạy EF Core Migrations (`Database.Migrate()`), áp dụng dữ liệu mẫu `HasData`, cập nhật Barcode 13 chữ số cho 15 sản phẩm mẫu, sau đó tạo/cập nhật hai stored procedure từ thư mục `Scripts`. Không cần mở từng file SQL thủ công.

> Chỉ xóa database khi đã sao lưu dữ liệu cần giữ. Lệnh trên không tự xóa database; nó tạo/cập nhật schema và dữ liệu seed theo migrations. Để tái tạo hoàn toàn, xóa database `tranthithaonhudb` trong SQL Server rồi chạy API, với điều kiện tài khoản SQL có quyền tạo database.

## Phân trang API phía máy chủ

Các endpoint trả dữ liệu phân trang bằng `Skip/Take`, mặc định 10 dòng/trang, giới hạn `pageSize` 1–100:

- `GET /api/Products/paged?pageNumber=1&pageSize=10`
- `GET /api/Categories/paged?pageNumber=1&pageSize=10`
- `GET /api/Customers/paged?pageNumber=1&pageSize=10`
- `GET /api/Users/paged?pageNumber=1&pageSize=10` (Admin)
- `GET /api/Orders/paged?pageNumber=1&pageSize=10`

Có thể thêm `keyword` cho Products, Categories, Customers và Users. Endpoint sản phẩm trả `CategoryName` cùng mỗi dòng, không cần hiển thị CategoryId cho người dùng.

## Barcode

15 sản phẩm seed sử dụng mã EAN-13 gồm 13 chữ số. `Product.Barcode` vẫn lưu dạng chuỗi để không mất số 0 ở đầu. `ProductCode` nội bộ nếu có nên là trường riêng, không dùng thay Barcode.

## Tài khoản mẫu

Dùng các tài khoản seed đang được cấu hình trong `SupermarketDbContext`/migration. Không đưa mật khẩu thật hoặc secret triển khai lên GitHub.
