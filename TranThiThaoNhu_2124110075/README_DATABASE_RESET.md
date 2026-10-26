# Hướng dẫn tạo lại database từ đầu

Yêu cầu: SQL Server Express đang chạy; `appsettings.json` trỏ đúng `localhost\\SQLEXPRESS`.

1. Mở solution trong Visual Studio.
2. Chọn `TranThiThaoNhu_2124110075.API` làm Startup Project.
3. Mở Tools → NuGet Package Manager → Package Manager Console. Chọn Default project là `TranThiThaoNhu_2124110075.API`.
4. Để xóa database cũ hoàn toàn: chạy `Drop-Database` và nhập `Y` xác nhận. Thao tác này xóa toàn bộ dữ liệu của database.
5. Chạy `Update-Database`. Chỉ cần chạy lệnh này một lần; không cần tạo migration mới.
6. Nhấn F5 chạy API. API sẽ tự chạy migration, tạo dữ liệu seed và tạo/cập nhật hai stored procedure từ thư mục `Scripts`.

Migration `FixSeedUserPasswordHashes` đã được sửa để không cập nhật bảng `Users` trước khi bảng `Employees` được đổi tên. Migration đổi tên bảng sẽ cập nhật password hash sau khi đổi tên.
