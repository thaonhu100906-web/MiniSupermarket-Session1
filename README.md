🛒 README – MINI SUPERMARKET DAILY FRESH
🌿 1. Giới thiệu dự án
Mini Supermarket Daily Fresh là phần mềm quản lý siêu thị mini, hỗ trợ quản lý các hoạt động bán hàng, sản phẩm, khách hàng, nhân viên và báo cáo doanh thu.

Hệ thống được xây dựng theo mô hình ứng dụng gồm hai phần chính:
- Backend API: Xử lý nghiệp vụ, dữ liệu và phân quyền người dùng.
- WinForms: Giao diện dành cho nhân viên sử dụng trực tiếp.
🎯 Mục tiêu
- Quản lý sản phẩm, danh mục và khách hàng.
- Hỗ trợ bán hàng tại quầy POS.
- Quản lý tài khoản và vai trò nhân viên.
- Theo dõi hóa đơn và doanh thu.
- Hỗ trợ tìm kiếm sản phẩm bằng mã vạch.
- Phân quyền truy cập theo từng loại tài khoản.

🧰 2. Công nghệ sử dụng
Công nghệ	Vai trò
C#	Ngôn ngữ lập trình chính
.NET 8	Nền tảng phát triển
Windows Forms	Xây dựng giao diện
ASP.NET Core Web API	Xử lý nghiệp vụ
Entity Framework Core	Kết nối và thao tác cơ sở dữ liệu
SQL Server	Hệ quản trị cơ sở dữ liệu
JWT	Xác thực và phân quyền
Swagger	Kiểm tra API


📁 3. Cấu trúc thư mục dự án
TranThiThaoNhu_2124110075/
│
├── TranThiThaoNhu_2124110075.sln
│
├── TranThiThaoNhu_2124110075/
│   └── TranThiThaoNhu_2124110075.API.csproj
│
├── MiniSupermarket.WinForms/
│   └── MiniSupermarket.WinForms.csproj
│
└── Các thư mục và tệp cấu hình liên quan


Chức năng các thành phần:
- *.sln: File Solution dùng để mở toàn bộ dự án.
- *.API.csproj: Dự án Backend API.
- MiniSupermarket.WinForms.csproj: Dự án giao diện Windows Forms.
- appsettings.json: Cấu hình kết nối cơ sở dữ liệu và ứng dụng.
- Data/: Chứa DbContext và cấu hình dữ liệu.
- Migrations/: Chứa các phiên bản thay đổi cấu trúc cơ sở dữ liệu.
- Controllers/: Chứa các API xử lý nghiệp vụ.

💻 4. Yêu cầu môi trường
Trước khi chạy dự án, cần chuẩn bị:
Phần mềm	Yêu cầu
Visual Studio	2022
.NET SDK	.NET 8
SQL Server	SQL Server Express hoặc phiên bản tương thích
SQL Server Management Studio	Dùng để quản lý cơ sở dữ liệu
Windows	Cần thiết để chạy ứng dụng WinForms

📌 Lưu ý: Dự án WinForms sử dụng net8.0-windows, vì vậy cần chạy trên môi trường Windows.

⚙️ 5. Cấu hình cơ sở dữ liệu
Bước 1: Kiểm tra SQL Server
Mở SQL Server Management Studio (SSMS), kết nối đến SQL Server đang sử dụng.

Theo cấu hình hiện tại, chuỗi kết nối mặc định là:
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=tranthithaonhudb;Trusted_Connection=True;TrustServerCertificate=True;"
}


Bước 2: Điều chỉnh chuỗi kết nối
Mở file appsettings.json trong dự án API.

Nếu SQL Server của máy sử dụng tên instance khác, hãy thay đổi:
- Server: Tên máy chủ SQL Server.
- Database: Tên cơ sở dữ liệu.
- Trusted_Connection: Phương thức xác thực Windows.
- TrustServerCertificate: Cho phép tin cậy chứng chỉ máy chủ trong môi trường phát triển.
Bước 3: Khởi tạo cơ sở dữ liệu
Khi chạy API, chương trình có gọi:
db.Database.Migrate();


Do đó, Entity Framework Core sẽ áp dụng các migration hiện có để tạo hoặc cập nhật cấu trúc cơ sở dữ liệu.

📌 Cần đảm bảo SQL Server đang hoạt động và tài khoản kết nối có đủ quyền tạo, cập nhật database.

🚀 6. Hướng dẫn chạy dự án
🔹 Bước 1: Giải nén source code
Giải nén file ZIP vào thư mục dễ tìm, ví dụ:
D:\Projects\MiniSupermarket\


🔹 Bước 2: Mở dự án
Mở file:
TranThiThaoNhu_2124110075.sln


bằng Visual Studio 2022.
🔹 Bước 3: Restore NuGet Packages
Trong Visual Studio:
1. Nhấn chuột phải vào Solution.
2. Chọn Restore NuGet Packages.
3. Chờ Visual Studio tải các thư viện cần thiết.
4. Kiểm tra lại lỗi build nếu có.
🔹 Bước 4: Chạy Backend API
Chọn dự án API làm Startup Project.

Nhấn:
F5


hoặc:
Ctrl + F5


API đang được cấu hình sử dụng cổng:
http://localhost:5138


Có thể truy cập Swagger tại:

🔗 http://localhost:5138/swagger

Nếu Swagger hiển thị danh sách API, Backend đã khởi động thành công.
🔹 Bước 5: Chạy ứng dụng WinForms
Giữ API hoạt động.

Tiếp theo:
1. Chọn MiniSupermarket.WinForms làm Startup Project.
2. Kiểm tra địa chỉ API trong ApiClientService.cs.
3. Đảm bảo địa chỉ đang trỏ đến:
http://localhost:5138/api/


4. Nhấn F5 để chạy ứng dụng.
🔹 Bước 6: Đăng nhập
Nhập tài khoản và mật khẩu hợp lệ đã được cấu hình trong cơ sở dữ liệu.

Sau khi đăng nhập thành công, hệ thống sẽ hiển thị giao diện tương ứng với quyền của tài khoản.

🧩 7. Các chức năng chính
🏠 7.1. Trang chủ
- Hiển thị giao diện chính của hệ thống.
- Điều hướng đến các chức năng theo quyền người dùng.
- Hiển thị menu chức năng tương ứng với vai trò tài khoản.
🛍️ 7.2. Quản lý sản phẩm
- Hiển thị danh sách sản phẩm.
- Xem thông tin chi tiết sản phẩm.
- Tìm kiếm sản phẩm.
- Tra cứu bằng mã vạch.
- Theo dõi thông tin giá và số lượng tồn kho.
🧾 7.3. Bán hàng POS
- Tìm kiếm và chọn sản phẩm.
- Thêm sản phẩm vào hóa đơn.
- Điều chỉnh số lượng.
- Tính tổng tiền thanh toán.
- Lưu hóa đơn sau khi hoàn tất giao dịch.
👥 7.4. Quản lý khách hàng
- Xem danh sách khách hàng.
- Tìm kiếm khách hàng.
- Tra cứu thông tin khách hàng.
- Quản lý thông tin khách hàng theo quyền được cấp.
📊 7.5. Báo cáo doanh thu
- Theo dõi thông tin doanh thu.
- Xem dữ liệu hóa đơn.
- Tra cứu chi tiết từng hóa đơn.
- Hỗ trợ kiểm tra thông tin giao dịch.
👤 7.6. Quản lý tài khoản
- Quản lý tài khoản nhân viên.
- Phân loại vai trò người dùng.
- Kiểm soát quyền truy cập các chức năng.

🔐 8. Phân quyền người dùng
Hệ thống sử dụng cơ chế phân quyền theo vai trò.
Chức năng	Admin	Cashier	Warehouse
Đăng nhập	✅	✅	✅
POS	✅	✅	Theo quyền
Xem sản phẩm	✅	✅	Theo quyền
Thêm sản phẩm	✅	❌	Theo quyền
Sửa sản phẩm	✅	❌	Theo quyền
Xóa sản phẩm	✅	❌	Theo quyền
Xem báo cáo doanh thu	✅	❌	Theo quyền
Xem chi tiết hóa đơn	✅	✅*	Theo quyền
Quản lý tài khoản	✅	❌	❌

* Cashier được định hướng chỉ xem chi tiết hóa đơn, không xem các thống kê doanh thu tổng hợp, biểu đồ hoặc danh sách sản phẩm bán chạy.

📌 Quyền truy cập thực tế còn phụ thuộc vào kiểm tra quyền tại Backend API, không chỉ việc ẩn hoặc hiện menu trên giao diện.

🧱 9. Các API chính
Controller	Chức năng
AuthController	Đăng nhập và xác thực
ProductsController	Quản lý sản phẩm
CategoryController	Quản lý danh mục
CustomersController	Quản lý khách hàng
UsersController	Quản lý tài khoản
OrdersController	Xử lý hóa đơn và bán hàng
ReportsController	Xử lý báo cáo

Các API được tổ chức theo mô hình RESTful, sử dụng HTTP request để trao đổi dữ liệu giữa WinForms và Backend.

🏷️ 10. Tìm kiếm và mã vạch
🔍 Tìm kiếm sản phẩm
Hệ thống hỗ trợ tìm kiếm sản phẩm theo từ khóa nhằm giúp nhân viên nhanh chóng tìm được sản phẩm cần thiết.
🏷️ Mã vạch
Chức năng mã vạch hỗ trợ:
- Nhận diện sản phẩm thông qua mã barcode.
- Tra cứu sản phẩm nhanh hơn.
- Hạn chế thao tác tìm kiếm thủ công.
- Hỗ trợ quá trình bán hàng tại quầy POS.

📄 11. Phân trang dữ liệu
Đối với các bảng có nhiều dữ liệu, hệ thống sử dụng phân trang để:
- Chia dữ liệu thành nhiều trang.
- Giảm lượng dữ liệu cần hiển thị cùng lúc.
- Giúp giao diện dễ quan sát hơn.
- Tăng hiệu quả khi thao tác với danh sách lớn.
Người dùng có thể chuyển trang để xem các bản ghi tiếp theo hoặc quay lại trang trước đó.

🧪 12. Kiểm thử hệ thống
Sau khi chạy ứng dụng, nên kiểm tra lần lượt các chức năng:
STT	Nội dung kiểm thử	Kết quả mong đợi
1	Đăng nhập Admin	Đăng nhập thành công
2	Đăng nhập Cashier	Đăng nhập thành công
3	Đăng nhập Warehouse	Đăng nhập thành công
4	Xem danh sách sản phẩm	Hiển thị dữ liệu
5	Tìm kiếm sản phẩm	Trả về kết quả phù hợp
6	Tra cứu barcode	Tìm đúng sản phẩm
7	Thực hiện bán hàng POS	Tạo hóa đơn thành công
8	Xem chi tiết hóa đơn	Hiển thị đúng thông tin
9	Kiểm tra phân quyền	Chỉ hiển thị chức năng được phép
10	Chuyển trang	Dữ liệu thay đổi đúng trang


🛠️ 13. Một số lỗi thường gặp
❌ Không kết nối được SQL Server
Nguyên nhân có thể:
- SQL Server chưa được khởi động.
- Sai tên Server hoặc Instance.
- Chuỗi kết nối không chính xác.
- Tài khoản Windows không có quyền truy cập.
Cách xử lý:
- Kiểm tra trạng thái SQL Server.
- Kiểm tra lại ConnectionStrings trong appsettings.json.
- Thử kết nối trực tiếp bằng SSMS.
❌ Không truy cập được API
Nguyên nhân có thể:
- API chưa được chạy.
- Cổng API không đúng.
- WinForms đang trỏ đến sai địa chỉ.
Cách xử lý:
- Chạy API trước.
- Kiểm tra Swagger.
- Kiểm tra BaseAddress trong ApiClientService.cs.
❌ Lỗi thiếu thư viện NuGet
Cách xử lý:
- Restore NuGet Packages.
- Kiểm tra phiên bản .NET SDK.
- Build lại Solution.
❌ Không đăng nhập được
Nguyên nhân có thể:
- Tài khoản không tồn tại.
- Mật khẩu không chính xác.
- Dữ liệu tài khoản chưa được khởi tạo.
- API chưa hoạt động.
Cách xử lý:
- Kiểm tra dữ liệu tài khoản trong SQL Server.
- Kiểm tra phản hồi API đăng nhập.
- Kiểm tra trạng thái Backend.

🔒 14. Lưu ý bảo mật và dữ liệu
- Không chia sẻ tài khoản quản trị cho người không có thẩm quyền.
- Không lưu mật khẩu dưới dạng văn bản thuần.
- Kiểm tra quyền truy cập ở cả giao diện và Backend.
- Không tự ý chỉnh sửa dữ liệu trực tiếp trong database khi hệ thống đang hoạt động.
- Sao lưu database định kỳ.
- Không đưa thông tin kết nối hoặc JWT Secret lên môi trường công khai.

📌 15. Tổng kết
Mini Supermarket Daily Fresh hỗ trợ quản lý các hoạt động cơ bản của một siêu thị mini, từ quản lý sản phẩm, khách hàng, bán hàng đến theo dõi hóa đơn và báo cáo.

Hệ thống được xây dựng bằng C# và .NET 8, sử dụng SQL Server để lưu trữ dữ liệu, ASP.NET Core Web API để xử lý nghiệp vụ và Windows Forms để cung cấp giao diện người dùng.

Việc phân quyền theo vai trò giúp kiểm soát chức năng mà mỗi nhân viên được phép sử dụng, góp phần nâng cao tính bảo mật và hiệu quả trong quá trình vận hành.

📘 README – Mini Supermarket Daily Fresh · Phiên bản tài liệu hướng dẫn sử dụng và triển khai

















ko đc m ơi, có 200 mấy dòng vậy, chữ tiêu đồ to lên, cây thư mục đồ nhiều lên, điểm nhấn lên, icon đồ nhiều vô













À hiểu rồi m =))) Bản trước nhìn giống tài liệu ghi chú hơn là README hoàn chỉnh. T sẽ làm lại theo kiểu README chuyên nghiệp, nổi bật như trang giới thiệu dự án:
- Tiêu đề chính thật lớn, chia cấp heading rõ ràng.
- Dùng nhiều icon để mỗi phần dễ nhận diện.
- Cây thư mục chi tiết, thụt cấp rõ ràng.
- Có mục lục, bảng công nghệ, sơ đồ hoạt động, hướng dẫn cài đặt từng bước.
- Làm nổi bật lưu ý, lệnh chạy, cấu hình và phân quyền.
- Nội dung dài và đầy đủ hơn, không viết kiểu vài dòng cho có.
Mình viết trực tiếp ở đây, không tạo file nữa nha.
🛒 MINI SUPERMARKET DAILY FRESH

📑 MỤC LỤC
STT	Nội dung	Mô tả
01	📌 Giới thiệu	Tổng quan về dự án
02	🎯 Mục tiêu	Mục đích xây dựng hệ thống
03	✨ Chức năng	Các chức năng chính
04	🧰 Công nghệ	Công nghệ và thư viện
05	🏗️ Kiến trúc	Mô hình hoạt động
06	📁 Cấu trúc thư mục	Giải thích source code
07	💻 Yêu cầu hệ thống	Phần mềm cần cài đặt
08	⚙️ Cấu hình	Thiết lập kết nối
09	🗄️ Database	Khởi tạo cơ sở dữ liệu
10	🚀 Hướng dẫn chạy	Chạy Backend và WinForms
11	👤 Hướng dẫn sử dụng	Các thao tác trên phần mềm
12	🔐 Phân quyền	Quyền hạn từng vai trò
13	🔎 Tìm kiếm và Barcode	Tìm kiếm, quét mã vạch
14	📄 Phân trang	Hiển thị dữ liệu
15	🧪 Kiểm thử	Danh sách kiểm tra
16	🛠️ Xử lý lỗi	Các lỗi thường gặp
17	🔒 Bảo mật	Lưu ý khi vận hành
18	👩‍💻 Thông tin dự án	Thông tin bàn giao


1. 📌 GIỚI THIỆU DỰ ÁN
Mini Supermarket Daily Fresh là hệ thống quản lý siêu thị mini được xây dựng nhằm hỗ trợ các hoạt động quản lý và vận hành cửa hàng.

Phần mềm cung cấp các chức năng quản lý sản phẩm, danh mục, khách hàng, tài khoản nhân viên, bán hàng tại quầy POS và theo dõi thông tin hóa đơn.

Hệ thống được phát triển bằng ngôn ngữ C#, sử dụng nền tảng .NET 8, cơ sở dữ liệu SQL Server và giao diện Windows Forms. Backend được xây dựng dưới dạng ASP.NET Core Web API để xử lý nghiệp vụ và trao đổi dữ liệu với ứng dụng.
🌱 DAILY FRESH – QUẢN LÝ SIÊU THỊ ĐƠN GIẢN, HIỆU QUẢ
🛍️
Sản phẩm
Quản lý và tra cứu hàng hóa

🧾
Bán hàng
Thanh toán và tạo hóa đơn

👥
Khách hàng
Tra cứu thông tin khách hàng

📊
Báo cáo
Theo dõi hóa đơn và doanh thu



1.1. Thông tin tổng quan
Thuộc tính	Thông tin
Tên dự án	Mini Supermarket Daily Fresh
Loại ứng dụng	Phần mềm quản lý siêu thị mini
Ngôn ngữ lập trình	C#
Nền tảng	.NET 8
Giao diện	Windows Forms
Backend	ASP.NET Core Web API
Cơ sở dữ liệu	Microsoft SQL Server
Kiến trúc	WinForms kết nối Web API

1.2. Đối tượng sử dụng
Hệ thống hướng đến ba nhóm người dùng chính:
- 👑 Admin: Quản lý và kiểm soát toàn bộ hoạt động của hệ thống.
- 🧾 Cashier: Nhân viên thu ngân, thực hiện bán hàng và tra cứu thông tin được cấp quyền.
- 📦 Warehouse: Nhân viên kho, sử dụng các chức năng liên quan theo quyền được cấu hình.

2. 🎯 MỤC TIÊU XÂY DỰNG
Dự án được xây dựng với các mục tiêu:
📦 Quản lý hàng hóa
- Lưu trữ thông tin sản phẩm.
- Quản lý danh mục sản phẩm.
- Theo dõi giá bán và mã vạch.
- Hỗ trợ tìm kiếm sản phẩm nhanh chóng.
🧾 Hỗ trợ bán hàng
- Tạo hóa đơn bán hàng.
- Chọn sản phẩm và điều chỉnh số lượng.
- Tính toán tổng tiền.
- Lưu thông tin giao dịch.
👥 Quản lý khách hàng
- Lưu thông tin khách hàng.
- Tìm kiếm khách hàng.
- Tra cứu thông tin phục vụ bán hàng.
📊 Theo dõi hoạt động kinh doanh
- Quản lý thông tin hóa đơn.
- Xem chi tiết giao dịch.
- Hỗ trợ theo dõi doanh thu theo chức năng được cấp quyền.
🔐 Kiểm soát truy cập
- Phân quyền theo vai trò.
- Kiểm soát chức năng trên giao diện.
- Kiểm tra quyền truy cập tại Backend API.

3. ✨ CÁC CHỨC NĂNG CHÍNH
3.1. 🏠 Trang chủ
Trang chủ là giao diện trung tâm sau khi người dùng đăng nhập thành công.

Chức năng:
- Hiển thị các khu vực chức năng.
- Điều hướng giữa các màn hình.
- Hiển thị menu theo vai trò tài khoản.
- Hỗ trợ người dùng truy cập nhanh các nghiệp vụ.
3.2. 🛍️ Quản lý sản phẩm
Chức năng quản lý sản phẩm hỗ trợ:
- Hiển thị danh sách sản phẩm.
- Xem thông tin sản phẩm.
- Tìm kiếm theo từ khóa.
- Tra cứu sản phẩm bằng barcode.
- Theo dõi thông tin giá bán và dữ liệu sản phẩm.
- Phân trang danh sách.
3.3. 🗂️ Quản lý danh mục
- Hiển thị danh sách danh mục.
- Xem thông tin danh mục.
- Quản lý danh mục theo quyền tài khoản.
- Hỗ trợ phân loại sản phẩm.
3.4. 🧾 Bán hàng POS
POS là khu vực hỗ trợ nhân viên thực hiện giao dịch tại quầy.

Các thao tác chính:
1. Tìm kiếm hoặc quét mã sản phẩm.
2. Chọn sản phẩm cần bán.
3. Thêm sản phẩm vào danh sách thanh toán.
4. Điều chỉnh số lượng.
5. Kiểm tra thông tin đơn hàng.
6. Tính tổng tiền.
7. Xác nhận thanh toán.
8. Lưu hóa đơn vào hệ thống.
3.5. 👥 Quản lý khách hàng
- Xem danh sách khách hàng.
- Tìm kiếm thông tin khách hàng.
- Tra cứu thông tin theo số điện thoại.
- Hỗ trợ thao tác khách hàng trong quá trình bán hàng.
3.6. 👤 Quản lý tài khoản
- Quản lý thông tin người dùng.
- Tạo tài khoản nhân viên.
- Cập nhật thông tin tài khoản.
- Thiết lập vai trò.
- Kiểm soát quyền truy cập.
3.7. 📊 Báo cáo và hóa đơn
Khu vực báo cáo hỗ trợ xem thông tin giao dịch và hóa đơn.

Các thông tin liên quan:
- Danh sách hóa đơn.
- Thông tin chi tiết hóa đơn.
- Sản phẩm trong hóa đơn.
- Số lượng từng sản phẩm.
- Đơn giá và thành tiền.
- Tổng tiền của hóa đơn.

4. 🧰 CÔNG NGHỆ SỬ DỤNG
Công nghệ	Vai trò trong dự án
💻 C#	Ngôn ngữ lập trình
⚙️ .NET 8	Nền tảng phát triển ứng dụng
🖥️ Windows Forms	Xây dựng giao diện Desktop
🌐 ASP.NET Core Web API	Xử lý nghiệp vụ và cung cấp API
🗄️ SQL Server	Lưu trữ dữ liệu
🔗 Entity Framework Core	Truy vấn và quản lý dữ liệu
🔑 JWT Bearer	Xác thực người dùng
📚 Swagger	Kiểm tra và mô tả API
📦 NuGet	Quản lý thư viện phụ thuộc

4.1. Một số thư viện quan trọng
- Microsoft.EntityFrameworkCore.SqlServer: Kết nối SQL Server.
- Microsoft.EntityFrameworkCore.Design: Hỗ trợ migration.
- Microsoft.AspNetCore.Authentication.JwtBearer: Xác thực bằng JWT.
- Swashbuckle.AspNetCore: Tích hợp Swagger.

5. 🏗️ KIẾN TRÚC HỆ THỐNG
Hệ thống được chia thành hai phần chính:
🖥️ WINDOWS FORMS
MiniSupermarket.WinForms
Giao diện người dùng · Nhận thao tác · Hiển thị dữ liệu
HTTP / JSON / JWT

🌐 ASP.NET CORE WEB API
TranThiThaoNhu_2124110075
Controllers · DTOs · Services · Authentication
Entity Framework Core

🗄️ SQL SERVER
tranthithaonhudb
Lưu trữ sản phẩm · Khách hàng · Người dùng · Hóa đơn

5.1. Luồng hoạt động
1. Người dùng mở ứng dụng WinForms.
2. Người dùng nhập thông tin đăng nhập.
3. WinForms gửi yêu cầu đến API.
4. API xác thực thông tin tài khoản.
5. API trả kết quả đăng nhập và thông tin xác thực.
6. WinForms sử dụng thông tin phiên đăng nhập để gọi các API tương ứng.
7. API kiểm tra quyền truy cập.
8. Backend xử lý nghiệp vụ và truy vấn SQL Server.
9. Dữ liệu được trả về cho WinForms để hiển thị.

6. 📁 CẤU TRÚC THƯ MỤC DỰ ÁN
Dưới đây là cấu trúc source code chính của phiên bản dự án.
**/
📦 TranThiThaoNhu_2124110075
│
├── 📄 TranThiThaoNhu_2124110075.sln
│
├── 📄 README.md
├── 📄 HO_TRO_TIEU_CHI_36_50.md
│
├── 📂 TranThiThaoNhu_2124110075
│   │
│   ├── 📄 TranThiThaoNhu_2124110075.API.csproj
│   ├── 📄 Program.cs
│   ├── 📄 appsettings.json
│   ├── 📄 appsettings.Development.json
│   ├── 📄 README_DATABASE_RESET.md
│   │
│   ├── 📂 Controllers
│   │   ├── 📄 AuthController.cs
│   │   ├── 📄 ProductsController.cs
│   │   ├── 📄 CategoryController.cs
│   │   ├── 📄 CustomersController.cs
│   │   ├── 📄 UsersController.cs
│   │   ├── 📄 OrdersController.cs
│   │   ├── 📄 ReportsController.cs
│   │   └── 📄 WeatherForecastController.cs
│   │
│   ├── 📂 Data
│   │   └── 📄 SupermarketDbContext.cs
│   │
│   ├── 📂 Models
│   │   ├── 📄 Product.cs
│   │   ├── 📄 Category.cs
│   │   ├── 📄 Customer.cs
│   │   ├── 📄 User.cs
│   │   ├── 📄 Order.cs
│   │   └── 📄 OrderDetail.cs
│   │
│   ├── 📂 DTOs
│   │   │
│   │   ├── 📂 Categories
│   │   │   └── 📄 CategoryRequestDto.cs
│   │   │
│   │   ├── 📂 Products
│   │   │   ├── 📄 ProductCreateDto.cs
│   │   │   ├── 📄 ProductUpdateDto.cs
│   │   │   ├── 📄 ProductListItemDto.cs
│   │   │   └── 📄 PagedResultDto.cs
│   │   │
│   │   ├── 📂 Customers
│   │   │   ├── 📄 CustomerCreateDto.cs
│   │   │   └── 📄 CustomerUpdateDto.cs
│   │   │
│   │   ├── 📂 Orders
│   │   │   ├── 📄 OrderListItemDto.cs
│   │   │   └── 📄 CheckoutRequestDto.cs
│   │   │
│   │   └── 📂 Users
│   │       ├── 📄 CreateUserDto.cs
│   │       └── 📄 UpdateUserDto.cs
│   │
│   ├── 📂 Services
│   │   └── 📄 ProductAdoSearchService.cs
│   │
│   ├── 📂 Scripts
│   │   ├── 📄 Products_SearchByKeyword.sql
│   │   ├── 📄 ShiftRevenueReport.sql
│   │   ├── 📄 RevenueByDayReport.sql
│   │   └── 📄 AddCustomerCreatedAt_ForDashboard.sql
│   │
│   ├── 📂 Migrations
│   │   ├── 📄 InitialCreateDatabase
│   │   ├── 📄 AddProductAndCustomer
│   │   ├── 📄 UpdateDatabase
│   │   ├── 📄 AddEmployees
│   │   ├── 📄 AddOrdersAndOrderDetails
│   │   ├── 📄 AlignDailyFreshData
│   │   ├── 📄 RenameEmployeesToUsers
│   │   ├── 📄 FixSeedUserPasswordHashes
│   │   ├── 📄 ConvertBarcodeToNumericEan13
│   │   ├── 📄 AddRevenueByDayStoredProcedure
│   │   └── 📄 SupermarketDbContextModelSnapshot.cs
│   │
│   └── 📂 Properties
│       └── 📄 launchSettings.json
│
└── 📂 MiniSupermarket.WinForms
    │
    ├── 📄 MiniSupermarket.WinForms.csproj
    ├── 📄 Program.cs
    ├── 📄 ApiClientService.cs
    ├── 📄 SessionManager.cs
    ├── 📄 PagedListResponse.cs
    ├── 📄 DailyFreshTheme.cs
    │
    ├── 📄 FormLogin.cs
    ├── 📄 FormLogin.Designer.cs
    ├── 📄 FormLogin.resx
    │
    ├── 📄 FormMainShell.cs
    ├── 📄 FormMainShell.Designer.cs
    ├── 📄 FormMainShell.resx
    │
    ├── 📄 FormPOS.cs
    ├── 📄 FormPOS.Designer.cs
    ├── 📄 FormPOS.resx
    │
    ├── 📄 FormProductManagement.cs
    ├── 📄 FormProductManagement.Designer.cs
    ├── 📄 FormProductManagement.resx
    │
    ├── 📄 FormCategoryManagement.cs
    ├── 📄 FormCategoryManagement.Designer.cs
    ├── 📄 FormCategoryManagement.resx
    │
    ├── 📄 FormCustomerManagement.cs
    ├── 📄 FormCustomerManagement.Designer.cs
    ├── 📄 FormCustomerManagement.resx
    │
    ├── 📄 FormAccountManagement.cs
    ├── 📄 FormAccountManagement.Designer.cs
    ├── 📄 FormAccountManagement.resx
    │
    ├── 📄 FormQuickReport.cs
    ├── 📄 FormQuickReport.Designer.cs
    ├── 📄 FormQuickReport.resx
    │
    └── 📄 Form1.cs
/**

6.1. Giải thích thư mục Backend
Thư mục / File	Chức năng
Controllers/	Tiếp nhận và xử lý HTTP request
Data/	Khởi tạo DbContext và cấu hình dữ liệu
Models/	Định nghĩa các thực thể dữ liệu
DTOs/	Định nghĩa dữ liệu đầu vào và đầu ra API
Services/	Chứa các lớp hỗ trợ xử lý nghiệp vụ
Scripts/	Chứa các câu lệnh SQL hỗ trợ
Migrations/	Quản lý thay đổi cấu trúc database
Properties/	Cấu hình chạy ứng dụng

6.2. Giải thích thư mục WinForms
File	Chức năng
Program.cs	Điểm bắt đầu ứng dụng
ApiClientService.cs	Gửi request đến Backend API
SessionManager.cs	Quản lý phiên đăng nhập
PagedListResponse.cs	Cấu trúc dữ liệu phân trang
DailyFreshTheme.cs	Giao diện và màu sắc ứng dụng
FormLogin.cs	Màn hình đăng nhập
FormMainShell.cs	Giao diện chính và menu
FormPOS.cs	Màn hình bán hàng
FormProductManagement.cs	Quản lý sản phẩm
FormCategoryManagement.cs	Quản lý danh mục
FormCustomerManagement.cs	Quản lý khách hàng
FormAccountManagement.cs	Quản lý tài khoản
FormQuickReport.cs	Màn hình báo cáo

💡 Lưu ý: Các file .Designer.cs chứa phần thiết kế giao diện được tạo bởi Windows Forms Designer. Các file .resx lưu tài nguyên giao diện. Không nên tự ý xóa những file này vì có thể làm lỗi giao diện.


7. 💻 YÊU CẦU HỆ THỐNG
Trước khi cài đặt, cần chuẩn bị các công cụ sau:
Phần mềm	Phiên bản / Yêu cầu
Windows	Môi trường chạy WinForms
Visual Studio	Visual Studio 2022
.NET SDK	.NET 8
SQL Server	SQL Server Express hoặc phiên bản tương thích
SSMS	SQL Server Management Studio
Internet	Cần khi tải NuGet lần đầu

7.1. Kiểm tra .NET SDK
Mở Terminal hoặc Command Prompt và nhập:
dotnet --version


Kiểm tra danh sách SDK:
dotnet --list-sdks


Nếu chưa có .NET 8 SDK, cần cài đặt trước khi build dự án.
7.2. Kiểm tra SQL Server
Mở SQL Server Management Studio và thử kết nối đến instance đang sử dụng.

Cần đảm bảo:
- SQL Server đã khởi động.
- Instance name chính xác.
- Phương thức xác thực phù hợp.
- Tài khoản có quyền truy cập database.

8. ⚙️ CẤU HÌNH DỰ ÁN
8.1. Giải nén source code
Giải nén file ZIP vào một thư mục trên máy tính.

Ví dụ:
D:\
└── Projects\
    └── MiniSupermarket\
        └── TranThiThaoNhu_2124110075\
            ├── TranThiThaoNhu_2124110075.sln
            ├── TranThiThaoNhu_2124110075\
            └── MiniSupermarket.WinForms\


8.2. Cấu hình chuỗi kết nối
Mở file:
TranThiThaoNhu_2124110075/
└── TranThiThaoNhu_2124110075/
    └── appsettings.json


Tìm phần ConnectionStrings.

Cấu hình hiện tại:
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=tranthithaonhudb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}


📝 Ý nghĩa các thông số
Thông số	Ý nghĩa
Server	Địa chỉ SQL Server
localhost\\SQLEXPRESS	SQL Server Express trên máy hiện tại
Database	Tên cơ sở dữ liệu
Trusted_Connection=True	Sử dụng Windows Authentication
TrustServerCertificate=True	Tin cậy chứng chỉ trong môi trường phát triển

Nếu máy sử dụng SQL Server instance khác, hãy thay đổi Server cho phù hợp.

Ví dụ:
Server=localhost


hoặc:
Server=DESKTOP-ABC123\SQLEXPRESS


⚠️ Không thay đổi các thông số database nếu chưa xác định rõ cấu hình SQL Server của máy.
8.3. Kiểm tra địa chỉ API trong WinForms
Mở file:
MiniSupermarket.WinForms/
└── ApiClientService.cs


Địa chỉ API được cấu hình:
http://localhost:5138/api/


Backend và WinForms phải sử dụng cùng một địa chỉ.

9. 🗄️ CƠ SỞ DỮ LIỆU
9.1. Thông tin database
Thuộc tính	Giá trị
Hệ quản trị	SQL Server
Database	tranthithaonhudb
ORM	Entity Framework Core
DbContext	SupermarketDbContext
Cơ chế cập nhật	EF Core Migrations

9.2. Các nhóm dữ liệu chính
Nhóm dữ liệu	Mục đích
👤 Users	Lưu thông tin tài khoản nhân viên
🛍️ Products	Lưu thông tin sản phẩm
🗂️ Categories	Lưu danh mục sản phẩm
👥 Customers	Lưu thông tin khách hàng
🧾 Orders	Lưu thông tin hóa đơn
📋 OrderDetails	Lưu chi tiết từng sản phẩm trong hóa đơn

9.3. Entity Framework Core Migrations
Dự án sử dụng Migration để quản lý các thay đổi trong cấu trúc database.

Khi Backend khởi động, chương trình gọi:
db.Database.Migrate();


Điều này giúp hệ thống tự động áp dụng các migration đang có khi database chưa cập nhật đến phiên bản mới nhất.
9.4. Cập nhật database thủ công
Nếu cần cập nhật database bằng Terminal, mở thư mục chứa project API và sử dụng:
dotnet ef database update


Nếu máy chưa cài công cụ EF:
dotnet tool install --global dotnet-ef


Sau đó chạy lại:
dotnet ef database update


⚠️ Quan trọng: Không tự ý xóa database đang sử dụng nếu chưa sao lưu. Việc xóa database có thể làm mất toàn bộ dữ liệu sản phẩm, khách hàng, tài khoản và hóa đơn.


10. 🚀 HƯỚNG DẪN CHẠY DỰ ÁN
QUY TRÌNH KHUYẾN NGHỊ
Thứ tự chạy: SQL Server → Backend API → WinForms
10.1. Bước 1 – Mở Solution
Mở thư mục source code vừa giải nén.

Nhấp đúp vào:
TranThiThaoNhu_2124110075.sln


Hoặc mở Visual Studio 2022, chọn:

File → Open → Project/Solution

Sau đó chọn file .sln.
10.2. Bước 2 – Restore NuGet Packages
Trong Visual Studio:
1. Nhấp chuột phải vào Solution.
2. Chọn Restore NuGet Packages.
3. Chờ quá trình tải thư viện hoàn tất.
4. Kiểm tra Output nếu có lỗi.
Có thể sử dụng Terminal:
dotnet restore


10.3. Bước 3 – Build Solution
Trong Visual Studio:

Build → Build Solution

Hoặc nhấn:
Ctrl + Shift + B


Nếu build thành công, Visual Studio sẽ hiển thị thông báo:
Build succeeded.


10.4. Bước 4 – Chạy Backend API
Chọn project:
TranThiThaoNhu_2124110075.API


Đặt làm Startup Project.

Nhấn F5 hoặc Ctrl + F5.

API được cấu hình sử dụng địa chỉ:
http://localhost:5138


🔎 Kiểm tra Swagger
Mở trình duyệt và truy cập:

http://localhost:5138/swagger

Nếu Swagger hiển thị danh sách API, Backend đã khởi động thành công.
Dấu hiệu API hoạt động

- Không có lỗi kết nối SQL Server.
- Không có lỗi khởi tạo database.
- Ứng dụng API đang chạy.
- Swagger truy cập được.

10.5. Bước 5 – Chạy WinForms
Giữ Backend API đang hoạt động.

Trong Visual Studio:
1. Nhấp chuột phải vào MiniSupermarket.WinForms.
2. Chọn Set as Startup Project.
3. Kiểm tra ApiClientService.cs.
4. Đảm bảo địa chỉ API đúng.
5. Nhấn F5 để chạy.
10.6. Bước 6 – Đăng nhập
Khi màn hình đăng nhập xuất hiện:
1. Nhập username.
2. Nhập password.
3. Nhấn nút đăng nhập.
4. Chờ hệ thống xác thực.
5. Kiểm tra giao diện được hiển thị theo vai trò.

11. 👩‍💻 HƯỚNG DẪN SỬ DỤNG
11.1. 🔑 Đăng nhập
Màn hình đăng nhập là nơi người dùng xác thực trước khi sử dụng hệ thống.

Các bước:
1. Mở ứng dụng Daily Fresh.
2. Nhập tên tài khoản.
3. Nhập mật khẩu.
4. Nhấn Đăng nhập.
5. Hệ thống kiểm tra thông tin.
6. Nếu hợp lệ, chuyển đến giao diện chính.
11.2. 🏠 Sử dụng trang chủ
Sau khi đăng nhập, người dùng được chuyển đến giao diện chính.

Các menu hiển thị phụ thuộc vào quyền của tài khoản.

Người dùng có thể:
- Mở màn hình POS.
- Truy cập quản lý sản phẩm.
- Truy cập quản lý khách hàng.
- Xem báo cáo nếu được cấp quyền.
- Đăng xuất khỏi hệ thống.
11.3. 🛍️ Quản lý sản phẩm
Quy trình xem sản phẩm:
1. Chọn menu Quản lý sản phẩm.
2. Hệ thống tải danh sách sản phẩm.
3. Sử dụng ô tìm kiếm để tra cứu.
4. Chọn sản phẩm để xem thông tin.
5. Sử dụng phân trang nếu danh sách có nhiều dữ liệu.
Thông tin sản phẩm có thể bao gồm:
- Mã sản phẩm.
- Tên sản phẩm.
- Danh mục.
- Barcode.
- Giá bán.
- Số lượng tồn kho.
11.4. 🧾 Thực hiện bán hàng
Quy trình bán hàng cơ bản:
🛒 Chọn sản phẩm
       │
       ▼
🔎 Tìm kiếm / Quét barcode
       │
       ▼
➕ Thêm vào giỏ hàng
       │
       ▼
🔢 Kiểm tra số lượng
       │
       ▼
💰 Tính tổng tiền
       │
       ▼
✅ Xác nhận thanh toán
       │
       ▼
🧾 Lưu hóa đơn


Lưu ý: Cần kiểm tra kỹ sản phẩm và số lượng trước khi xác nhận thanh toán.
11.5. 👥 Tra cứu khách hàng
1. Chọn menu khách hàng hoặc chức năng tìm khách hàng tại POS.
2. Nhập thông tin cần tra cứu.
3. Nhấn tìm kiếm.
4. Kiểm tra kết quả.
5. Chọn khách hàng phù hợp nếu cần gắn với giao dịch.
11.6. 📊 Xem chi tiết hóa đơn
1. Mở khu vực báo cáo hoặc tra cứu hóa đơn.
2. Tìm hóa đơn cần xem.
3. Chọn hóa đơn.
4. Mở phần chi tiết.
5. Kiểm tra các sản phẩm và số tiền trong giao dịch.

12. 🔐 PHÂN QUYỀN NGƯỜI DÙNG
Hệ thống sử dụng vai trò để kiểm soát các chức năng.
12.1. 👑 Admin – Quản trị viên
Admin là vai trò có quyền quản lý hệ thống.

Các nhóm chức năng:
- Quản lý sản phẩm.
- Quản lý danh mục.
- Quản lý khách hàng.
- Quản lý tài khoản.
- Thực hiện bán hàng.
- Xem báo cáo.
- Theo dõi thông tin hóa đơn.
12.2. 🧾 Cashier – Nhân viên thu ngân
Cashier tập trung vào nghiệp vụ bán hàng tại quầy.

Các quyền theo yêu cầu:
- ✅ Truy cập POS.
- ✅ Xem danh sách sản phẩm.
- ✅ Xem thông tin chi tiết sản phẩm.
- ❌ Không được xóa sản phẩm.
- ✅ Xem khu vực báo cáo hóa đơn ở phạm vi được cấp.
- ✅ Chỉ xem chi tiết hóa đơn.
- ❌ Không xem báo cáo tổng hợp doanh thu, biểu đồ hoặc thống kê khác nếu không được cấp quyền.
12.3. 📦 Warehouse – Nhân viên kho
Warehouse sử dụng các chức năng liên quan đến dữ liệu kho theo cấu hình quyền của hệ thống.

Quyền cụ thể cần căn cứ vào cấu hình role hiện tại của Backend và giao diện.
12.4. 📋 Bảng tổng hợp phân quyền
Chức năng	👑 Admin	🧾 Cashier	📦 Warehouse
Đăng nhập	✅	✅	✅
POS	✅	✅	Theo cấu hình
Xem sản phẩm	✅	✅	Theo cấu hình
Thêm sản phẩm	✅	❌	Theo cấu hình
Sửa sản phẩm	✅	❌	Theo cấu hình
Xóa sản phẩm	✅	❌	Theo cấu hình
Quản lý tài khoản	✅	❌	❌
Xem chi tiết hóa đơn	✅	✅*	Theo cấu hình
Xem báo cáo tổng hợp	✅	❌	Theo cấu hình

* Cashier chỉ xem chi tiết hóa đơn, không đồng nghĩa với quyền xem toàn bộ báo cáo doanh thu.
🔐 Lưu ý kỹ thuật: Ẩn menu trên WinForms không thay thế cho việc kiểm tra quyền tại API. Backend cần xác thực role cho từng endpoint tương ứng. Cấu hình quyền trên giao diện và API phải thống nhất để tránh trường hợp người dùng không thấy menu nhưng vẫn có thể gọi trực tiếp API.


13. 🔎 TÌM KIẾM SẢN PHẨM VÀ BARCODE
13.1. Tìm kiếm sản phẩm
Hệ thống hỗ trợ tìm kiếm sản phẩm theo từ khóa.

Chức năng giúp:
- Tìm sản phẩm nhanh.
- Hạn chế việc phải dò danh sách thủ công.
- Hỗ trợ nhân viên khi thao tác tại POS.
- Kết hợp với phân trang khi có nhiều dữ liệu.
Backend có service:
Services/
└── ProductAdoSearchService.cs


SQL script hỗ trợ tìm kiếm:
Scripts/
└── Products_SearchByKeyword.sql


13.2. Mã vạch sản phẩm
Barcode được sử dụng để nhận diện sản phẩm.

Quy trình:
1. Nhập hoặc quét mã barcode.
2. Gửi mã đến chức năng tra cứu.
3. Backend tìm sản phẩm tương ứng.
4. Trả về thông tin sản phẩm.
5. Giao diện hiển thị kết quả.
💡 Lợi ích
- Tăng tốc độ thao tác bán hàng.
- Giảm nhầm lẫn khi chọn sản phẩm.
- Hỗ trợ tra cứu chính xác.
- Tiết kiệm thời gian cho nhân viên.

14. 📄 PHÂN TRANG DỮ LIỆU
Đối với danh sách có nhiều bản ghi, hệ thống sử dụng cơ chế phân trang.
14.1. Mục đích
- Giảm lượng dữ liệu hiển thị cùng lúc.
- Giúp giao diện dễ quan sát.
- Hạn chế tải quá nhiều dữ liệu trong một lần.
- Hỗ trợ thao tác trên danh sách lớn.
14.2. Cấu trúc dữ liệu phân trang
Backend có DTO:
DTOs/
└── Products/
    └── PagedResultDto.cs


WinForms có lớp:
PagedListResponse.cs


14.3. Quy trình phân trang
📋 Yêu cầu danh sách
        │
        ▼
🔢 Xác định trang hiện tại
        │
        ▼
📦 API truy vấn dữ liệu
        │
        ▼
🗄️ SQL Server trả kết quả
        │
        ▼
📄 API trả dữ liệu phân trang
        │
        ▼
🖥️ WinForms hiển thị danh sách



15. 🧪 KIỂM THỬ HỆ THỐNG
Sau khi chạy dự án, nên kiểm tra các nhóm chức năng sau.
15.1. Kiểm thử đăng nhập
STT	Trường hợp	Kết quả mong đợi
1	Đăng nhập đúng thông tin	Đăng nhập thành công
2	Sai mật khẩu	Thông báo lỗi
3	Tài khoản không tồn tại	Không cho đăng nhập
4	Đăng xuất	Kết thúc phiên đăng nhập

15.2. Kiểm thử sản phẩm
STT	Trường hợp	Kết quả mong đợi
1	Mở danh sách sản phẩm	Hiển thị dữ liệu
2	Tìm kiếm sản phẩm	Trả về kết quả phù hợp
3	Tra cứu barcode	Tìm đúng sản phẩm
4	Chuyển trang	Hiển thị trang tương ứng
5	Cashier xem sản phẩm	Được phép xem
6	Cashier xóa sản phẩm	Không được phép

15.3. Kiểm thử POS
STT	Trường hợp	Kết quả mong đợi
1	Thêm sản phẩm vào giỏ	Sản phẩm được thêm
2	Thay đổi số lượng	Thành tiền cập nhật
3	Tính tổng hóa đơn	Tổng tiền chính xác
4	Thanh toán	Tạo hóa đơn thành công
5	Xem chi tiết hóa đơn	Hiển thị đúng dữ liệu

15.4. Kiểm thử phân quyền
STT	Tài khoản	Nội dung kiểm tra
1	Admin	Truy cập các chức năng quản trị
2	Cashier	Truy cập POS
3	Cashier	Xem sản phẩm
4	Cashier	Không được xóa sản phẩm
5	Cashier	Chỉ xem chi tiết hóa đơn
6	Warehouse	Kiểm tra các chức năng theo quyền


16. 🛠️ XỬ LÝ CÁC LỖI THƯỜNG GẶP
16.1. ❌ Lỗi không kết nối được SQL Server
Nguyên nhân:
- SQL Server chưa chạy.
- Sai tên instance.
- Sai chuỗi kết nối.
- Không có quyền truy cập.
Cách xử lý:
1. Mở SQL Server Configuration Manager.
2. Kiểm tra SQL Server Service.
3. Mở SSMS và thử kết nối.
4. Kiểm tra appsettings.json.
5. Chạy lại Backend API.
16.2. ❌ Lỗi không truy cập được Swagger
Nguyên nhân:
- API chưa khởi động.
- Cổng đang sử dụng bị thay đổi.
- Ứng dụng gặp lỗi khi khởi tạo.
Cách xử lý:
- Kiểm tra Output trong Visual Studio.
- Kiểm tra cổng API.
- Kiểm tra kết nối database.
- Chạy lại API.
16.3. ❌ WinForms không kết nối được API
Nguyên nhân:
- Backend chưa chạy.
- Sai BaseAddress.
- Cổng API không khớp.
- Request bị lỗi xác thực.
Cách xử lý:

Mở:
MiniSupermarket.WinForms/
└── ApiClientService.cs


Kiểm tra địa chỉ:
http://localhost:5138/api/


Đảm bảo API đang chạy trước khi mở WinForms.
16.4. ❌ Lỗi thiếu package
Thực hiện:
dotnet restore


Sau đó:
dotnet build


Nếu vẫn lỗi, kiểm tra phiên bản .NET SDK và các package trong file .csproj.
16.5. ❌ Lỗi database chưa cập nhật
Chạy lệnh:
dotnet ef database update


Đảm bảo Terminal đang ở đúng project API hoặc chỉ định đúng project khi chạy lệnh.
16.6. ❌ Đăng nhập không thành công
Kiểm tra:
- Username.
- Password.
- Trạng thái tài khoản.
- Kết nối API.
- Dữ liệu tài khoản trong database.
- Kết quả trả về từ API đăng nhập.

17. 🔒 BẢO MẬT VÀ LƯU Ý VẬN HÀNH
🔑 Tài khoản và mật khẩu
- Không chia sẻ tài khoản quản trị.
- Không lưu mật khẩu dạng văn bản thuần.
- Không sử dụng tài khoản cá nhân của người khác.
- Đăng xuất sau khi kết thúc ca làm việc.
🗄️ Dữ liệu
- Sao lưu database định kỳ.
- Kiểm tra dữ liệu trước khi cập nhật.
- Không tự ý xóa bảng hoặc dữ liệu.
- Kiểm tra hóa đơn trước khi thực hiện các thao tác nghiệp vụ.
🔐 Phân quyền
- Không chỉ kiểm tra quyền trên giao diện.
- Backend phải kiểm tra role trước khi xử lý request.
- Không cho phép người dùng truy cập endpoint ngoài quyền hạn.
- Cập nhật quyền cần kiểm tra đồng bộ cả API và WinForms.
⚙️ Cấu hình
- Không công khai thông tin kết nối database.
- Không chia sẻ JWT Secret.
- Không đưa thông tin nhạy cảm vào Git công khai.
- Kiểm tra cấu hình trước khi triển khai trên máy khác.

18. 👩‍💻 THÔNG TIN DỰ ÁN
🌿 MINI SUPERMARKET DAILY FRESH
PROJECT DOCUMENTATION
Hệ thống quản lý siêu thị mini

Thông tin	Giá trị
Tên Solution	TranThiThaoNhu_2124110075
Backend	ASP.NET Core Web API
Frontend	Windows Forms
Ngôn ngữ	C#
Framework	.NET 8
Database	SQL Server
Giao tiếp	HTTP / JSON


📌 Tài liệu bao gồm
- Giới thiệu và kiến trúc hệ thống.
- Cấu trúc source code.
- Hướng dẫn cài đặt môi trường.
- Hướng dẫn cấu hình database.
- Hướng dẫn chạy chương trình.
- Hướng dẫn sử dụng chức năng.
- Phân quyền người dùng.
- Kiểm thử và xử lý lỗi.


🎉 KẾT LUẬN
Mini Supermarket Daily Fresh được xây dựng nhằm hỗ trợ quản lý và vận hành siêu thị mini thông qua các chức năng quản lý sản phẩm, khách hàng, bán hàng, hóa đơn và phân quyền người dùng.

Với kiến trúc WinForms kết hợp ASP.NET Core Web API và SQL Server, hệ thống tách biệt phần giao diện với phần xử lý nghiệp vụ, giúp việc quản lý và phát triển ứng dụng thuận tiện hơn.

Tài liệu README này cung cấp các thông tin cần thiết để người đọc có thể tìm hiểu cấu trúc dự án, thiết lập môi trường, khởi tạo cơ sở dữ liệu và chạy ứng dụng.

🌱 DAILY FRESH – SMART MANAGEMENT, BETTER SHOPPING!
