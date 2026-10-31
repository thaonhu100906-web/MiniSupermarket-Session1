# Hỗ trợ hoàn thiện tiêu chí 36–50 — Daily Fresh

## Các thay đổi trong bản cập nhật

- **36–37:** Giữ kiến trúc `FormMainShell` với menu trái, header trên và form con `TopLevel = false`, `Dock = Fill` trong panel trung tâm.
- **38–40:** Giữ luồng đăng nhập, `SessionManager.JwtToken` và `ApiClientService` dùng chung.
- **39:** Thu ngân chỉ thấy menu Bán hàng POS; màn hình mặc định của thu ngân chuyển sang POS.
- **41:** Giữ màn hình quản lý sản phẩm và các thao tác CRUD hiện có.
- **42:** POS cộng dồn số lượng khi quét barcode; tính tổng giỏ; nhập tiền khách đưa; tính tiền thối theo số tiền cuối cùng sau đổi điểm; chặn số tiền âm/không hợp lệ/không đủ; API kiểm tra lại tiền nhận trước khi lưu hóa đơn; thông báo lỗi API ưu tiên hiển thị nội dung tiếng Việt trong thuộc tính `message`.
- **43:** Lưới sản phẩm tô màu đỏ nhạt cho ô tồn kho có `StockQuantity <= 10` bằng `CellFormatting`.
- **44:** Có `GET api/reports/dashboard-summary` và bốn thẻ KPI: doanh thu hôm nay, hóa đơn hôm nay, tổng khách hàng và số mặt hàng tồn kho thấp. Chưa đếm riêng khách hàng mới trong ngày do model chưa có ngày tạo.
- **45:** Có lọc khoảng ngày và biểu đồ cột doanh thu theo ngày. Chưa có lựa chọn gom theo tuần/tháng và chưa dùng stored procedure riêng cho biểu đồ; cần hoàn thiện nếu giảng viên chấm đúng các phần đó.
- **46:** Thêm `GET api/reports/best-sellers?take=5` và lưới Top 5 sản phẩm theo tổng số lượng bán.
- **47:** Thêm `RedeemPoints` cho checkout; quy đổi 1 điểm = 1.000 VNĐ, trừ điểm khách hàng và giảm tổng hóa đơn trong cùng transaction.
- **48:** Xuất CSV danh mục sản phẩm, hóa đơn theo khoảng ngày và doanh thu theo ngày theo khoảng ngày đã chọn.
- **49–50:** Cần thực hiện demo thực tế và luyện giải thích luồng code sau khi chạy thành công trên máy có SQL Server và .NET SDK.

## Cập nhật POS theo phản hồi giao diện

- Đã bỏ yêu cầu nhập số điện thoại/điểm bằng các hộp thoại InputBox rời rạc.
- Thêm tra cứu khách hàng theo số điện thoại tại màn hình POS, hiển thị tên/hạng/điểm trước khi nhập điểm đổi.
- Khách không tìm thấy sẽ nhận thông báo rõ ràng; không thể đổi điểm nếu chưa tra cứu hoặc nhập vượt số điểm hiện có.
- Sau thanh toán thành công, thông báo không còn in JSON thô; giỏ hàng và thông tin thanh toán được làm mới.

## Ghi chú quan trọng

Bản cập nhật được thực hiện trên mã nguồn trong ZIP người dùng gửi. Môi trường xử lý hiện tại không có `dotnet` CLI nên chưa thể chạy `dotnet build` hoặc kết nối SQL Server để xác nhận end-to-end. Hãy mở solution trên máy phát triển, build cả API và WinForms, xử lý lỗi build nếu có rồi mới dùng bản này để demo.

Dashboard hiện đếm tổng số khách hàng (đã đổi nhãn cho đúng dữ liệu thực tế), không giả mạo đây là số khách hàng mới trong ngày. Nếu đề bài bắt buộc số khách hàng mới trong ngày, cần bổ sung `CreatedAt` vào model, migration và cập nhật các bản ghi hiện hữu theo quy tắc thống nhất.

**Các phần cần người dùng kiểm tra trên máy:** build API và WinForms; chạy API + SQL Server; kiểm tra checkout thành công/thất bại, số tồn kho sau bán, KPI, biểu đồ, Top 5, xuất đủ 3 CSV. Tiêu chí 49–50 cần demo thực tế và thuyết trình; mã nguồn không thể tự chứng minh thay phần demo.


## Tiêu chí 45 - Biểu đồ doanh thu theo ngày
- API `GET /api/reports/revenue-by-day` gọi Stored Procedure `dbo.GetRevenueByDay`.
- Stored Procedure nhóm theo `CAST(OrderDate AS DATE)`, tính `SUM(TotalAmount)` và `COUNT(*)`.
- EF Core Migration `20261030160000_AddRevenueByDayStoredProcedure` tự tạo Stored Procedure khi cập nhật database mới.
- WinForms dùng `System.Windows.Forms.DataVisualization.Charting.Chart`, `SeriesChartType.Column`; dữ liệu được nạp từ API theo khoảng ngày chọn.
- Không tạo thêm bảng hoặc cột mới.
