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

