-- === Drop existing objects so script can rerun end-to-end ===
DROP VIEW IF EXISTS v_booking_seats;
DROP TABLE IF EXISTS seat_assignments CASCADE;
DROP TABLE IF EXISTS bookings CASCADE;
DROP TABLE IF EXISTS performances CASCADE;
DROP TABLE IF EXISTS role_permissions CASCADE;
DROP TABLE IF EXISTS user_roles CASCADE;
DROP TABLE IF EXISTS permissions CASCADE;
DROP TABLE IF EXISTS roles CASCADE;
DROP TABLE IF EXISTS users CASCADE;
DROP TYPE IF EXISTS seat_category CASCADE;
CREATE TABLE performances (
    id SERIAL PRIMARY KEY,
    title TEXT NOT NULL,
    starts_at TIMESTAMPTZ NOT NULL,
    duration_minutes INT NOT NULL CHECK (duration_minutes > 0),
    created_at TIMESTAMPTZ NOT NULL DEFAULT now()
);

CREATE TYPE seat_category AS ENUM ('standard','vip','double');

CREATE TABLE bookings (
    id SERIAL PRIMARY KEY,
    performance_id INT NOT NULL REFERENCES performances(id) ON DELETE CASCADE,
    customer_name TEXT NOT NULL,
    category seat_category NOT NULL,
    ticket_count INT NOT NULL CHECK (ticket_count > 0),
    total_amount NUMERIC(12,2) NOT NULL,
    created_at TIMESTAMPTZ NOT NULL DEFAULT now()
);

CREATE TABLE seat_assignments (
    id SERIAL PRIMARY KEY,
    booking_id INT NOT NULL REFERENCES bookings(id) ON DELETE CASCADE,
    performance_id INT NOT NULL REFERENCES performances(id) ON DELETE CASCADE,
    seat_row CHAR(1) NOT NULL CHECK (seat_row BETWEEN 'A' AND 'J'),
    seat_number INT NOT NULL CHECK (seat_number BETWEEN 1 AND 10),
    UNIQUE (performance_id, seat_row, seat_number),
    UNIQUE (booking_id, seat_row, seat_number)
);

CREATE VIEW v_booking_seats AS
SELECT b.id AS booking_id, b.customer_name, b.category, b.ticket_count,
       p.title, p.starts_at,
       sa.seat_row || sa.seat_number::text AS seat_code
FROM bookings b
JOIN performances p ON p.id = b.performance_id
LEFT JOIN seat_assignments sa ON sa.booking_id = b.id;


-- === Auth schema ===
CREATE EXTENSION IF NOT EXISTS pgcrypto;

CREATE TABLE users (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    username TEXT NOT NULL UNIQUE,
    full_name TEXT,
    password_hash TEXT NOT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMPTZ NOT NULL DEFAULT now()
);

CREATE TABLE roles (
    id SERIAL PRIMARY KEY,
    code TEXT NOT NULL UNIQUE,
    name TEXT NOT NULL
);

CREATE TABLE permissions (
    id SERIAL PRIMARY KEY,
    code TEXT NOT NULL UNIQUE,
    name TEXT NOT NULL
);

CREATE TABLE user_roles (
    user_id UUID NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    role_id INT NOT NULL REFERENCES roles(id) ON DELETE CASCADE,
    UNIQUE (user_id, role_id)
);

CREATE TABLE role_permissions (
    role_id INT NOT NULL REFERENCES roles(id) ON DELETE CASCADE,
    permission_id INT NOT NULL REFERENCES permissions(id) ON DELETE CASCADE,
    UNIQUE (role_id, permission_id)
);

-- Seed roles
INSERT INTO roles (code, name) VALUES
    ('ADMIN', 'Quản trị'),
    ('STAFF', 'Nhân viên bán vé'),
    ('VIEWER', 'Chỉ xem báo cáo')
ON CONFLICT (code) DO NOTHING;

-- Seed permissions
INSERT INTO permissions (code, name) VALUES
    ('PERFORMANCE_MANAGE', 'Quản lý suất diễn'),
    ('BOOKING_CREATE', 'Tạo booking'),
    ('SEAT_ASSIGN', 'Gán ghế'),
    ('REPORT_VIEW', 'Xem báo cáo'),
    ('USER_ADMIN', 'Quản lý người dùng')
ON CONFLICT (code) DO NOTHING;

-- Map permissions to roles
INSERT INTO role_permissions (role_id, permission_id)
SELECT r.id, p.id
FROM roles r
JOIN permissions p ON p.code IN ('PERFORMANCE_MANAGE','BOOKING_CREATE','SEAT_ASSIGN','REPORT_VIEW','USER_ADMIN')
WHERE r.code = 'ADMIN'
ON CONFLICT DO NOTHING;

INSERT INTO role_permissions (role_id, permission_id)
SELECT r.id, p.id
FROM roles r
JOIN permissions p ON p.code IN ('BOOKING_CREATE','REPORT_VIEW')
WHERE r.code = 'STAFF'
ON CONFLICT DO NOTHING;

INSERT INTO role_permissions (role_id, permission_id)
SELECT r.id, p.id
FROM roles r
JOIN permissions p ON p.code IN ('REPORT_VIEW')
WHERE r.code = 'VIEWER'
ON CONFLICT DO NOTHING;

-- Seed default admin user (password: admin123). Đổi ngay sau khi triển khai.
INSERT INTO users (username, full_name, password_hash)
VALUES ('admin', 'Administrator', crypt('admin123', gen_salt('bf')))
ON CONFLICT (username) DO NOTHING;

INSERT INTO user_roles (user_id, role_id)
SELECT u.id, r.id FROM users u, roles r
WHERE u.username = 'admin' AND r.code = 'ADMIN'
ON CONFLICT DO NOTHING;

-- Seed additional users for testing (password: 123456)
INSERT INTO users (username, full_name, password_hash)
VALUES
    ('staff1', 'Nhân viên 1', crypt('123456', gen_salt('bf'))),
    ('viewer1', 'Người xem báo cáo', crypt('123456', gen_salt('bf')))
ON CONFLICT (username) DO NOTHING;

INSERT INTO user_roles (user_id, role_id)
SELECT u.id, r.id FROM users u, roles r
WHERE u.username = 'staff1' AND r.code = 'STAFF'
ON CONFLICT DO NOTHING;

INSERT INTO user_roles (user_id, role_id)
SELECT u.id, r.id FROM users u, roles r
WHERE u.username = 'viewer1' AND r.code = 'VIEWER'
ON CONFLICT DO NOTHING;

-- === Sample data for app ===
INSERT INTO performances (title, starts_at, duration_minutes)
SELECT 'Kịch nói: Truyện cổ Việt', '2026-02-03 19:30:00+07', 120
WHERE NOT EXISTS (
    SELECT 1 FROM performances WHERE title = 'Kịch nói: Truyện cổ Việt' AND starts_at = '2026-02-03 19:30:00+07'
);

INSERT INTO performances (title, starts_at, duration_minutes)
SELECT 'Ca nhạc: Giai điệu mùa xuân', '2026-02-04 20:00:00+07', 100
WHERE NOT EXISTS (
    SELECT 1 FROM performances WHERE title = 'Ca nhạc: Giai điệu mùa xuân' AND starts_at = '2026-02-04 20:00:00+07'
);

INSERT INTO performances (title, starts_at, duration_minutes)
SELECT 'Múa rối nước: Đêm quê', '2026-02-05 18:30:00+07', 90
WHERE NOT EXISTS (
    SELECT 1 FROM performances WHERE title = 'Múa rối nước: Đêm quê' AND starts_at = '2026-02-05 18:30:00+07'
);

INSERT INTO bookings (performance_id, customer_name, category, ticket_count, total_amount)
SELECT p.id, 'Liên', 'standard', 1, 100000
FROM performances p
WHERE p.title = 'Kịch nói: Truyện cổ Việt' AND p.starts_at = '2026-02-03 19:30:00+07'
AND NOT EXISTS (
    SELECT 1 FROM bookings b
    WHERE b.performance_id = p.id AND b.customer_name = 'Liên'
      AND b.category = 'standard' AND b.ticket_count = 1 AND b.total_amount = 100000
);

INSERT INTO bookings (performance_id, customer_name, category, ticket_count, total_amount)
SELECT p.id, 'Hoàng Long', 'vip', 2, 300000
FROM performances p
WHERE p.title = 'Kịch nói: Truyện cổ Việt' AND p.starts_at = '2026-02-03 19:30:00+07'
AND NOT EXISTS (
    SELECT 1 FROM bookings b
    WHERE b.performance_id = p.id AND b.customer_name = 'Hoàng Long'
      AND b.category = 'vip' AND b.ticket_count = 2 AND b.total_amount = 300000
);

INSERT INTO bookings (performance_id, customer_name, category, ticket_count, total_amount)
SELECT p.id, 'Minh Anh', 'double', 2, 360000
FROM performances p
WHERE p.title = 'Ca nhạc: Giai điệu mùa xuân' AND p.starts_at = '2026-02-04 20:00:00+07'
AND NOT EXISTS (
    SELECT 1 FROM bookings b
    WHERE b.performance_id = p.id AND b.customer_name = 'Minh Anh'
      AND b.category = 'double' AND b.ticket_count = 2 AND b.total_amount = 360000
);

-- Sample seat assignments for the above bookings
INSERT INTO seat_assignments (booking_id, performance_id, seat_row, seat_number)
SELECT b.id, b.performance_id, 'A', 1
FROM bookings b
WHERE b.customer_name = 'Liên'
AND NOT EXISTS (
    SELECT 1 FROM seat_assignments sa
    WHERE sa.booking_id = b.id AND sa.seat_row = 'A' AND sa.seat_number = 1
);

INSERT INTO seat_assignments (booking_id, performance_id, seat_row, seat_number)
SELECT b.id, b.performance_id, 'A', 2
FROM bookings b
WHERE b.customer_name = 'Hoàng Long'
AND NOT EXISTS (
    SELECT 1 FROM seat_assignments sa
    WHERE sa.booking_id = b.id AND sa.seat_row = 'A' AND sa.seat_number = 2
);

INSERT INTO seat_assignments (booking_id, performance_id, seat_row, seat_number)
SELECT b.id, b.performance_id, 'A', 3
FROM bookings b
WHERE b.customer_name = 'Hoàng Long'
AND NOT EXISTS (
    SELECT 1 FROM seat_assignments sa
    WHERE sa.booking_id = b.id AND sa.seat_row = 'A' AND sa.seat_number = 3
);

INSERT INTO seat_assignments (booking_id, performance_id, seat_row, seat_number)
SELECT b.id, b.performance_id, 'B', 1
FROM bookings b
WHERE b.customer_name = 'Minh Anh'
AND NOT EXISTS (
    SELECT 1 FROM seat_assignments sa
    WHERE sa.booking_id = b.id AND sa.seat_row = 'B' AND sa.seat_number = 1
);

INSERT INTO seat_assignments (booking_id, performance_id, seat_row, seat_number)
SELECT b.id, b.performance_id, 'B', 2
FROM bookings b
WHERE b.customer_name = 'Minh Anh'
AND NOT EXISTS (
    SELECT 1 FROM seat_assignments sa
    WHERE sa.booking_id = b.id AND sa.seat_row = 'B' AND sa.seat_number = 2
);

-- More sample performances
INSERT INTO performances (title, starts_at, duration_minutes)
SELECT 'Kịch nói: Người trong bao', '2026-02-06 19:00:00+07', 110
WHERE NOT EXISTS (
    SELECT 1 FROM performances WHERE title = 'Kịch nói: Người trong bao' AND starts_at = '2026-02-06 19:00:00+07'
);

INSERT INTO performances (title, starts_at, duration_minutes)
SELECT 'Nhạc kịch: Giấc mơ xanh', '2026-02-07 20:00:00+07', 130
WHERE NOT EXISTS (
    SELECT 1 FROM performances WHERE title = 'Nhạc kịch: Giấc mơ xanh' AND starts_at = '2026-02-07 20:00:00+07'
);

INSERT INTO performances (title, starts_at, duration_minutes)
SELECT 'Hài kịch: Chuyện nhà Tèo', '2026-02-08 18:00:00+07', 95
WHERE NOT EXISTS (
    SELECT 1 FROM performances WHERE title = 'Hài kịch: Chuyện nhà Tèo' AND starts_at = '2026-02-08 18:00:00+07'
);

INSERT INTO performances (title, starts_at, duration_minutes)
SELECT 'Múa rối: Cổ tích phố', '2026-02-09 19:30:00+07', 80
WHERE NOT EXISTS (
    SELECT 1 FROM performances WHERE title = 'Múa rối: Cổ tích phố' AND starts_at = '2026-02-09 19:30:00+07'
);

-- More sample bookings
INSERT INTO bookings (performance_id, customer_name, category, ticket_count, total_amount)
SELECT p.id, 'Thảo Vy', 'standard', 2, 200000
FROM performances p
WHERE p.title = 'Kịch nói: Người trong bao' AND p.starts_at = '2026-02-06 19:00:00+07'
AND NOT EXISTS (
    SELECT 1 FROM bookings b
    WHERE b.performance_id = p.id AND b.customer_name = 'Thảo Vy'
      AND b.category = 'standard' AND b.ticket_count = 2 AND b.total_amount = 200000
);

INSERT INTO bookings (performance_id, customer_name, category, ticket_count, total_amount)
SELECT p.id, 'Quang Huy', 'vip', 1, 150000
FROM performances p
WHERE p.title = 'Kịch nói: Người trong bao' AND p.starts_at = '2026-02-06 19:00:00+07'
AND NOT EXISTS (
    SELECT 1 FROM bookings b
    WHERE b.performance_id = p.id AND b.customer_name = 'Quang Huy'
      AND b.category = 'vip' AND b.ticket_count = 1 AND b.total_amount = 150000
);

INSERT INTO bookings (performance_id, customer_name, category, ticket_count, total_amount)
SELECT p.id, 'Ngọc Hà', 'double', 1, 180000
FROM performances p
WHERE p.title = 'Nhạc kịch: Giấc mơ xanh' AND p.starts_at = '2026-02-07 20:00:00+07'
AND NOT EXISTS (
    SELECT 1 FROM bookings b
    WHERE b.performance_id = p.id AND b.customer_name = 'Ngọc Hà'
      AND b.category = 'double' AND b.ticket_count = 1 AND b.total_amount = 180000
);

INSERT INTO bookings (performance_id, customer_name, category, ticket_count, total_amount)
SELECT p.id, 'Gia Bảo', 'vip', 3, 450000
FROM performances p
WHERE p.title = 'Nhạc kịch: Giấc mơ xanh' AND p.starts_at = '2026-02-07 20:00:00+07'
AND NOT EXISTS (
    SELECT 1 FROM bookings b
    WHERE b.performance_id = p.id AND b.customer_name = 'Gia Bảo'
      AND b.category = 'vip' AND b.ticket_count = 3 AND b.total_amount = 450000
);

INSERT INTO bookings (performance_id, customer_name, category, ticket_count, total_amount)
SELECT p.id, 'Đức Minh', 'standard', 4, 400000
FROM performances p
WHERE p.title = 'Hài kịch: Chuyện nhà Tèo' AND p.starts_at = '2026-02-08 18:00:00+07'
AND NOT EXISTS (
    SELECT 1 FROM bookings b
    WHERE b.performance_id = p.id AND b.customer_name = 'Đức Minh'
      AND b.category = 'standard' AND b.ticket_count = 4 AND b.total_amount = 400000
);

INSERT INTO bookings (performance_id, customer_name, category, ticket_count, total_amount)
SELECT p.id, 'Lan Anh', 'double', 2, 360000
FROM performances p
WHERE p.title = 'Múa rối: Cổ tích phố' AND p.starts_at = '2026-02-09 19:30:00+07'
AND NOT EXISTS (
    SELECT 1 FROM bookings b
    WHERE b.performance_id = p.id AND b.customer_name = 'Lan Anh'
      AND b.category = 'double' AND b.ticket_count = 2 AND b.total_amount = 360000
);

-- Seat assignments for new bookings
INSERT INTO seat_assignments (booking_id, performance_id, seat_row, seat_number)
SELECT b.id, b.performance_id, 'C', 1
FROM bookings b
WHERE b.customer_name = 'Thảo Vy'
AND NOT EXISTS (
    SELECT 1 FROM seat_assignments sa
    WHERE sa.booking_id = b.id AND sa.seat_row = 'C' AND sa.seat_number = 1
);

INSERT INTO seat_assignments (booking_id, performance_id, seat_row, seat_number)
SELECT b.id, b.performance_id, 'C', 2
FROM bookings b
WHERE b.customer_name = 'Thảo Vy'
AND NOT EXISTS (
    SELECT 1 FROM seat_assignments sa
    WHERE sa.booking_id = b.id AND sa.seat_row = 'C' AND sa.seat_number = 2
);

INSERT INTO seat_assignments (booking_id, performance_id, seat_row, seat_number)
SELECT b.id, b.performance_id, 'C', 3
FROM bookings b
WHERE b.customer_name = 'Quang Huy'
AND NOT EXISTS (
    SELECT 1 FROM seat_assignments sa
    WHERE sa.booking_id = b.id AND sa.seat_row = 'C' AND sa.seat_number = 3
);

INSERT INTO seat_assignments (booking_id, performance_id, seat_row, seat_number)
SELECT b.id, b.performance_id, 'D', 1
FROM bookings b
WHERE b.customer_name = 'Ngọc Hà'
AND NOT EXISTS (
    SELECT 1 FROM seat_assignments sa
    WHERE sa.booking_id = b.id AND sa.seat_row = 'D' AND sa.seat_number = 1
);

INSERT INTO seat_assignments (booking_id, performance_id, seat_row, seat_number)
SELECT b.id, b.performance_id, 'D', 2
FROM bookings b
WHERE b.customer_name = 'Gia Bảo'
AND NOT EXISTS (
    SELECT 1 FROM seat_assignments sa
    WHERE sa.booking_id = b.id AND sa.seat_row = 'D' AND sa.seat_number = 2
);

INSERT INTO seat_assignments (booking_id, performance_id, seat_row, seat_number)
SELECT b.id, b.performance_id, 'D', 3
FROM bookings b
WHERE b.customer_name = 'Gia Bảo'
AND NOT EXISTS (
    SELECT 1 FROM seat_assignments sa
    WHERE sa.booking_id = b.id AND sa.seat_row = 'D' AND sa.seat_number = 3
);

INSERT INTO seat_assignments (booking_id, performance_id, seat_row, seat_number)
SELECT b.id, b.performance_id, 'D', 4
FROM bookings b
WHERE b.customer_name = 'Gia Bảo'
AND NOT EXISTS (
    SELECT 1 FROM seat_assignments sa
    WHERE sa.booking_id = b.id AND sa.seat_row = 'D' AND sa.seat_number = 4
);

INSERT INTO seat_assignments (booking_id, performance_id, seat_row, seat_number)
SELECT b.id, b.performance_id, 'E', 1
FROM bookings b
WHERE b.customer_name = 'Đức Minh'
AND NOT EXISTS (
    SELECT 1 FROM seat_assignments sa
    WHERE sa.booking_id = b.id AND sa.seat_row = 'E' AND sa.seat_number = 1
);

INSERT INTO seat_assignments (booking_id, performance_id, seat_row, seat_number)
SELECT b.id, b.performance_id, 'E', 2
FROM bookings b
WHERE b.customer_name = 'Đức Minh'
AND NOT EXISTS (
    SELECT 1 FROM seat_assignments sa
    WHERE sa.booking_id = b.id AND sa.seat_row = 'E' AND sa.seat_number = 2
);

INSERT INTO seat_assignments (booking_id, performance_id, seat_row, seat_number)
SELECT b.id, b.performance_id, 'E', 3
FROM bookings b
WHERE b.customer_name = 'Đức Minh'
AND NOT EXISTS (
    SELECT 1 FROM seat_assignments sa
    WHERE sa.booking_id = b.id AND sa.seat_row = 'E' AND sa.seat_number = 3
);

INSERT INTO seat_assignments (booking_id, performance_id, seat_row, seat_number)
SELECT b.id, b.performance_id, 'E', 4
FROM bookings b
WHERE b.customer_name = 'Đức Minh'
AND NOT EXISTS (
    SELECT 1 FROM seat_assignments sa
    WHERE sa.booking_id = b.id AND sa.seat_row = 'E' AND sa.seat_number = 4
);

INSERT INTO seat_assignments (booking_id, performance_id, seat_row, seat_number)
SELECT b.id, b.performance_id, 'F', 1
FROM bookings b
WHERE b.customer_name = 'Lan Anh'
AND NOT EXISTS (
    SELECT 1 FROM seat_assignments sa
    WHERE sa.booking_id = b.id AND sa.seat_row = 'F' AND sa.seat_number = 1
);

INSERT INTO seat_assignments (booking_id, performance_id, seat_row, seat_number)
SELECT b.id, b.performance_id, 'F', 2
FROM bookings b
WHERE b.customer_name = 'Lan Anh'
AND NOT EXISTS (
    SELECT 1 FROM seat_assignments sa
    WHERE sa.booking_id = b.id AND sa.seat_row = 'F' AND sa.seat_number = 2
);


