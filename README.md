# TheaterTickets (VB.NET WinForms + PostgreSQL)

## Yêu cầu hệ thống
- .NET SDK 9.0
- PostgreSQL 12+ (có `pgcrypto`)
- NuGet: Npgsql, System.Configuration.ConfigurationManager

## Cách kết nối PostgreSQL
1. Tạo database mới (ví dụ `theaterdb`):
   ```bash
   createdb -h 127.0.0.1 -p 5432 -U postgres theaterdb
   ```
2. Import schema (file này sẽ DROP & CREATE lại toàn bộ bảng/data):
   ```bash
   psql -h 127.0.0.1 -p 5432 -U postgres -d theaterdb -f schema.sql
   ```
3. Chuỗi kết nối (sửa Password cho đúng) trong `src/TheaterTickets.App/App.config`:
   ```
   Host=127.0.0.1;Port=5432;Username=postgres;Password=YOUR_PASSWORD;Database=theaterdb
   ```

## Giá ghế mặc định
- standard: 100,000
- vip: 150,000
- double: 180,000

## Cách chạy chương trình
- Mở solution TheaterTickets.sln bằng Visual Studio 2022+.
- Chọn cấu hình Debug và nhấn Run.

## Chức năng
- Đăng nhập & phân quyền theo role.
- Quản lý suất diễn: CRUD, tìm theo tên/khoảng thời gian, DataGridView, xác nhận xóa.
- Đặt vé: chọn suất, khách hàng, loại ghế, số lượng, tính tiền, xác nhận lưu, tìm suất.
- Gán ghế: sơ đồ 10x10, giới hạn số ghế, cảnh báo vượt, khóa ghế trùng booking khác, hiển thị ghế đã gán, xác nhận lưu.
- Báo cáo ghế: thống kê số vé theo từng loại ghế cho mỗi suất.
- Quản lý người dùng: tạo/sửa user, gán role, bật/tắt tài khoản.

## Tài khoản mẫu
- admin / admin123 (ADMIN)
- staff1 / 123456 (STAFF)
- viewer1 / 123456 (VIEWER)

## Giả định / giới hạn
- Sơ đồ ghế cố định 10×10 (A–J, 1–10).
- Giá ghế cố định theo loại, không theo vị trí/zone.
- Thời gian hold ghế tạm chưa được hỗ trợ; kiểm tra trùng dựa trên lưu DB và unique constraint.
- Kiểm thử thủ công cơ bản, chưa có test tự động.

## Tiến độ
- [x] DB schema
- [x] UI forms
- [x] Logic đặt vé & kiểm tra trùng
- [x] Báo cáo/bonus
