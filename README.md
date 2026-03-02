# TheaterTickets (VB.NET WinForms + PostgreSQL)

## Yêu cầu hệ thống
- .NET SDK 9.0
- PostgreSQL 18+ (user postgres)
- NuGet: Npgsql, System.Configuration.ConfigurationManager

## Cách kết nối PostgreSQL
1. Tạo database mới (ví dụ 	heaterdb):
   `ash
   createdb -h 127.0.0.1 -p 5432 -U postgres theaterdb
   `
2. Import schema:
   `ash
   psql -h 127.0.0.1 -p 5432 -U postgres -d theaterdb -f schema.sql
   `
3. Chuỗi kết nối (sửa Password cho đúng) trong src/TheaterTickets.App/App.config:
   `
   Host=127.0.0.1;Port=5432;Username=postgres;Password=YOUR_PASSWORD;Database=theaterdb
   `

## Giá ghế mặc định
- standard: 100,000
- vip: 150,000
- double: 180,000

## Cách chạy chương trình
- Mở solution TheaterTickets.sln bằng Visual Studio 2022+.
- Chọn cấu hình Debug và nhấn Run.

## Chức năng
- Quản lý suất diễn (frmPerformanceMaster): CRUD, tìm theo tên/khoảng thời gian, DataGridView, xác nhận xóa.
- Đặt vé (frmBooking): chọn suất, khách hàng, loại ghế, số lượng, tính tiền, xác nhận lưu, tìm suất.
- Gán ghế (frmSeatAssignment): sơ đồ 10x10, giới hạn số ghế, cảnh báo vượt, khóa ghế trùng booking khác, hiển thị ghế đã gán, xác nhận lưu.
- Báo cáo (frmSeatReport): thống kê số vé theo từng loại ghế cho mỗi suất.

## Giả định / giới hạn
- Sơ đồ ghế cố định 10×10 (A–J, 1–10).
- Giá ghế cố định theo loại, không theo vị trí/zone.
- Chưa có phiên đăng nhập/role; tất cả thao tác được phép.
- Thời gian hold ghế tạm chưa hỗ trợ; kiểm tra trùng dựa trên lưu DB và unique constraint.
- Kiểm thử thủ công cơ bản, chưa có test tự động.

## Tiến độ
- [x] DB schema
- [x] UI forms
- [x] Logic đặt vé & kiểm tra trùng
- [x] Báo cáo/bonus
- [ ] Kiểm thử + screenshot
